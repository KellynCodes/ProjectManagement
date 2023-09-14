using ProjectManagement.AsyncClient.Interfaces;
using ProjectManagement.Models.Configuration;
using ProjectManagement.Models.Domains.Security.Enums;
using ProjectManagement.Models.Entities.Domains.Project;
using ProjectManagement.Models.Enums;
using ProjectManagement.Models.Identity;
using ProjectManagement.Services.Domain.Notification.Dtos;
using ProjectManagement.Services.Domain.User.Dto;
using ProjectManagement.Services.Domains.Notification.Dtos;
using ProjectManagement.Services.Domains.Security;

namespace ProjectManagement.Services.Domains.Notification;
public class NotificationManagerService : INotificationManagerService
{
    private readonly IOtpCodeService _otpCodeService;
    private readonly ICommandClient _commandClient;
    private readonly AppSetting _appSetting;
    private readonly string MessageSource = "Jadera.com";

    public NotificationManagerService(
        IOtpCodeService otpCodeService,
        ICommandClient commandClient,
        AppSetting appSetting)
    {
        _otpCodeService = otpCodeService;
        _commandClient = commandClient;
        _appSetting = appSetting;
    }

    public async Task CreateOtpNotificationAsync(CreateOtpNotificationDto model, CancellationToken cancellationToken)
    {
        string subject = GenerateSubject(model.operation);
        var otp = await _otpCodeService.GenerateOtpAsync(model.userId, model.operation);
        var messageId = SHA256Hasher.Hash($"{typeof(CreateOtpNotificationDto)}_{model.userId}_{DateTime.UtcNow.Date.ToShortDateString()}");
        SendEmailNotification command = new()
        {
            Source = MessageSource,
            Subject = subject,
            CommandSentAt = DateTime.UtcNow,
            Content = otp,
            IsTransactional = true,
            To = new Personality(model.email, model.fullName),
            TTL = TimeSpan.FromMinutes(5),
            MessageId = messageId
        };

        await _commandClient.SendCommand(_appSetting.QueueConfiguration.EmailQueueUrl, command, cancellationToken);
    }

    public async Task NotifyUserForAssignedTaskAsync(ApplicationUser user, ProjTask task, CancellationToken cancellationToken)
    {
        var messageId = SHA256Hasher.Hash($"NotifyUserForAssignedTask_{user.Id}_{DateTime.UtcNow.Date.ToShortDateString()}");
        string currentDirectory = Directory.GetCurrentDirectory();
        string folderPath = $"{currentDirectory}/wwwroot/HtmlTemplate/Notification";
        string fileName = "TaskAssignment.html";
        string htmlContent = Path.Combine(folderPath, fileName);
        var replacements = new Dictionary<string, string>
        {
           {"{userName}", user.UserName!},
           {"{title}", task.Title},
        };

        foreach (var placeholder in replacements.Keys)
        {
            htmlContent = htmlContent.Replace(placeholder, replacements[placeholder]);
        }

        string htmTemplate = "";
        SendEmailNotification command = new()
        {
            Source = MessageSource,
            Subject = "You have been assigned a new task.",
            CommandSentAt = DateTime.UtcNow,
            Content = htmTemplate,
            IsTransactional = true,
            To = new Personality(user.Email!, user.UserName!),
            TTL = TimeSpan.FromMinutes(5),
            MessageId = messageId
        };

        await _commandClient.SendCommand(_appSetting.QueueConfiguration.EmailQueueUrl, command, cancellationToken);
    }

    public async Task NotifyUserForCompletedTaskAsync(ApplicationUser user, ProjTask task, Status status, CancellationToken cancellationToken)
    {
        string messageId = SHA256Hasher.Hash($"NotifyUserForAssignedTask_{user.Id}_{DateTime.UtcNow.Date.ToShortDateString()}");
        string subject = GenerateSubject(NotificationType.StatusUpdate);
        string currentDirectory = Directory.GetCurrentDirectory();
        string folderPath = $"{currentDirectory}/wwwroot/HtmlTemplate/Notification";
        string fileName = "TaskUpdate.html";
        string htmlContent = Path.Combine(folderPath, fileName);
        var replacements = new Dictionary<string, string>
        {
           {"{userName}", user.UserName!},
           {"{title}", task.Title},
           {"{status}", task.Status.ToString()},
           {"{dateTime}", DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")},
        };

        foreach (var placeholder in replacements.Keys)
        {
            htmlContent = htmlContent.Replace(placeholder, replacements[placeholder]);
        }

        string htmTemplate = "";
        SendEmailNotification command = new()
        {
            Source = MessageSource,
            Subject = subject,
            CommandSentAt = DateTime.UtcNow,
            Content = htmTemplate,
            IsTransactional = true,
            To = new Personality(user.Email!, user.UserName!),
            TTL = TimeSpan.FromMinutes(5),
            MessageId = messageId
        };

        await _commandClient.SendCommand(_appSetting.QueueConfiguration.EmailQueueUrl, command, cancellationToken);
    }

    public async Task NotifyUserWhenTaskDueDateIsWithin48HrsAsync(UserResponseDto user, CancellationToken cancellationToken)
    {
        string messageId = SHA256Hasher.Hash($"NotifyUserForAssignedTask_{user.UserId}_{DateTime.UtcNow.Date.ToShortDateString()}");
        string subject = GenerateSubject(NotificationType.DueDateReminder);
        string currentDirectory = Directory.GetCurrentDirectory();
        string folderPath = $"{currentDirectory}/wwwroot/HtmlTemplate/Notification";
        string fileName = "TaskUpdate.html";
        string fullPath = Path.Combine(folderPath, fileName);
        string htmlContent = await File.ReadAllTextAsync(fullPath, cancellationToken);
        Dictionary<string, string> replacements = new();

        foreach (var task in user.Task)
        {
            if (task is null) continue;
            replacements = new Dictionary<string, string>
        {
           {"{userName}", user.UserName!},
           {"{title}", task.Title},
           {"{status}", task.Status.ToString()},
           {"{dateTime}", DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")},
        };
        }

        foreach (var placeholder in replacements.Keys)
        {
            htmlContent = htmlContent.Replace(placeholder, replacements[placeholder]);
        }

        SendEmailNotification command = new()
        {
            Source = MessageSource,
            Subject = subject,
            CommandSentAt = DateTime.UtcNow,
            Content = htmlContent,
            IsTransactional = true,
            To = new Personality(user.Email!, user.UserName!),
            TTL = TimeSpan.FromMinutes(5),
            MessageId = messageId
        };

        await _commandClient.SendCommand(_appSetting.QueueConfiguration.EmailQueueUrl, command, cancellationToken);
    }

    private static string GenerateSubject(NotificationType operation)
    {
        return operation switch
        {
            NotificationType.EmailConfirmation => "Email Confirmation One-Time Password",
            NotificationType.PasswordReset => "Password Reset One-Time Password",
            NotificationType.StatusUpdate => "Task update",
            NotificationType.DueDateReminder => "Task due date reminder.",
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        };
    }
}
