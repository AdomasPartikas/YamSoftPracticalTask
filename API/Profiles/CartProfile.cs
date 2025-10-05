using AutoMapper;
using YamSoft.API.Dtos;
using YamSoft.API.Entities;

namespace YamSoft.API.Profiles;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Cart, CartDto>();
        CreateMap<Cart, CartResponseDto>();
        CreateMap<CartItem, CartItemDto>();
        CreateMap<CartItem, CartItemResponseDto>();
        CreateMap<AddToCartDto, CartItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CartId, opt => opt.Ignore())
            .ForMember(dest => dest.AddedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Cart, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore());
    }
}