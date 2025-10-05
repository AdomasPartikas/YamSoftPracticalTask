using AutoMapper;
using YamSoft.API.Dtos;
using YamSoft.API.Interfaces;
using YamSoft.API.Models;

namespace YamSoft.API.Services;

public class AuthService(IDatabaseService databaseService, IMapper mapper) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(UserRegisterDto userDto)
    {
        var username = userDto.Username;

        if (await databaseService.UserExistsAsync(username))
        {
            throw new Exception("User already exists");
        }

        var hashedPassword = HashPassword(userDto.Password);

        var createdUser = await databaseService.CreateUserAsync(username, hashedPassword);

        await databaseService.CreateNotificationAsync(createdUser.Id, Enums.NotificationType.Welcome, "Welcome to YamSoft Shop!");

        return new AuthResponse
        {
            Token = "dummy_token",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = mapper.Map<UserResponseDto>(createdUser)
        };
    }

    public async Task<AuthResponse> LoginAsync(UserLoginDto userDto)
    {
        var username = userDto.Username;

        if (!await databaseService.UserExistsAsync(username))
        {
            throw new Exception("User does not exist");
        }

        var user = await databaseService.GetUserByUsernameAsync(username);
        if (user == null || !VerifyPassword(userDto.Password, user.HashedPassword))
        {
            throw new Exception("Invalid credentials");
        }

        await databaseService.CreateNotificationAsync(user.Id, Enums.NotificationType.Login, "You have successfully logged in!");

        return new AuthResponse
        {
            Token = "dummy_token",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = mapper.Map<UserResponseDto>(user)
        };
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool VerifyPassword(string providedPassword, string existingHashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, existingHashedPassword);
    }
}
