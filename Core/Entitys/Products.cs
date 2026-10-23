namespace Products.Core.Entitys;

public class Product
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

    public Guid BrandId { get; set; }
    public virtual Brand Brand { get; set; } = null!;
}
