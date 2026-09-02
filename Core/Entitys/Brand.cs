namespace Products.Core.Entitys;

public class Brand
{
    public Guid BrandId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
