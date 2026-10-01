using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Products.Core.Entitys;
using Products.Core.RepositoryContrast;
using Products.Infrastructure.Data;

namespace Products.Infrastructure.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ProductDbContext _productDbContext;
    private readonly ILogger<CategoryRepository> _logger;

    public CategoryRepository(ProductDbContext productDbContext, ILogger<CategoryRepository> logger)
    {
        _productDbContext = productDbContext;
        _logger = logger;
    }

    public async Task<Category?> AddCategoryAsync(Category category)
    {
        if (category is null) return null;

        var res = await _productDbContext.Categories.AddAsync(category);
        return res.Entity;
    }

    public async Task<Category?> DeleteCategoryAsync(Guid id)
    {
        var category = await _productDbContext.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category is null) return null;

        var res = _productDbContext.Categories.Remove(category);
        return res.Entity;
    }

    public async Task<Category?> GetByIdAsync(Guid id, bool track = false)
    {
        IQueryable<Category> query = _productDbContext.Categories;
        if (!track) query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(c => c.CategoryId == id);
    }

    public async Task<IReadOnlyList<Category>> GetCategoryByConditionAsync(
        Expression<Func<Category, bool>> predicate)
    {
        return await _productDbContext.Categories
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Category>> GetCategorysAsync()
    {
        return await _productDbContext.Categories
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<(IReadOnlyList<Category> Items, int Total)> GetPagedAsync(
        string? name, Guid? parentCategoryId, bool descending, int page, int size)
    {
        var query = _productDbContext.Categories.AsNoTracking().AsQueryable();

        if (parentCategoryId is not null)
            query = query.Where(c => c.ParentCategoryId == parentCategoryId);

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(c => c.Name == name);

        var total = await query.CountAsync();

        query = descending
            ? query.OrderByDescending(c => c.Name)
            : query.OrderBy(c => c.Name);

        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (items, total);
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        if (category is null) return false;

        var rows = await _productDbContext.Categories
            .Where(c => c.CategoryId == category.CategoryId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, category.Name)
                .SetProperty(p => p.Description, category.Description)
                .SetProperty(p => p.ParentCategoryId, category.ParentCategoryId));

        return rows > 0;
    }

    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _productDbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to save category changes to the database.");
            throw;
        }
    }
}
