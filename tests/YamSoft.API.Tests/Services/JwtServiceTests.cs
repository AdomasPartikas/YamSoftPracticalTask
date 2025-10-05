using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using YamSoft.API.Entities;
using YamSoft.API.Services;

namespace YamSoft.API.Tests.Services;

public class JwtServiceTests
{
    private readonly Mock<IConfiguration> _configMock;
    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
        _configMock = new Mock<IConfiguration>();
        
        _configMock.Setup(x => x["Jwt:Key"]).Returns("this-is-a-secret-key-for-testing-with-minimum-256-bits-length");
        _configMock.Setup(x => x["Jwt:Issuer"]).Returns("YamSoft.API");
        _configMock.Setup(x => x["Jwt:Audience"]).Returns("YamSoft.Client");
        _configMock.Setup(x => x["Jwt:ExpiryHours"]).Returns("2");

        _jwtService = new JwtService(_configMock.Object);
    }

    [Fact]
    public async Task GenerateToken_ValidUser_ReturnsTokenAndExpiry()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            HashedPassword = "hashedpassword"
        };

        // Act
        var (token, expiresAt) = await _jwtService.GenerateToken(user);

        // Assert
        Assert.NotEmpty(token);
        Assert.True(expiresAt > DateTime.UtcNow);
        Assert.True(expiresAt <= DateTime.UtcNow.AddHours(2.1));
    }

    [Fact]
    public async Task GenerateToken_ValidUser_TokenContainsUserIdClaim()
    {
        // Arrange
        var user = new User
        {
            Id = 42,
            Username = "testuser",
            HashedPassword = "hashedpassword"
        };

        // Act
        var (token, _) = await _jwtService.GenerateToken(user);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");

        Assert.NotNull(userIdClaim);
        Assert.Equal("42", userIdClaim.Value);
    }

    [Fact]
    public async Task GenerateToken_ValidUser_TokenHasCorrectIssuerAndAudience()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            HashedPassword = "hashedpassword"
        };

        // Act
        var (token, _) = await _jwtService.GenerateToken(user);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        
        Assert.Equal("YamSoft.API", jwtToken.Issuer);
        Assert.Equal("YamSoft.Client", jwtToken.Audiences.First());
    }

    [Fact]
    public void GenerateToken_MissingJwtKey_ThrowsInvalidOperationException()
    {
        // Arrange
        _configMock.Setup(x => x["Jwt:Key"]).Returns((string?)null);

        var jwtServiceWithMissingKey = new JwtService(_configMock.Object);
        var user = new User { Id = 1, Username = "test", HashedPassword = "hash" };

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await jwtServiceWithMissingKey.GenerateToken(user));
    }

    [Fact]
    public void GenerateToken_MissingJwtIssuer_ThrowsInvalidOperationException()
    {
        // Arrange
        _configMock.Setup(x => x["Jwt:Issuer"]).Returns((string?)null);
        
        var jwtServiceWithMissingIssuer = new JwtService(_configMock.Object);
        var user = new User { Id = 1, Username = "test", HashedPassword = "hash" };

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await jwtServiceWithMissingIssuer.GenerateToken(user));
    }
}