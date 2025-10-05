using AutoMapper;
using YamSoft.API.Dtos;
using YamSoft.API.Entities;

namespace YamSoft.API.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductResponseDto>();
        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}