using ProjectManagement.Models.Domains.Security.Enums;

namespace ProjectManagement.Services.Domain.Notification.Dtos;

public class SendEmailNotification
{
    public string Subject { get; init; } = null!;
    public Personality To { get; set; } = null!;
    public bool IsTransactional { get; set; }
    public TimeSpan TTL { get; set; }
    public DateTime CommandSentAt { get; set; }
    public IList<Personality> CCs { get; set; }
    public IList<Personality> BCCs { get; set; }
    public string Content { get; set; } = null!;
    public string Source { get; set; } = null!;
    public string MessageId { get; set; } = null!;
    public NotificationType NotificationType { get; set; }
}
