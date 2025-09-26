namespace YamSoft.API.Entities;

public class User
{
    public required string Username { get; set; }
    public required string HashedPassword { get; set; }
}