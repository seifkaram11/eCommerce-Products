using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.CachedContrast;

public interface ICachedCategory
{
    Task<bool> AddCategoryAsync(Category request);
    Task<Category?> DeleteCategoryAsync(Guid id);
    Task<IEnumerable<Category>> RetrieveAllCategoriesAsync(int? pageNum=1,int? pageSize=10);
    Task<Category?> RetrieveCategoryByIDAsync(Guid id);
    Task<Category?> UpdateCategoryAsync(Guid id, Category request);
    Task<bool> IsCategoryExistsAsync(Guid id);
    Task<IEnumerable<Category>> FilteringAsync(
    string? name,
    Guid? parentCategoryId,
    bool descending,
    int? pageNum=1, int? pageSize=10);
}
