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
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Cart? Cart { get; set; }
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}