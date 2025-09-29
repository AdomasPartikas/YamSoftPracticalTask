using YamSoft.API.Dtos;
using YamSoft.API.Models;

namespace YamSoft.API.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(UserRegisterDto userDto);
    Task<AuthResponse> LoginAsync(UserLoginDto userDto);
}
