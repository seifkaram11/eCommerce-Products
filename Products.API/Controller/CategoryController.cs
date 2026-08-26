using Microsoft.AspNetCore.Mvc;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.API.Containers;

[ApiController]
[Route("api/1/Categorys")]
public class CategoryController:ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetCategorys(CategoryAddRequest addRequest)
    {
        List<CategoryResponse> response=new();
        return Ok();
    }

    [HttpGet("/{id:guid}")]
    public async Task<ActionResult> GetCategoryById(Guid id)
    {
        CategoryResponse response=new();
        return Ok();
    }

    [HttpGet("search/{name:string}")]
    public async Task<ActionResult> GetCategoryByName(string name)
    {
        List<CategoryResponse> response=new();
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> AddProducts(CategoryAddRequest product)
    {
        CategoryResponse response=new();
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProducts(CategoryUpdateRequest updateRequest)
    {
        CategoryResponse response=new();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProducts(Guid id)
    {
        CategoryResponse response=new();
        return Ok();
    }
}
