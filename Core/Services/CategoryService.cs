using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Products.Core.CachedContrast;
using Products.Core.DTOs;
using Products.Core.Entitys;
using Products.Core.Enums;
using Products.Core.RepositoryContrast;
using Products.Core.ServiceContrast;

namespace Products.Core.Service;

public class CategoryService : ICategoryService
{
    private readonly IValidator<CategoryAddRequest> _categoryAddRequestValidator;
    private readonly IValidator<CategoryUpdateRequest> _categoryUpdateRequestValidator;
    private readonly ICachedCategory _cachedCategory;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(
        IValidator<CategoryAddRequest> categoryAddRequest,
        IValidator<CategoryUpdateRequest> categoryUpdateRequest,
        ICategoryRepository categoryRepository,
        IMapper mapper,
        ICachedCategory cachedCategory,
        ILogger<CategoryService> logger)
    {
        _categoryAddRequestValidator = categoryAddRequest;
        _categoryUpdateRequestValidator = categoryUpdateRequest;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _cachedCategory = cachedCategory;
        _logger = logger;
    }

    public async Task<CategoryResponse?> AddCategoryAsync(CategoryAddRequest request)
    {
        if (request is null) return null;

        var validation = await _categoryAddRequestValidator.ValidateAsync(request);
        if (!validation.IsValid) return null;

        var category = _mapper.Map<Category>(request);

        if (!await IsValidParentAsync(categoryId: null, category.ParentCategoryId))
            return null;

        var added = await _categoryRepository.AddCategoryAsync(category);
        if (added is null) return null;

        var rows = await _categoryRepository.SaveChangesAsync();
        if (rows <= 0) return null;

        await TryCacheAsync(() => _cachedCategory.AddCategoryAsync(added), "add", added.CategoryId);

        return _mapper.Map<CategoryResponse>(added);
    }

    public async Task<CategoryResponse?> DeleteCategoryAsync(Guid id)
    {
        var deleted = await _categoryRepository.DeleteCategoryAsync(id);
        if (deleted is null) return null;

        var rows = await _categoryRepository.SaveChangesAsync();
        if (rows <= 0) return null;

        await TryCacheAsync(() => _cachedCategory.DeleteCategoryAsync(id), "delete", id);

        return _mapper.Map<CategoryResponse>(deleted);
    }

    public async Task<CategoryResponse?> UpdateCategoryAsync(Guid id, CategoryUpdateRequest request)
    {
        if (request is null) return null;

        var validation = await _categoryUpdateRequestValidator.ValidateAsync(request);
        if (!validation.IsValid) return null;

        var existing = await _categoryRepository.GetByIdAsync(id);
        if (existing is null) return null;

        var category = _mapper.Map<Category>(request);
        category.CategoryId = id;

        if (!await IsValidParentAsync(id, category.ParentCategoryId))
            return null;
        var updated = await _categoryRepository.UpdateCategoryAsync(category);
        if (!updated) return null;

        var result = await _categoryRepository.GetByIdAsync(id);
        if (result is null) return null;
        await _cachedCategory.UpdateCategoryAsync(id,category);

        return _mapper.Map<CategoryResponse>(result);
    }

    public async Task<CategoryResponse?> RetrieveCategoryByIDAsync(Guid id)
    {
        Category? cached = null;
        try
        {
            cached = await _cachedCategory.RetrieveCategoryByIDAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Cache read failed for category {CategoryId}", id);
        }

        if (cached is not null)
            return _mapper.Map<CategoryResponse>(cached);

        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null) return null;

        await TryCacheAsync(() => _cachedCategory.AddCategoryAsync(category), "populate", id);

        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<IEnumerable<CategoryResponse>> RetrieveAllCategorysAsync()
    {
        var categories=await _cachedCategory.RetrieveAllCategoriesAsync();

        if(categories is not null)
            return categories.Select(c => _mapper.Map<CategoryResponse>(c));

        categories = await _categoryRepository.GetCategorysAsync();
        return categories.Select(c => _mapper.Map<CategoryResponse>(c)).ToList();
    }

    public async Task<IEnumerable<CategoryResponse>> FilteringAsync(
    string? name,
    Guid? ParentCategoryId,
    int? PageSize = 10,
    int? PageNum = 1,
    TypeOfSorted? typeOfSorted = TypeOfSorted.ASCENDING)
    {
        var size = Math.Clamp(PageSize ?? 10, 1, 100);
        var page = Math.Max(PageNum ?? 1, 1);
        var descending = typeOfSorted == TypeOfSorted.DESCENDING;

        var items = await _cachedCategory.FilteringAsync(name,ParentCategoryId,descending);

        if(items is null || !items.Any())
        {
            items = await _categoryRepository.GetCategorysAsync();
            foreach(var item in items)
            {
                await TryCacheAsync(() => _cachedCategory.AddCategoryAsync(item), "populate", item.CategoryId);
            }
        }

        if(!string.IsNullOrEmpty(name))
            if(ParentCategoryId is not null)
                items = items.Where(_=>_.Name.Contains(name ?? "", StringComparison.OrdinalIgnoreCase) && _.ParentCategoryId == ParentCategoryId);
            else
                items = items.Where(_=>_.Name.Contains(name ?? "", StringComparison.OrdinalIgnoreCase) && (_.ParentCategoryId == ParentCategoryId || ParentCategoryId is null));
        else
            if(ParentCategoryId is not null)
                items = items.Where(_=>_.ParentCategoryId == ParentCategoryId);

        if(descending)
            items = items.OrderByDescending(_=>_.Name);
        else
            items = items.OrderBy(_=>_.Name);

        var total_ = items.Count();

        var numOfPages_ = (int)Math.Ceiling(total_ / (double)size);

        items = items.Skip((page - 1) * size).Take(size);

        return items.Select(c =>
        {
            var response = _mapper.Map<CategoryResponse>(c);
            response.PageNumber = page;
            response.NumberOfPage = numOfPages_;
            response.totalNumOfRecoreds = total_;
            return response;
        });
    }

    private async Task TryCacheAsync(Func<Task> action, string operation, Guid id)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Cache {Operation} failed for category {CategoryId}", operation, id);
        }
    }

    private async Task<bool> IsValidParentAsync(Guid? categoryId, Guid? parentId)
    {
        if (parentId is null) return true;
        if (parentId == categoryId) return false;

        var visited = new HashSet<Guid>();
        var current = parentId;

        while (current is not null)
        {
            if (current == categoryId) return false;
            if (!visited.Add(current.Value)) return false;

            var node = await _categoryRepository.GetByIdAsync(current.Value);
            if (node is null) return false;

            current = node.ParentCategoryId;
        }

        return true;
    }
}
