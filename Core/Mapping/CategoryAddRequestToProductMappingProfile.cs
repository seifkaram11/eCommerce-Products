using AutoMapper;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.Mapping;

class CategoryAddRequestToProductMappingProfile :Profile
{
    public CategoryAddRequestToProductMappingProfile()
    {
        CreateMap<CategoryAddRequest,Category>();
    }
}
