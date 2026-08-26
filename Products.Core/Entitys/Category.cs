namespace Products.Core.Entitys;

public class Category
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
