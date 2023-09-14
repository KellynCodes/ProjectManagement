using ProjectManagement.Models.Entities.Domains.Project;
using ProjectManagement.Models.Enums;
using ProjectManagement.Models.Identity;
using ProjectManagement.Services.Domain.User.Dto;
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

    /// <summary>
    /// Mark notificaiton as read or unread
    /// </summary>
    /// <param name="notificationStatus">Enum to choose between unread and read. <see cref="NotificationStatus"/> </param>
    /// <returns></returns>
    Task NotifyUserForAssignedTaskAsync(ApplicationUser user, ProjTask task, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="task"></param>
    /// <param name="status"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task NotifyUserForCompletedTaskAsync(ApplicationUser user, ProjTask task, Status status, CancellationToken cancellationToken);

    /// <summary>
    /// Notify user when task is within 48hrs to be due.
    /// </summary>
    /// <param name="userAndTask"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task NotifyUserWhenTaskDueDateIsWithin48HrsAsync(UserResponseDto userAndTask, CancellationToken cancellationToken);
}