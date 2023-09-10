using ProjectManagement.Services.Domain.Notification.Dtos;

namespace ProjectManagement.Worker.Services;
public interface INotificationReceivedService
{
    Task HandleEmailSendingEvent(SendEmailNotification command, CancellationToken cancellationToken);
    Task HandleEmailBroadcastEvent(SendBroadcastEmailNotification command, CancellationToken cancellationToken);
    Task HandleSmsSendingEvent(SendSmsNotification command, CancellationToken cancellationToken);
    Task HandleSmsBroadcastEvent(SendBroadcastSmsNotification command, CancellationToken cancellationToken);
}
