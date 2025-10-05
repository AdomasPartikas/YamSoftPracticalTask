using AutoMapper;
using YamSoft.API.Dtos;
using YamSoft.API.Entities;

namespace YamSoft.API.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.HashedPassword, opt => opt.Ignore())
            .ForMember(dest => dest.Cart, opt => opt.Ignore())
            .ForMember(dest => dest.Notifications, opt => opt.Ignore());

        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Cart, CartDto>();
        CreateMap<CartItem, CartItemDto>();
        CreateMap<AddToCartDto, CartItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CartId, opt => opt.Ignore())
            .ForMember(dest => dest.AddedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Cart, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore());

        CreateMap<Notification, NotificationDto>();
        CreateMap<CreateNotificationDto, Notification>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ProcessedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
    }
}

