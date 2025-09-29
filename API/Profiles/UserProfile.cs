using AutoMapper;
using YamSoft.API.Dtos;
using YamSoft.API.Entities;

namespace YamSoft.API.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // User mappings
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.HashedPassword, opt => opt.Ignore())
            .ForMember(dest => dest.Cart, opt => opt.Ignore())
            .ForMember(dest => dest.Notifications, opt => opt.Ignore());
    }
}

