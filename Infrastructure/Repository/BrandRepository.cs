using Products.Core.Entitys;
using Products.Core.RepositoryContrast;
using Products.Infrastructure.Data;

namespace Products.Infrastructure.Repository;

class BrandRepository : IBrandRepository
{
    ProductDbContext _productDbContext;

    public BrandRepository(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<Brand?> AddBrandAsync(Brand brand)
    {
        if(brand is null)return null;

        var res=await _productDbContext.Brands.AddAsync(brand);
        return res.Entity;
    }

    public async Task<Brand?> DeleteBrandAsync(Guid id)
    {
        var brand = await _productDbContext.Brands.FindAsync(id);
        if(brand is null)return null;

        _productDbContext.Brands.Remove(brand);
        return brand;
    }

    public async Task<IEnumerable<Brand>> GetBrandByConditionAsync(Func<Brand, bool> func)
    {
        return _productDbContext.Brands.Where(func);
    }

    public async Task<IEnumerable<Brand>> GetBrandsAsync()
    {
        return _productDbContext.Brands;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _productDbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateBrandAsync(Brand brand)
    {
        if(brand is null)return false;

        _productDbContext.Brands.Update(brand);
        return true;
    }
}
