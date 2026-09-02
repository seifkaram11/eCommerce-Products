using Microsoft.EntityFrameworkCore;
using Products.Core.Entitys;
using Products.Core.RepositoryContrast;
using Products.Infrastructure.Data;

namespace Products.Infrastructure.Repository;

public class CategoryRepository : ICategoryRepository
{
    ProductDbContext _productDbContext;

    public CategoryRepository(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<Category?> AddCategoryAsync(Category category)
    {
        if(category is null)return null;

        var res=await _productDbContext.Categories.AddAsync(category);
        return res.Entity;
    }

    public async Task<Category?> DeleteCategoryAsync(Guid id)
    {
        var category=await _productDbContext.Categories.FirstOrDefaultAsync(_=>_.CategoryId==id);
        if(category is null) return null;
        var res=_productDbContext.Categories.Remove(category);
        return res.Entity;
    }

    public async Task<IEnumerable<Category>> GetCategoryByConditionAsync(Func<Category, bool> func)
    {
        return _productDbContext.Categories.Where(func);
    }

    public async Task<IEnumerable<Category>> GetCategorysAsync()
    {
        return _productDbContext.Categories;
    }

    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return _productDbContext.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"Inner: {ex.InnerException?.Message}");
            Console.WriteLine($"Inner Inner: {ex.InnerException?.InnerException?.Message}");

            throw;
        }
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        if(category is null)return false;

        var numOfRowsEffected=await _productDbContext.Categories.Where(_=>_.CategoryId==category.CategoryId).ExecuteUpdateAsync(set=>set.
            SetProperty(p=>p.Name,category.Name)
            .SetProperty(p=>p.Description,category.Description)
            .SetProperty(p=>p.ParentCategory,category.ParentCategory)
        );

        return numOfRowsEffected>0? true:false;
    }
}
