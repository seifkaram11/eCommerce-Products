using AutoMapper;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.Mapping;

class CategoryAddRequestToCategoryMappingProfile :Profile
{
    public CategoryAddRequestToCategoryMappingProfile()
    {
        CreateMap<CategoryAddRequest,Category>();
    }
}
