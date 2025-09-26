using YamSoft.API.Entities;

namespace YamSoft.API.Interfaces;

public interface IDatabaseService
{
    User CreateUser(string username, string hashedPassword);
    bool UserExists(string username);
    User GetUserByUsername(string username);
}