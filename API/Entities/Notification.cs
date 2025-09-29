using System.ComponentModel.DataAnnotations;
using YamSoft.API.Enums;

namespace YamSoft.API.Entities;

public class Notification
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    public User User { get; set; } = null!;
    
    [Required]
    public NotificationType Type { get; set; }
    
    [Required]
    public required string Message { get; set; }
    
    [Required]
    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
}