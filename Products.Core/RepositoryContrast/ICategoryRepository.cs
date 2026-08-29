using Products.Core.Entitys;

namespace Products.Core.RepositoryContrast;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategorysAsync();
    Task<IEnumerable<Category>> GetCategoryByConditionAsync(Func<Category,bool> func);
    Task<Category?> AddCategoryAsync(Category category);
    Task<bool> UpdateCategoryAsync(Category category);
    Task<Category?> DeleteCategoryAsync(Guid id);
    Task<int> SaveChangesAsync();
}
