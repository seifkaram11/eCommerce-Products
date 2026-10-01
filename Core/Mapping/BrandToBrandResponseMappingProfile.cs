using AutoMapper;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.Mapping;

public class BrandToBrandResponseMappingProfile:Profile
{
    public BrandToBrandResponseMappingProfile()
    {
        CreateMap<Brand,BrandResponse>();
    }
}
