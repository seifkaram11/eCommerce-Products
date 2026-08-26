using Microsoft.AspNetCore.Mvc;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.API.Containers;

[ApiController]
[Route("api/1/Products")]
public class ProductController:ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetProduct(ProductAddRequest addRequest)
    {
        ProductResponse response=new();
        return Ok();
    }

    [HttpGet("/{id:guid}")]
    public async Task<ActionResult> GetProductById(Guid id)
    {
        ProductResponse response=new();
        return Ok();
    }

    [HttpGet("search/{name:string}")]
    public async Task<ActionResult> GetProductsByName(string name)
    {
        ProductResponse response=new();
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> AddProducts(ProductAddRequest product)
    {
        ProductResponse response=new();
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProducts(ProductUpdateRequest updateRequest)
    {
        ProductResponse response=new();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProducts(Guid id)
    {
        ProductResponse response=new();
        return Ok();
    }
}
