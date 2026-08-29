using AutoMapper;
using FluentValidation;
using Products.Core.DTOs;
using Products.Core.Entitys;
using Products.Core.Enums;
using Products.Core.RepositoryContrast;
using Products.Core.ServiceContrast;

namespace Products.Core.Service;

public class CategoryService : ICategoryService
{
    IValidator<CategoryAddRequest> _categoryAddRequestValidator;
    IValidator<CategoryUpdateRequest> _categoryUpdateRequestValidator;
    ICategoryRepository _categoryRepository;
    IMapper _mapper;

    public CategoryService(IValidator<CategoryAddRequest> categoryAddRequest, IValidator<CategoryUpdateRequest> categoryUpdateRequest, ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryAddRequestValidator = categoryAddRequest;
        _categoryUpdateRequestValidator = categoryUpdateRequest;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryResponse?> AddCategoryAsync(CategoryAddRequest request)
    {
        if(request is null)return null;

        var validate=await _categoryAddRequestValidator.ValidateAsync(request);
        if(!validate.IsValid)
            return null;

        var category=_mapper.Map<Category>(request);
        var addRes=await _categoryRepository.AddCategoryAsync(category);
        int numOfRowsEffected=await _categoryRepository.SaveChangesAsync();
        return numOfRowsEffected>0?_mapper.Map<CategoryResponse>(addRes):null;
    }

    public async Task<CategoryResponse?> DeleteCategoryAsync(Guid id)
    {
        var product=await _categoryRepository.DeleteCategoryAsync(id);
        int numOfRowsEffected=await _categoryRepository.SaveChangesAsync();
        return  numOfRowsEffected>0? _mapper.Map<CategoryResponse>(product):null;
    }

    public async Task<IEnumerable<CategoryResponse>> FilteringAsync(
    string? name,
    Guid? ParentCategoryId,
    int? PageSize = 10, int? PageNum = 1,
    TypeOfSorted? typeOfSorted = TypeOfSorted.ASCENDING)
    {
        IEnumerable<CategoryResponse> responses;

        if(ParentCategoryId is not null)
        {
            var list=await _categoryRepository.GetCategoryByConditionAsync(_=>_.ParentCategoryId==ParentCategoryId);
            responses=list.Select(_=>_mapper.Map<CategoryResponse>(_));
        }
        else
        {
            var list=await _categoryRepository.GetCategorysAsync();
            responses=list.Select(_=>_mapper.Map<CategoryResponse>(_));
        }
        if(name is not null)
            responses=responses.Where(_=>_.Name==name);

        int totalOfRecoreds=responses.Count(),pageSize= PageSize ?? 10,numOfPages = (int)Math.Ceiling((double)totalOfRecoreds / pageSize),pageNum=PageNum ?? 1;
        responses=responses.Select(_=>{_.PageNumber=pageNum;_.NumberOfPage=numOfPages;_.totalNumOfRecoreds=totalOfRecoreds; return _;});
        responses=responses.Skip((pageNum-1)*pageSize).Take(pageSize);

        if(typeOfSorted is not null)
        {
            if(typeOfSorted==TypeOfSorted.DESCENDING)
            {
                responses=responses.OrderByDescending(_=>_.Name);
            }
            else
            {
                responses=responses.OrderBy(_=>_.Name);
            }
        }
        else
        {
            responses=responses.OrderBy(_=>_.Name);
        }
        return responses;
    }

    public async Task<IQueryable<CategoryResponse>> RetrieveAllCategorysAsync()
    {
        var products=await _categoryRepository.GetCategorysAsync();
        var res=products.Select(_=>_mapper.Map<CategoryResponse>(_));
        return res.AsQueryable();
    }

    public async Task<CategoryResponse?> RetrieveCategoryByIDAsync(Guid id)
    {
        var res=await _categoryRepository.GetCategoryByConditionAsync(_=>_.CategoryId==id);
        var theResponse=res.FirstOrDefault(_=>true);
        if(theResponse is null)return null;
        return _mapper.Map<CategoryResponse>(theResponse);
    }

    public async Task<CategoryResponse?> UpdateCategoryAsync(Guid id, CategoryUpdateRequest request)
    {
        if(request is null)return null;

        var validate=await _categoryUpdateRequestValidator.ValidateAsync(request);
        if(!validate.IsValid)
            return null;

        var category=_mapper.Map<Category>(request);
        category.CategoryId=id;
        var res=await _categoryRepository.UpdateCategoryAsync(category);
        if(!res) return null;
        int numOfRowsEffected=await _categoryRepository.SaveChangesAsync();
        if(numOfRowsEffected>0)
        {
            var returnValue=await _categoryRepository.GetCategoryByConditionAsync(_=>_.CategoryId==id);
            return _mapper.Map<CategoryResponse>(returnValue);
        }
        return null;
    }
}
