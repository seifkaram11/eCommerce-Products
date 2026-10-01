namespace Products.Core.RepositoryContrast;

using Products.Core.Entitys;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetBrandsAsync();
    Task<IEnumerable<Brand>> GetBrandByConditionAsync(Func<Brand,bool> func);
    Task<Brand?> AddBrandAsync(Brand brand);
    Task<bool> UpdateBrandAsync(Brand brand);
    Task<Brand?> DeleteBrandAsync(Guid id);
    Task<int> SaveChangesAsync();
}
