using AutoMapper;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.Mapping;

class ProductUpdateRequestToProductMappingProfile:Profile
{
    public ProductUpdateRequestToProductMappingProfile()
    {
        CreateMap<ProductUpdateRequest, Product>()
            .ForMember(dest => dest.CategoryId, op => op.MapFrom(src => src.Category))
            .ForMember(dest => dest.BrandId, op => op.MapFrom(src => src.Brand))
            .ForMember(dest => dest.Variants, op => op.Ignore())
            .ForMember(dest => dest.Images, op => op.Ignore())
            .ForMember(dest => dest.Brand, op => op.Ignore())
            .ForMember(dest => dest.Category, op => op.Ignore());
    }
}
