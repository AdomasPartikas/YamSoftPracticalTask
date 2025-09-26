using YamSoft.API.Dtos;

namespace YamSoft.API.Models;

public class AuthResponse
{
    public required string Token { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public required UserDto User { get; set; }
}