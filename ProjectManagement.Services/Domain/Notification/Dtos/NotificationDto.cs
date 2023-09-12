using ProjectManagement.Models.Domains.Security.Enums;

namespace ProjectManagement.Models.Entities.Domains.Notification
{
    public record NotificationDto
    {
        public DateTimeOffset TimeStamp { get; set; }
        public string Message { get; set; } = string.Empty;
        public NotificationType NotificationType { get; set; }
    }
}
