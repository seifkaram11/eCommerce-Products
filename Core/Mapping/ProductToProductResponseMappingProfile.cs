using AutoMapper;
using Products.Core.DTOs;
using Products.Core.Entitys;

namespace Products.Core.Mapping;

public class ProductToProductResponseMappingProfile:Profile
{
    public ProductToProductResponseMappingProfile()
    {
        CreateMap<Product,ProductResponse>();
    }
}
