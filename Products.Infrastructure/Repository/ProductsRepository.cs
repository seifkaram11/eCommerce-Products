using Microsoft.EntityFrameworkCore;
using Products.Core.Entitys;
using Products.Core.RepositoryContrast;
using Products.Infrastructure.Data;

namespace Products.Infrastructure.Repository;

class ProductsRepository : IProductsRepository
{
    ProductDbContext _productDbContext;

    public ProductsRepository(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<Product?> AddProductAsync(Product product)
    {
        if(product is null)return null;

        var res=await _productDbContext.Products.AddAsync(product);
        return res.Entity;
    }

    public async Task<Product?> DeleteProductAsync(Guid id)
    {
        var product=await _productDbContext.Products.FirstOrDefaultAsync(_=>_.ProductId==id);
        if(product is null) return null;
        var res=_productDbContext.Products.Remove(product);
        return res.Entity;
    }

    public async Task<IEnumerable<Product>> GetProductByConditionAsync(Func<Product, bool> func)
    {
        return _productDbContext.Products.Where(func);
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return _productDbContext.Products;
    }

    public async Task<Product?> UpdateProductAsync(Product product)
    {
        if(product is null)return null;

        var numOfRowsEffected=await _productDbContext.Products.Where(_=>_.ProductId==product.ProductId).ExecuteUpdateAsync(setters => setters
            .SetProperty(p => p.Name, product.Name)
            .SetProperty(p => p.Price, product.Price)
            .SetProperty(p => p.Description, product.Description)
            .SetProperty(p => p.CategoryId, product.CategoryId)
            .SetProperty(p => p.BrandId, product.BrandId));

        return numOfRowsEffected==1? product:null;
    }

    public async Task<int> SaveChangesAsync()
    {
        return _productDbContext.SaveChanges();
    }
}
