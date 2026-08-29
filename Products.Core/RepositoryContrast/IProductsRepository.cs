using Products.Core.Entitys;

namespace Products.Core.RepositoryContrast;

public interface IProductsRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<IEnumerable<Product>> GetProductByConditionAsync(Func<Product,bool> func);
    Task<Product?> AddProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<Product?> DeleteProductAsync(Guid id);
    Task<int> SaveChangesAsync();
}
