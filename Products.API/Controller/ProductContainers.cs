using Microsoft.AspNetCore.Mvc;
using Products.Core.DTOs;
using Products.Core.Enums;
using Products.Core.ServiceContrast;

namespace Products.API.Containers;

[ApiController]
[Route("api/v1/Products")]
public class ProductController:ControllerBase
{
    IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetProductById(Guid id)
    {
        return Ok(await _productService.RetrieveProductByIDAsync(id));
    }

    [HttpGet]
    public async Task<ActionResult> GetProducts
    ([FromQuery]string? name,
    [FromQuery] Guid? categoryId,[FromQuery]Guid? brandId,
    [FromQuery] decimal? minPrice,[FromQuery]decimal? maxPrice,
    [FromQuery] int? PageSize=10,[FromQuery]int? PageNum=1,
    [FromQuery] TypeOfSorted? typeOfSorted=TypeOfSorted.ASCENDING,
    [FromQuery] SortOrder? sortOrder=SortOrder.Name)
    {
        return Ok(await _productService.FilteringAsync
        (name,
         categoryId, brandId,
          minPrice, maxPrice,
         PageSize, PageNum,
         typeOfSorted, sortOrder));
    }

    [HttpPost]
    public async Task<ActionResult> AddProduct(ProductAddRequest request)
    {
        var response=await _productService.AddProductAsync(request);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateProduct(Guid id,ProductUpdateRequest updateRequest)
    {
        var response=await _productService.UpdateProductAsync(id,updateRequest);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        var response=await _productService.DeleteProductAsync(id);
        return Ok(response);
    }
}
