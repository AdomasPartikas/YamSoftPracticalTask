using AutoMapper;
using Moq;
using YamSoft.API.Dtos;
using YamSoft.API.Entities;
using YamSoft.API.Enums;
using YamSoft.API.Interfaces;
using YamSoft.API.Services;

namespace YamSoft.API.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IDatabaseService> _databaseServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _databaseServiceMock = new Mock<IDatabaseService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _mapperMock = new Mock<IMapper>();
        _authService = new AuthService(_databaseServiceMock.Object, _jwtServiceMock.Object, _mapperMock.Object);
    }

    #region RegisterAsync Tests

    [Fact]
    public async Task RegisterAsync_ValidUser_ReturnsAuthResponse()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = "newuser",
            Password = "password123"
        };

        var createdUser = new User
        {
            Id = 1,
            Username = "newuser",
            HashedPassword = "hashedpassword",
            CreatedAt = DateTime.UtcNow
        };

        var userResponseDto = new UserResponseDto
        {
            Id = 1,
            Username = "newuser",
            CreatedAt = createdUser.CreatedAt
        };

        var token = "jwt-token";
        var expiresAt = DateTime.UtcNow.AddHours(2);

        _databaseServiceMock.Setup(x => x.UserExistsAsync("newuser")).ReturnsAsync(false);
        _databaseServiceMock.Setup(x => x.CreateUserAsync("newuser", It.IsAny<string>())).ReturnsAsync(createdUser);
        _databaseServiceMock.Setup(x => x.CreateNotificationAsync(1, NotificationType.Welcome, "Welcome to YamSoft Shop!"))
            .ReturnsAsync(new Notification { Message = "Welcome to YamSoft Shop!" });
        _jwtServiceMock.Setup(x => x.GenerateToken(createdUser)).ReturnsAsync((token, expiresAt));
        _mapperMock.Setup(x => x.Map<UserResponseDto>(createdUser)).Returns(userResponseDto);

        // Act
        var result = await _authService.RegisterAsync(userRegisterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(token, result.Token);
        Assert.Equal(expiresAt, result.ExpiresAt);
        Assert.Equal(userResponseDto, result.User);

        // Verify method calls
        _databaseServiceMock.Verify(x => x.UserExistsAsync("newuser"), Times.Once);
        _databaseServiceMock.Verify(x => x.CreateUserAsync("newuser", It.IsAny<string>()), Times.Once);
        _databaseServiceMock.Verify(x => x.CreateNotificationAsync(1, NotificationType.Welcome, "Welcome to YamSoft Shop!"), Times.Once);
        _jwtServiceMock.Verify(x => x.GenerateToken(createdUser), Times.Once);
        _mapperMock.Verify(x => x.Map<UserResponseDto>(createdUser), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_UserAlreadyExists_ThrowsException()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = "existinguser",
            Password = "password123"
        };

        _databaseServiceMock.Setup(x => x.UserExistsAsync("existinguser")).ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () => 
            await _authService.RegisterAsync(userRegisterDto));

        Assert.Equal("User already exists", exception.Message);

        // Verify
        _databaseServiceMock.Verify(x => x.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region LoginAsync Tests

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsAuthResponse()
    {
        // Arrange
        var userLoginDto = new UserLoginDto
        {
            Username = "existinguser",
            Password = "password123"
        };

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var existingUser = new User
        {
            Id = 1,
            Username = "existinguser",
            HashedPassword = hashedPassword,
            CreatedAt = DateTime.UtcNow
        };

        var userResponseDto = new UserResponseDto
        {
            Id = 1,
            Username = "existinguser",
            CreatedAt = existingUser.CreatedAt
        };

        var token = "jwt-token";
        var expiresAt = DateTime.UtcNow.AddHours(2);

        _databaseServiceMock.Setup(x => x.UserExistsAsync("existinguser")).ReturnsAsync(true);
        _databaseServiceMock.Setup(x => x.GetUserByUsernameAsync("existinguser")).ReturnsAsync(existingUser);
        _databaseServiceMock.Setup(x => x.CreateNotificationAsync(1, NotificationType.Login, "You have successfully logged in!"))
            .ReturnsAsync(new Notification { Message = "You have successfully logged in!" });
        _jwtServiceMock.Setup(x => x.GenerateToken(existingUser)).ReturnsAsync((token, expiresAt));
        _mapperMock.Setup(x => x.Map<UserResponseDto>(existingUser)).Returns(userResponseDto);

        // Act
        var result = await _authService.LoginAsync(userLoginDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(token, result.Token);
        Assert.Equal(expiresAt, result.ExpiresAt);
        Assert.Equal(userResponseDto, result.User);

        // Verify
        _databaseServiceMock.Verify(x => x.UserExistsAsync("existinguser"), Times.Once);
        _databaseServiceMock.Verify(x => x.GetUserByUsernameAsync("existinguser"), Times.Once);
        _databaseServiceMock.Verify(x => x.CreateNotificationAsync(1, NotificationType.Login, "You have successfully logged in!"), Times.Once);
        _jwtServiceMock.Verify(x => x.GenerateToken(existingUser), Times.Once);
        _mapperMock.Verify(x => x.Map<UserResponseDto>(existingUser), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_UserDoesNotExist_ThrowsException()
    {
        // Arrange
        var userLoginDto = new UserLoginDto
        {
            Username = "nonexistentuser",
            Password = "password123"
        };

        _databaseServiceMock.Setup(x => x.UserExistsAsync("nonexistentuser")).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () => 
            await _authService.LoginAsync(userLoginDto));

        Assert.Equal("User does not exist", exception.Message);

        // Verify
        _databaseServiceMock.Verify(x => x.GetUserByUsernameAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsException()
    {
        // Arrange
        var userLoginDto = new UserLoginDto
        {
            Username = "existinguser",
            Password = "wrongpassword"
        };

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctpassword");
        var existingUser = new User
        {
            Id = 1,
            Username = "existinguser",
            HashedPassword = hashedPassword,
            CreatedAt = DateTime.UtcNow
        };

        _databaseServiceMock.Setup(x => x.UserExistsAsync("existinguser")).ReturnsAsync(true);
        _databaseServiceMock.Setup(x => x.GetUserByUsernameAsync("existinguser")).ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () => 
            await _authService.LoginAsync(userLoginDto));

        Assert.Equal("Invalid credentials", exception.Message);

        // Verify
        _jwtServiceMock.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
        _databaseServiceMock.Verify(x => x.CreateNotificationAsync(It.IsAny<int>(), It.IsAny<NotificationType>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_UserIsNull_ThrowsException()
    {
        // Arrange
        var userLoginDto = new UserLoginDto
        {
            Username = "existinguser",
            Password = "password123"
        };

        _databaseServiceMock.Setup(x => x.UserExistsAsync("existinguser")).ReturnsAsync(true);
        _databaseServiceMock.Setup(x => x.GetUserByUsernameAsync("existinguser")).ReturnsAsync(null as User);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () => 
            await _authService.LoginAsync(userLoginDto));

        Assert.Equal("Invalid credentials", exception.Message);
    }

    #endregion
}