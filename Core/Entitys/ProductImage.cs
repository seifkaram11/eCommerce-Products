namespace Products.Core.Entitys;

public class ProductImage
{
    public Guid ProductImageId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
