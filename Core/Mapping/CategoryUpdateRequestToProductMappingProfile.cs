using AutoMapper;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.Mapping;

public class CategoryUpdateRequestToCategoryMappingProfile :Profile
{
    public CategoryUpdateRequestToCategoryMappingProfile()
    {
        CreateMap<CategoryUpdateRequest, Category>();
    }
}
