using AutoMapper;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.Mapping;

public class BrandUpdateRequestToBrandMappingProfile :Profile
{
    public BrandUpdateRequestToBrandMappingProfile()
    {
        CreateMap<BrandUpdateRequest, Brand>();
    }
}
