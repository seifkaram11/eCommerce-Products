using Products.Core.Entitys;

namespace Products.Core.DTOs;

public class ProductResponse
{
    public Guid ProductId{get;set;}
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid CategoryId {get;set;}
    public Guid BrandId{get;set;}
    public ICollection<ProductVariant> Variants{ get; set; } = new List<ProductVariant>();

    public int? totalNumOfRecoreds{get;set;}
    public int? NumberOfPage{get;set;}
    public int? PageNumber{get;set;}
}
