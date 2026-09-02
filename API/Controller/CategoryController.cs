using Microsoft.AspNetCore.Mvc;
using Products.Core.DTOs;
using Products.Core.Enums;
using Products.Core.ServiceContrast;

namespace Products.API.Containers;

[ApiController]
[Route("api/V1/Categories")]
public class CategoryController:ControllerBase
{
    ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult> GetCategories
    ([FromQuery]string? name,
    [FromQuery]Guid? ParentCategoryId,
    [FromQuery] int? PageSize=10,[FromQuery]int? PageNum=1,
    [FromQuery] TypeOfSorted? typeOfSorted=TypeOfSorted.ASCENDING)
    {
        return Ok(await _categoryService.FilteringAsync
        (
            name,
            ParentCategoryId,
            PageSize, PageNum,
            typeOfSorted
        ));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetCategoryById(Guid id)
    {
        var response=await _categoryService.RetrieveCategoryByIDAsync(id);
        if(response is null)return BadRequest();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> AddCategory(CategoryAddRequest request)
    {
        var response=await _categoryService.AddCategoryAsync(request);
        if(response is null)return BadRequest();
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateCategory(Guid id,CategoryUpdateRequest updateRequest)
    {
        var response=await _categoryService.UpdateCategoryAsync(id,updateRequest);
        if(response is null)return BadRequest();
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        var response=await _categoryService.DeleteCategoryAsync(id);
        if(response is null)return BadRequest("the category is not found");
        return Ok(response);
    }
}
