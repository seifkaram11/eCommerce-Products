namespace Products.Core.Entitys;

public class ProductVariant
{
    public Guid ProductVariantId { get; set; }
    public string SKU { get; set; } = null!;
    public decimal Price { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
