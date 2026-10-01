namespace Products.Core.DTOs;

public class BrandUpdateRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
}
