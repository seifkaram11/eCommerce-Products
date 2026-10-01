using Products.Core.Entitys;

namespace Products.Core.CachedContrast;

public interface ICachedBrand
{
    Task<bool> AddBrandAsync(Brand request);
    Task<Brand?> DeleteBrandAsync(Guid id);
    Task<IQueryable<Brand>> RetrieveAllBrandsAsync();
    Task<Brand?> RetrieveBrandByIDAsync(Guid id);
    Task<Brand?> UpdateBrandAsync(Guid id, Brand request);
    Task<bool> IsBrandExistsAsync(Guid id);
    Task<IEnumerable<Brand>> FilteringAsync(string? name,bool descending);
}
