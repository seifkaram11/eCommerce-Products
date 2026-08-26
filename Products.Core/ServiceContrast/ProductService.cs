using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.ServiceContrast;

interface IProductService
{
    Task<IEnumerable<ProductResponse>> RetrieveAllProductsAsync();
    Task<IEnumerable<ProductResponse>> RetrieveSpecificProductsAsync(Func<Product, bool> func);
    Task<bool> AddProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(Guid id);

}
