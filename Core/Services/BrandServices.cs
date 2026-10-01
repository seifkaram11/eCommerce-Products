using AutoMapper;
using Microsoft.Extensions.Logging;
using Products.Core.CachedContrast;
using Products.Core.DTOs;
using Products.Core.Entitys;
using Products.Core.Enums;
using Products.Core.RepositoryContrast;
using Products.Core.ServiceContrast;

namespace Products.Core.Service;

public class BrandServices : IBrandServices
{
    private readonly IBrandRepository _brandRepository;
    private readonly ICachedBrand _cachedBrand;
    private readonly IMapper _mapper;
    private readonly ILogger<BrandServices> _logger;

    public BrandServices(
        IBrandRepository brandRepository,
        IMapper mapper,
        ICachedBrand cachedBrand,
        ILogger<BrandServices> logger)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
        _cachedBrand = cachedBrand;
        _logger = logger;
    }

    public async Task<BrandResponse?> AddBrandAsync(BrandAddRequest request)
    {
        if (request is null) return null;

        var brand = _mapper.Map<Brand>(request);
        var added = await _brandRepository.AddBrandAsync(brand);
        if (added is null) return null;

        int numOfRowsEffected = await _brandRepository.SaveChangesAsync();
        if (numOfRowsEffected <= 0) return null;

        await TryCacheAsync(() => _cachedBrand.AddBrandAsync(added), "add", added.BrandId);

        return _mapper.Map<BrandResponse>(added);
    }

    public async Task<BrandResponse?> DeleteBrandAsync(Guid id)
    {
        var brand = await _brandRepository.DeleteBrandAsync(id);
        if (brand is null) return null;

        int numOfRowsEffected = await _brandRepository.SaveChangesAsync();
        if (numOfRowsEffected <= 0) return null;

        await TryCacheAsync(() => _cachedBrand.DeleteBrandAsync(id), "delete", id);

        return _mapper.Map<BrandResponse>(brand);
    }

    public async Task<BrandResponse?> UpdateBrandAsync(Guid id, BrandUpdateRequest request)
    {
        if (request is null) return null;

        var rete = await _brandRepository.GetBrandByConditionAsync(_ => _.BrandId == id);
        var brand = rete.FirstOrDefault();
        if (brand is null) return null;

        _mapper.Map(request, brand);
        brand.BrandId = id;

        await _brandRepository.UpdateBrandAsync(brand);
        await _brandRepository.SaveChangesAsync();


        await TryCacheAsync(() => _cachedBrand.UpdateBrandAsync(id, brand), "update", id);

        return _mapper.Map<BrandResponse>(brand);
    }

    public async Task<BrandResponse?> RetrieveBrandByIDAsync(Guid id)
    {
        Brand? cached = null;
        try
        {
            cached = await _cachedBrand.RetrieveBrandByIDAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Cache read failed for brand {BrandId}", id);
        }

        if (cached is not null)
            return _mapper.Map<BrandResponse>(cached);

        var rete = await _brandRepository.GetBrandByConditionAsync(_ => _.BrandId == id);
        var brand = rete.FirstOrDefault();
        if (brand is null) return null;

        await TryCacheAsync(() => _cachedBrand.AddBrandAsync(brand), "populate", id);

        return _mapper.Map<BrandResponse>(brand);
    }

    public async Task<IQueryable<BrandResponse>> RetrieveAllBrandsAsync()
    {
        var cachedBrands = await _cachedBrand.RetrieveAllBrandsAsync();
        if (cachedBrands is not null && cachedBrands.Any())
            return cachedBrands.Select(_ => _mapper.Map<BrandResponse>(_));

        var list = await _brandRepository.GetBrandsAsync();
        return list.Select(_ => _mapper.Map<BrandResponse>(_)).ToList().AsQueryable();
    }

    public async Task<IEnumerable<BrandResponse>> FilteringAsync(
        string? name,
        int? PageSize = 10, int? PageNum = 1,
        TypeOfSorted? typeOfSorted = TypeOfSorted.ASCENDING)
    {
        var size = Math.Clamp(PageSize ?? 10, 1, 100);
        var page = Math.Max(PageNum ?? 1, 1);
        var descending = typeOfSorted == TypeOfSorted.DESCENDING;

        var items = await _cachedBrand.FilteringAsync(name,descending);

        if(items is null || !items.Any())
        {
            items = await _brandRepository.GetBrandsAsync();
            foreach(var item in items)
            {
                await TryCacheAsync(() => _cachedBrand.AddBrandAsync(item), "populate", item.BrandId);
            }
        }

        if(!string.IsNullOrEmpty(name))
                items = items.Where(_=>_.Name.Contains(name ?? "", StringComparison.OrdinalIgnoreCase));

        if(descending)
            items = items.OrderByDescending(_=>_.Name);
        else
            items = items.OrderBy(_=>_.Name);

        var total_ = items.Count();

        var numOfPages_ = (int)Math.Ceiling(total_ / (double)size);

        items = items.Skip((page - 1) * size).Take(size);

        return items.Select(c =>
        {
            var response = _mapper.Map<BrandResponse>(c);
            response.NumberOfPages = numOfPages_;
            response.totalNumOfRecoreds = total_;
            return response;
        });
    }

    private async Task TryCacheAsync(Func<Task> action, string operation, Guid id)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Cache {Operation} failed for brand {BrandId}", operation, id);
        }
    }
}
