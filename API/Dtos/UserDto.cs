namespace YamSoft.API.Dtos;

public class UserDto // Base class for user data transfer objects
{
    public required string Username { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UserAuthDto : UserDto
{
    public required string Password { get; set; }
    // Additional properties for authentication can be added here
}

public class UserLoginDto : UserAuthDto
{
    // Additional properties for login can be added here
}

public class UserRegisterDto : UserAuthDto
{
    // Additional properties for registration can be added here
}
