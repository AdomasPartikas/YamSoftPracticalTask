using YamSoft.API.Enums;

namespace YamSoft.API.Dtos;

public class NotificationDto
{
    public int UserId { get; set; }
    public NotificationType Type { get; set; }
    public required string Message { get; set; }
    public NotificationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}

public class CreateNotificationDto : NotificationDto
{
    // Inherits all properties from NotificationDto
}

public class UpdateNotificationDto : NotificationDto
{
    // Inherits all properties from NotificationDto
}