using AutoMapper;
using YamSoft.API.Dtos;
using YamSoft.API.Interfaces;
using YamSoft.API.Models;

namespace YamSoft.API.Services;

public class AuthService(IDatabaseService databaseService, IMapper mapper) : IAuthService
{
    public AuthResponse Register(UserRegisterDto userDto)
    {
        var username = userDto.Username;

        if (!databaseService.UserExists(username)) // Changed to !UserExists for demo purposes *change back when connected to real DB*
        {
            throw new Exception("User already exists");
        }

        var hashedPassword = HashPassword(userDto.Password);

        var createdUser = databaseService.CreateUser(username, hashedPassword);

        return new AuthResponse
        {
            Token = "dummy_token",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = mapper.Map<UserDto>(createdUser)
        };
    }

    public AuthResponse Login(UserLoginDto userDto)
    {
        var username = userDto.Username;

        if (!databaseService.UserExists(username))
        {
            throw new Exception("User does not exist");
        }

        var user = databaseService.GetUserByUsername(username);
        var hashedPassword = HashPassword(userDto.Password); // Due to the absence of a real database, we hash the provided password for comparison
        if (!VerifyPassword(userDto.Password, hashedPassword))
        {
            throw new Exception("Invalid password");
        }

        return new AuthResponse
        {
            Token = "dummy_token",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = mapper.Map<UserDto>(user)
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
