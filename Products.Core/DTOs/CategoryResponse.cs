namespace Products.Core.DTOs;

public class CategoryResponse
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid ParentCategory { get; set; }
}
