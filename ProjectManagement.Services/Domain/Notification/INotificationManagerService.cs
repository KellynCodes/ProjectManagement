using ProjectManagement.Models.Entities.Domains.Notification;
using ProjectManagement.Models.Enums;
using ProjectManagement.Services.Domains.Notification.Dtos;
using ProjectManagement.Services.Utility;

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

    /// <summary>
    /// Mark notificaiton as read or unread
    /// </summary>
    /// <param name="notificationStatus">Enum to choose between unread and read. <see cref="NotificationStatus"/> </param>
    /// <returns></returns>
    Task<ServiceResponse<NotificationDto>> MarkNotificationAsReadOrUnreadAsync(NotificationStatus notificationStatus);
}