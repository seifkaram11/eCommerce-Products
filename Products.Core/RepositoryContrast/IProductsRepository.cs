using Products.Core.Entitys;

namespace Products.Core.RepositoryContrast;

public interface IProductsRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product> GetProductByConditionAsync(Func<Product,bool> func);
    Task<bool> AddProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(Guid id);
}
