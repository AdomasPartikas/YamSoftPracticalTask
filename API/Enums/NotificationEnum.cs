using System.Runtime.Serialization;

namespace YamSoft.API.Enums;

public enum NotificationType
{
    [EnumMember(Value = "welcome")]
    Welcome,
    
    [EnumMember(Value = "login")]
    Login,
    
    [EnumMember(Value = "order_placed")]
    OrderPlaced,
    
    [EnumMember(Value = "payment_success")]
    PaymentSuccess,
    
    [EnumMember(Value = "system_notification")]
    SystemNotification
}

public enum NotificationStatus
{
    [EnumMember(Value = "pending")]
    Pending,
    
    [EnumMember(Value = "sent")]
    Sent,
    
    [EnumMember(Value = "failed")]
    Failed,
    
    [EnumMember(Value = "read")]
    Read
}