using AutoMapper;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.Mapping;

class CategoryUpdateRequestToProductMappingProfile :Profile
{
    public CategoryUpdateRequestToProductMappingProfile()
    {
        CreateMap<CategoryUpdateRequest, Category>();
    }
}
