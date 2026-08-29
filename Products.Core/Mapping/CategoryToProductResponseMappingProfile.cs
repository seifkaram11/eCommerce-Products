using AutoMapper;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.Mapping;

public class CategoryToProductResponseMappingProfile:Profile
{
    public CategoryToProductResponseMappingProfile()
    {
        CreateMap<Category,CategoryResponse>();
    }
}
