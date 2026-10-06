using Products.Core.CachedContrast;
using Products.Core.Entitys;
using Products.Redis.Data;
using StackExchange.Redis;

namespace Products.Redis.Cached;

public class CachedBrand : ICachedBrand
{
    private const string KeyPrefix = "Brand#";
    private const string IndexKey = "Brands:ids";
    private static readonly TimeSpan Ttl = TimeSpan.FromHours(1);

    private readonly IDatabase _db;

    public CachedBrand(Client client)
    {
        _db = client.GetDatabase();
    }

    private static string KeyOf(Guid id) => KeyPrefix + id;

    public async Task<bool> AddBrandAsync(Brand request)
    {
        if (request is null) return false;

        await WriteAsync(request);
        return true;
    }

    public async Task<Brand?> DeleteBrandAsync(Guid id)
    {
        var brand = await RetrieveBrandByIDAsync(id);

        await _db.KeyDeleteAsync(KeyOf(id));
        await _db.SetRemoveAsync(IndexKey, id.ToString());

        return brand;
    }

    public async Task<bool> IsBrandExistsAsync(Guid id)
    {
        return await _db.KeyExistsAsync(KeyOf(id));
    }

    public async Task<IQueryable<Brand>> RetrieveAllBrandsAsync(int? pageNum=1, int? pageSize=10)
    {
        var ids = await _db.SetMembersAsync(IndexKey);
        var brands = new List<Brand>();

        if(pageNum is null)pageNum=1;
        if(pageSize is null)pageSize=10;
        int PageNum=(int)pageNum!,PageSize=(int)pageSize!;
        ids=ids.Skip(PageSize*(PageNum-1)).Take(PageSize).ToArray();

        foreach (var member in ids)
        {
            if (!Guid.TryParse(member.ToString(), out var id))
                continue;

            var brand = await RetrieveBrandByIDAsync(id);
            if (brand is null)
            {
                await _db.SetRemoveAsync(IndexKey, member);
                continue;
            }

            brands.Add(brand);
        }

        if(brands.Count == 0)
        {
            await _db.KeyDeleteAsync(IndexKey);
        }
        return brands.AsQueryable();
    }

    public async Task<Brand?> RetrieveBrandByIDAsync(Guid id)
    {
        var entries = await _db.HashGetAllAsync(KeyOf(id));
        return Map(id, entries);
    }

    public async Task<Brand?> UpdateBrandAsync(Guid id, Brand request)
    {
        if (request is null) return null;
        if (!await IsBrandExistsAsync(id)) return null;

        var brand = new Brand
        {
            BrandId = id,
            Name = request.Name,
            Description = request.Description,
            LogoUrl = request.LogoUrl
        };

        await WriteAsync(brand);
        return brand;
    }

    private async Task WriteAsync(Brand brand)
    {
        var key = KeyOf(brand.BrandId);
        await _db.HashSetAsync(key, new[]
        {
            new HashEntry("Name", brand.Name),
            new HashEntry("Description", brand.Description ?? ""),
            new HashEntry("LogoUrl", brand.LogoUrl ?? "")
        });

        await _db.KeyExpireAsync(key, Ttl);
        await _db.SetAddAsync(IndexKey, brand.BrandId.ToString());
    }

    private static Brand? Map(Guid id, HashEntry[] entries)
    {
        if (entries.Length == 0) return null;

        var map = entries.ToDictionary(e => e.Name.ToString(), e => e.Value);

        if (!map.TryGetValue("Name", out var name) || name.IsNullOrEmpty)
            return null;

        return new Brand
        {
            BrandId = id,
            Name = name.ToString(),
            Description = map.TryGetValue("Description", out var d) && !d.IsNullOrEmpty? d.ToString(): null,
            LogoUrl = map.TryGetValue("LogoUrl", out var l) && !l.IsNullOrEmpty? l.ToString(): null
        };
    }

    public async Task<IEnumerable<Brand>> FilteringAsync(
        string? name,
        bool descending,
        int? pageNum=1, int? pageSize=10
        )
    {
        var categories = await RetrieveAllBrandsAsync(pageNum,pageSize);
        if (categories is null || !categories.Any())
            return Enumerable.Empty<Brand>();

        if (!string.IsNullOrWhiteSpace(name))
        {
            categories = categories.Where(c =>
                c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        categories = descending? categories.OrderByDescending(c => c.Name): categories.OrderBy(c => c.Name);

        return categories;
    }
}
