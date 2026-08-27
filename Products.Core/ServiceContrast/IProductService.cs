using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.ServiceContrast;

interface IProductService
{
    Task<IQueryable<ProductResponse>> RetrieveAllProductsAsync();
    Task<IQueryable<ProductResponse>> RetrieveSpecificProductsAsync(Func<Product, bool> func);
    Task<ProductResponse?> AddProductAsync(ProductAddRequest request);
    Task<ProductResponse?> UpdateProductAsync(ProductUpdateRequest product);
    Task<ProductResponse?> DeleteProductAsync(Guid id);
}
