using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using YamSoft.API.Entities;
using YamSoft.API.Interfaces;

namespace YamSoft.API.Services;

public class JwtService(IConfiguration config) : IJwtService
{
    public Task<(string, DateTime)> GenerateToken(User user)
    {
        var jwtKey = config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
        var jwtIssuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
        var jwtAudience = config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");
        var jwtExpiryHours = int.Parse(config["Jwt:ExpiryHours"] ?? "1");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(jwtKey);
        var expiresAt = DateTime.UtcNow.AddHours(jwtExpiryHours);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            ]),
            Expires = expiresAt,
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return Task.FromResult((tokenHandler.WriteToken(token), expiresAt));
    }
}