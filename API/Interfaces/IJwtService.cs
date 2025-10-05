using YamSoft.API.Entities;

namespace YamSoft.API.Interfaces;

public interface IJwtService
{
    public Task<(string, DateTime)> GenerateToken(User user);
}
