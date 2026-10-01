using System.Linq.Expressions;
using Products.Core.Entitys;

namespace Products.Core.RepositoryContrast;

public interface ICategoryRepository
{
    Task<Category?> AddCategoryAsync(Category category);
    Task<Category?> DeleteCategoryAsync(Guid id);
    Task<Category?> GetByIdAsync(Guid id, bool track = false);
    Task<IReadOnlyList<Category>> GetCategoryByConditionAsync(Expression<Func<Category, bool>> predicate);
    Task<IReadOnlyList<Category>> GetCategorysAsync();
    Task<(IReadOnlyList<Category> Items, int Total)> GetPagedAsync
    (string? name, Guid? parentCategoryId, bool descending, int page, int size);
    Task<bool> UpdateCategoryAsync(Category category);
    Task<int> SaveChangesAsync();
}
