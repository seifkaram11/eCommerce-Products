using Products.Core.Entitys;
using Products.Core.RepositoryContrast;

namespace Products.Infrastructure.Repository;

class ProductsRepository : IProductsRepository
{
    public Task<bool> AddProductAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteProductAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetProductByConditionAsync(Func<Product, bool> func)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetProductsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateProductAsync(Product product)
    {
        throw new NotImplementedException();
    }
}
