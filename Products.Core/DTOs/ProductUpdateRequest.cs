namespace Products.Core.DTOs;

public class ProductUpdateRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Guid Category { get; set; }
    public Guid Brand{get;set;}
}
