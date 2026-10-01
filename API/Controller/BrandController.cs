using Microsoft.AspNetCore.Mvc;
using Products.Core.DTOs;
using Products.Core.Enums;
using Products.Core.ServiceContrast;

namespace Products.API.Containers;

[ApiController]
[Route("api/V1/Brands")]
public class BrandController:ControllerBase
{
    IBrandServices _brandServices;

    public BrandController(IBrandServices brandServices)
    {
        _brandServices = brandServices;
    }

    [HttpGet]
    public async Task<ActionResult> GetCategories
    ([FromQuery]string? name,
    [FromQuery]Guid? ParentCategoryId,
    [FromQuery] int? PageSize=10,[FromQuery]int? PageNum=1,
    [FromQuery] TypeOfSorted? typeOfSorted=TypeOfSorted.ASCENDING)
    {
        return Ok(await _brandServices.FilteringAsync
        (
            name,
            PageSize, PageNum,
            typeOfSorted
        ));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetCategoryById(Guid id)
    {
        var response=await _brandServices.RetrieveBrandByIDAsync(id);
        if(response is null)return BadRequest();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> AddCategory(BrandAddRequest request)
    {
        var response=await _brandServices.AddBrandAsync(request);
        if(response is null)return BadRequest();
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateCategory(Guid id,BrandUpdateRequest updateRequest)
    {
        var response=await _brandServices.UpdateBrandAsync(id,updateRequest);
        if(response is null)return BadRequest();
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        var response=await _brandServices.DeleteBrandAsync(id);
        if(response is null)return BadRequest("the category is not found");
        return Ok(response);
    }
}
