using System.ComponentModel.DataAnnotations;

namespace YamSoft.API.Entities;

public class Notification
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    public User User { get; set; } = null!;
    
    [Required]
    public required string Type { get; set; }
    
    [Required]
    public required string Message { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
}