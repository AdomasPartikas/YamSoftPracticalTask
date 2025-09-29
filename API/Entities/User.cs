using System.ComponentModel.DataAnnotations;

namespace YamSoft.API.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string HashedPassword { get; set; }
}