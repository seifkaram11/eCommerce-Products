using Products.Core.DTOs;
using Products.Core.Enums;

namespace Products.Core.ServiceContrast;

public interface IProductService
{
    Task<IQueryable<ProductResponse>> RetrieveAllProductsAsync();
    Task<ProductResponse?> RetrieveProductByIDAsync(Guid id);
    Task<ProductResponse?> AddProductAsync(ProductAddRequest request);
    Task<ProductResponse?> UpdateProductAsync(Guid id,ProductUpdateRequest product);
    Task<ProductResponse?> DeleteProductAsync(Guid id);
    Task<IEnumerable<ProductResponse>> FilteringAsync
    (string? name,
    Guid? categoryId, Guid? brandId,
    decimal? minPrice,decimal? maxPrice,
    int? PageSize=10, int? PageNum=1,
    TypeOfSorted? typeOfSorted = TypeOfSorted.ASCENDING,
    SortOrder? sortOrder=SortOrder.Name);
}
