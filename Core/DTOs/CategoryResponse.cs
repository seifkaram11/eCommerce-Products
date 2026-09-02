namespace Products.Core.DTOs;

public class CategoryResponse
{
    public Guid CategoryId{get;set;}
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid ParentCategoryId { get; set; }

    public int? totalNumOfRecoreds{get;set;}
    public int? NumberOfPage{get;set;}
    public int? PageNumber{get;set;}
}
