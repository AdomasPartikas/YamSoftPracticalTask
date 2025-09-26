using YamSoft.API.Entities;
using YamSoft.API.Interfaces;

namespace YamSoft.API.Services;

public class DatabaseService : IDatabaseService
{
    public User CreateUser(string username, string hashedPassword)
    {
        // Implementation to create a user in the database

        return new User { Username = username, HashedPassword = hashedPassword };
    }

    public bool UserExists(string username)
    {
        // Implementation to check if a user exists in the database

        return true;
    }

    public User GetUserByUsername(string username)
    {
        // Implementation to retrieve a user by username from the database

        return new User { Username = username, HashedPassword = "existing_hashed_password" };
    }
}
