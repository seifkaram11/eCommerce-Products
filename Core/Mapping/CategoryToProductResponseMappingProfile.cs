using AutoMapper;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.Mapping;

public class CategoryToCategoryResponseMappingProfile:Profile
{
    public CategoryToCategoryResponseMappingProfile()
    {
        CreateMap<Category,CategoryResponse>();
    }
}
