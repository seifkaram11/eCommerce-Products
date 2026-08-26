using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Products.Core.Entitys;

namespace Products.Core.DTOs;

public class ProductResponse
{
    public Guid ProductId{get;set;}
    public string ProductName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid Category {get;set;}
    public Guid Brand{get;set;}
    ICollection<ProductVariant> Variants{ get; set; } = new List<ProductVariant>();
}
