using Products.Core.CachedContrast;
using Products.Core.Entitys;
using Products.Redis.Data;
using StackExchange.Redis;

namespace Products.Redis.Cached;

public class CachedCategory : ICachedCategory
{
    private const string KeyPrefix = "Category#";
    private const string IndexKey = "Categories:ids";
    private static readonly TimeSpan Ttl = TimeSpan.FromHours(1);

    private readonly IDatabase _db;

    public CachedCategory(Client client)
    {
        _db = client.GetDatabase();
    }

    private static string KeyOf(Guid id) => KeyPrefix + id;

    public async Task<bool> AddCategoryAsync(Category request)
    {
        if (request is null) return false;

        await WriteAsync(request);
        return true;
    }

    public async Task<Category?> DeleteCategoryAsync(Guid id)
    {
        var category = await RetrieveCategoryByIDAsync(id);

        await _db.KeyDeleteAsync(KeyOf(id));
        await _db.SetRemoveAsync(IndexKey, id.ToString());

        return category;
    }

    public async Task<bool> IsCategoryExistsAsync(Guid id)
    {
        return await _db.KeyExistsAsync(KeyOf(id));
    }

    public async Task<IEnumerable<Category>> RetrieveAllCategoriesAsync(int? pageNum=1,int? pageSize=10)
    {
        var ids = await _db.SetMembersAsync(IndexKey);
        var categories = new List<Category>();

        if(pageNum is null)pageNum=1;
        if(pageSize is null)pageSize=10;
        int PageNum=(int)pageNum!,PageSize=(int)pageSize!;
        ids=ids.Skip(PageSize*(PageNum-1)).Take(PageSize).ToArray();

        foreach (var member in ids)
        {
            if (!Guid.TryParse(member.ToString(), out var id))
                continue;

            var category = await RetrieveCategoryByIDAsync(id);
            if (category is null)
            {
                await _db.SetRemoveAsync(IndexKey, member);
                continue;
            }

            categories.Add(category);
        }

        if(categories.Count == 0)
        {
            await _db.KeyDeleteAsync(IndexKey);
        }
        return categories;
    }

    public async Task<Category?> RetrieveCategoryByIDAsync(Guid id)
    {
        var entries = await _db.HashGetAllAsync(KeyOf(id));
        return Map(id, entries);
    }

    public async Task<Category?> UpdateCategoryAsync(Guid id, Category request)
    {
        if (request is null) return null;
        if (!await IsCategoryExistsAsync(id)) return null;

        var category = new Category
        {
            CategoryId = id,
            Name = request.Name,
            Description = request.Description,
            ParentCategoryId = request.ParentCategoryId
        };

        await WriteAsync(category);
        return category;
    }

    private async Task WriteAsync(Category category)
    {
        var key = KeyOf(category.CategoryId);

        await _db.HashSetAsync(key, new[]
        {
            new HashEntry("Name", category.Name),
            new HashEntry("Description", category.Description ?? ""),
            new HashEntry("ParentCategoryId", category.ParentCategoryId?.ToString() ?? "")
        });

        await _db.KeyExpireAsync(key, Ttl);
        await _db.SetAddAsync(IndexKey, category.CategoryId.ToString());
    }

    private static Category? Map(Guid id, HashEntry[] entries)
    {
        if (entries.Length == 0) return null;

        var map = entries.ToDictionary(e => e.Name.ToString(), e => e.Value);

        if (!map.TryGetValue("Name", out var name) || name.IsNullOrEmpty)
            return null;

        return new Category
        {
            CategoryId = id,
            Name = name.ToString(),
            Description = map.TryGetValue("Description", out var d) && !d.IsNullOrEmpty
                ? d.ToString(): null,
            ParentCategoryId = map.TryGetValue("ParentCategoryId", out var p)&& Guid.TryParse(p.ToString(), out var parentId)? parentId: null
        };
    }

    public async Task<IEnumerable<Category>> FilteringAsync(
    string? name,
    Guid? parentCategoryId,
    bool descending,
    int? pageNum=1, int? pageSize=10)
    {
        var categories = await RetrieveAllCategoriesAsync(pageNum,pageSize);
        if (categories is null || !categories.Any())
            return Enumerable.Empty<Category>();

        if (!string.IsNullOrWhiteSpace(name))
        {
            categories = categories.Where(c =>
                c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        if (parentCategoryId.HasValue)
        {
            categories = categories.Where(c =>
                c.ParentCategoryId == parentCategoryId);
        }

        categories = descending
            ? categories.OrderByDescending(c => c.Name)
            : categories.OrderBy(c => c.Name);

        return categories;
    }
}
