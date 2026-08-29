using Products.Core.DTOs;
using Products.Core.Enums;

namespace Products.Core.ServiceContrast;

public interface ICategoryService
{
    Task<IQueryable<CategoryResponse>> RetrieveAllCategorysAsync();
    Task<CategoryResponse?> RetrieveCategoryByIDAsync(Guid id);
    Task<CategoryResponse?> AddCategoryAsync(CategoryAddRequest request);
    Task<CategoryResponse?> UpdateCategoryAsync(Guid id,CategoryUpdateRequest request);
    Task<CategoryResponse?> DeleteCategoryAsync(Guid id);
    Task<IEnumerable<CategoryResponse>> FilteringAsync
    (string? name,
    Guid? ParentCategoryId,
    int? PageSize = 10, int? PageNum = 1,
    TypeOfSorted? typeOfSorted = TypeOfSorted.ASCENDING);
}
