using YamSoft.API.Dtos;
using YamSoft.API.Models;

namespace YamSoft.API.Interfaces;

public interface IAuthService
{
    public AuthResponse Register(UserRegisterDto userDto);
    public AuthResponse Login(UserLoginDto userDto);
}
