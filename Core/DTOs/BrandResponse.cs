namespace Products.Core.DTOs;

public class BrandResponse
{
    public Guid BrandId{get;set;}
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public int NumberOfPages { get; set; }
    public int totalNumOfRecoreds { get; set; }
}
