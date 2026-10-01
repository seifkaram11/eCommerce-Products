using Products.Core.DTOs;
using Products.Core.Enums;

namespace Products.Core.ServiceContrast;

public interface IBrandServices
{
    Task<IQueryable<BrandResponse>> RetrieveAllBrandsAsync();
    Task<BrandResponse?> RetrieveBrandByIDAsync(Guid id);
    Task<BrandResponse?> AddBrandAsync(BrandAddRequest request);
    Task<BrandResponse?> UpdateBrandAsync(Guid id, BrandUpdateRequest request);
    Task<BrandResponse?> DeleteBrandAsync(Guid id);
    Task<IEnumerable<BrandResponse>> FilteringAsync
    (string? name,
    int? PageSize = 10, int? PageNum = 1,
    TypeOfSorted? typeOfSorted = TypeOfSorted.ASCENDING);
}
