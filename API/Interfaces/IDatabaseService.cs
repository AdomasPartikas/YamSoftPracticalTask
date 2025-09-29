using YamSoft.API.Entities;

namespace YamSoft.API.Interfaces;

public interface IDatabaseService
{
    Task<User> CreateUserAsync(string username, string hashedPassword);
    Task<bool> UserExistsAsync(string username);
    Task<User> GetUserByUsernameAsync(string username);
    Task CreateNotificationAsync(int userId, string type, string message);
}