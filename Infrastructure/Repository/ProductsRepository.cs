using MassTransit;
using Microsoft.EntityFrameworkCore;
using Products.Core.Entitys;
using Products.Core.RepositoryContrast;
using Products.Infrastructure.Contracts;
using Products.Infrastructure.Data;

namespace Products.Infrastructure.Repository;

public class ProductsRepository : IProductsRepository
{
    ProductDbContext _productDbContext;
    IPublishEndpoint publisher;

    public ProductsRepository(ProductDbContext productDbContext, IPublishEndpoint publisher)
    {
        _productDbContext = productDbContext;
        this.publisher = publisher;
    }

    public async Task<Product?> AddProductAsync(Product product)
    {
        if(product is null)return null;

        var res=await _productDbContext.Products.AddAsync(product);
        product.Variants.Select(_=>
            publisher.Publish(new NewProduct(_.ProductId, _.ProductVariantId, product.Name, _.SKU, _.Price)));

        return res.Entity;
    }

    public async Task<Product?> DeleteProductAsync(Guid id)
    {
        var product=await _productDbContext.Products.FirstOrDefaultAsync(_=>_.ProductId==id);
        if(product is null) return null;

        var res=_productDbContext.Products.Remove(product);
        product.Variants.Select(_=>publisher.Publish(new DeleteProduct(_.ProductId, _.ProductVariantId)));

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

    public async Task<bool> UpdateProductAsync(Product product)
    {
        if(product is null)return false;

        var numOfRowsEffected=await _productDbContext.Products.Where(_=>_.ProductId==product.ProductId).ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Name, product.Name)
            .SetProperty(p => p.Description, product.Description)
            .SetProperty(p => p.CategoryId, product.CategoryId)
            .SetProperty(p => p.BrandId, product.BrandId));



        return numOfRowsEffected==1? true:false;
    }

     public async Task<bool> UpdateProductVariantAsync(ProductVariant productVariant)
    {
        if(productVariant is null)return false;

        var numOfRowsEffected=await _productDbContext.ProductVariants.Where(_=>_.ProductId==productVariant.ProductId).ExecuteUpdateAsync(set => set
            .SetProperty(p => p.SKU, productVariant.SKU)
            .SetProperty(p => p.Price, productVariant.Price));
        await publisher.Publish(new UpdateProduct(productVariant.ProductId ,productVariant.ProductVariantId, productVariant.Product.Name, productVariant.SKU, productVariant.Price));

        return numOfRowsEffected==1? true:false;
    }


    public async Task<int> SaveChangesAsync()
    {
        return _productDbContext.SaveChanges();
    }
}
