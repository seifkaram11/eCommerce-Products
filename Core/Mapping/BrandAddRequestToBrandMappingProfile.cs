using AutoMapper;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.Mapping;

class BrandAddRequestToBrandMappingProfile :Profile
{
    public BrandAddRequestToBrandMappingProfile()
    {
        CreateMap<BrandAddRequest,Brand>();
    }
}
