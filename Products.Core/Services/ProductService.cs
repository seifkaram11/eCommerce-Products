using AutoMapper;
using FluentValidation;
using Products.Core.DTOs;
using Products.Core.Entitys;
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

    public async Task<ProductResponse?> UpdateProductAsync(ProductUpdateRequest request)
    {
        if(request is null)return null;

        var validate=await _productUpdateRequestValidator.ValidateAsync(request);
        if(!validate.IsValid)
            return null;

        var product=_mapper.Map<Product>(request);
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

    public async Task<IQueryable<ProductResponse>> RetrieveSpecificProductsAsync(Func<Product, bool> func)
    {
        var products=await _productsRepository.GetProductByConditionAsync(func);
        var res=products.Select(_=>_mapper.Map<ProductResponse>(_));
        return res.AsQueryable();
    }
}
