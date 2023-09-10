using ProjectManagement.Services.Domains.Notification.Dtos;

namespace ProjectManagement.Services.Domains.Notification;
/// <summary>
/// Manages the generation, formatting and scheduling of sms/email notification sending via a queue based messaging system
/// </summary>
public interface INotificationManagerService
{
    /// <summary>
    /// Sends out otp emails to users
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateOtpNotificationAsync(CreateOtpNotificationDto model, CancellationToken cancellationToken);
}