using Products.Core.DTOs;
using Products.Core.Entitys;
using Products.Core.ServiceContrast;

namespace Products.Core.Service;

class ProductService : IProductService
{
    public Task<bool> AddProductAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteProductAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProductResponse>> RetrieveAllProductsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProductResponse>> RetrieveSpecificProductsAsync(Func<Product, bool> func)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateProductAsync(Product product)
    {
        throw new NotImplementedException();
    }
}
