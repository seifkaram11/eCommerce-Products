using AutoMapper;
using FluentValidation;
using Products.Core.DTOs;
using Products.Core.Entitys;
using Products.Core.Enums;
using Products.Core.RepositoryContrast;
using Products.Core.ServiceContrast;

namespace Products.Core.Service;

class ProductService : IProductService
{
    IProductsRepository _productsRepository;
    IValidator<ProductAddRequest> _productAddRequestValidator;
    IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
    IMapper _mapper;

    public ProductService(IProductsRepository productsRepository, IMapper mapper, IValidator<ProductAddRequest> productAddRequestValidator, IValidator<ProductUpdateRequest> productUpdateRequestValidator)
    {
        _productsRepository = productsRepository;
        _mapper = mapper;
        _productAddRequestValidator = productAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
    }

    public async Task<ProductResponse?> AddProductAsync(ProductAddRequest request)
    {
        if(request is null)return null;

        var validate=await _productAddRequestValidator.ValidateAsync(request);
        if(!validate.IsValid)
            return null;


        Product product=_mapper.Map<Product>(request);
        Product? addRes=await _productsRepository.AddProductAsync(product);
        int numOfRowsEffected=await _productsRepository.SaveChangesAsync();
        return numOfRowsEffected>0?_mapper.Map<ProductResponse>(addRes):null;
    }

    public async Task<ProductResponse?> DeleteProductAsync(Guid id)
    {
        var product=await _productsRepository.DeleteProductAsync(id);
        int numOfRowsEffected=await _productsRepository.SaveChangesAsync();
        return  numOfRowsEffected>0? _mapper.Map<ProductResponse>(product):null;
    }

    public async Task<ProductResponse?> UpdateProductAsync(Guid id,ProductUpdateRequest request)
    {
        if(request is null)return null;

        var validate=await _productUpdateRequestValidator.ValidateAsync(request);
        if(!validate.IsValid)
            return null;

        var product=_mapper.Map<Product>(request);
        product.ProductId=id;
        var res=await _productsRepository.UpdateProductAsync(product);
        int numOfRowsEffected=await _productsRepository.SaveChangesAsync();
        return numOfRowsEffected>0?_mapper.Map<ProductResponse>(res):null;
    }

    public async Task<IQueryable<ProductResponse>> RetrieveAllProductsAsync()
    {
        var products=await _productsRepository.GetProductsAsync();
        var res=products.Select(_=>_mapper.Map<ProductResponse>(_));
        return res.AsQueryable();
    }

    public async Task<ProductResponse?> RetrieveProductByIDAsync(Guid id)
    {
        var res=await _productsRepository.GetProductByConditionAsync(_=>_.ProductId==id);
        var theResponse=res.FirstOrDefault(_=>true);
        if(theResponse is null)return null;
        return _mapper.Map<ProductResponse>(theResponse);
    }

    public async Task<IEnumerable<ProductResponse>> FilteringAsync
    (string? name,
    Guid? categoryId, Guid? brandId,
    decimal? minPrice,decimal? maxPrice,
    int? PageSize=10, int? PageNum=1,
    TypeOfSorted? typeOfSorted = TypeOfSorted.ASCENDING,
    SortOrder? sortOrder=SortOrder.Name)
    {
        IEnumerable<ProductResponse> responses;
        if(categoryId is not null)
        {
            var lsit=await _productsRepository.GetProductByConditionAsync(_=>_.CategoryId==categoryId);
            responses=lsit.Select(_=>_mapper.Map<ProductResponse>(_));
        }
        else
        {
            var lsit=await _productsRepository.GetProductsAsync();
            responses=lsit.Select(_=>_mapper.Map<ProductResponse>(_));
        }

        if(brandId is not null)
            responses=responses.Where(_=>_.BrandId==brandId);

        if(name is not null)
            responses=responses.Where(_=>_.Name==name);

        if(minPrice is not null)
            responses=responses.Where(_=>_.Price>=minPrice);

        if(maxPrice is not null)
            responses=responses.Where(_=>_.Price<=maxPrice);

        int totalOfRecoreds=responses.Count(),pageSize= PageSize ?? 10,numOfPages=totalOfRecoreds/pageSize ,pageNum=PageNum ?? 1;
        responses=responses.Select(_=>{_.PageNumber=pageNum;_.NumberOfPage=numOfPages;_.totalNumOfRecoreds=totalOfRecoreds; return _;});
        responses=responses.Skip((pageNum-1)*pageSize).Take(pageSize);

        // responses=typeOfSorted is not null? responses : typeOfSorted==TypeOfSorted.DESCENDING? responses.OrderBy(_=>_.)

        if(typeOfSorted is not null)
        {
            if(typeOfSorted==TypeOfSorted.DESCENDING)
            {
                if(sortOrder is not null)
                {
                    if(sortOrder==SortOrder.Name)
                    {
                        responses=responses.OrderByDescending(_=>_.Name);
                    }
                    else
                    {
                        responses=responses.OrderByDescending(_=>_.Price);
                    }
                }
            }
            else
            {
                if(sortOrder is not null)
                {
                    if(sortOrder==SortOrder.Name)
                    {
                        responses=responses.OrderBy(_=>_.Name);
                    }
                    else
                    {
                        responses=responses.OrderBy(_=>_.Price);
                    }
                }
            }
        }
        else
        {
            if(sortOrder is not null)
            {
                if(sortOrder==SortOrder.Name)
                {
                    responses=responses.OrderBy(_=>_.Name);
                }
                else
                {
                    responses=responses.OrderBy(_=>_.Price);
                }
            }
        }
        return responses;
    }
}
