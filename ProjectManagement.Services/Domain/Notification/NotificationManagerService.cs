using ProjectManagement.AsyncClient.Interfaces;
using ProjectManagement.Models.Configuration;
using ProjectManagement.Models.Entities.Domains.Notification;
using ProjectManagement.Models.Enums;
using ProjectManagement.Services.Domain.Notification.Dtos;
using ProjectManagement.Services.Domains.Notification.Dtos;
using ProjectManagement.Services.Domains.Security;
using ProjectManagement.Services.Utility;

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

    public async Task<ServiceResponse<NotificationDto>> MarkNotificationAsReadOrUnreadAsync(NotificationStatus notificationStatus)
    {
        throw new NotImplementedException();
    }

    private string GenerateSubject(OtpOperation operation)
    {
        switch (operation)
        {
            case OtpOperation.EmailConfirmation:
                return "Email Confirmation One-Time Password";
            case OtpOperation.PasswordReset:
                return "Password Reset One-Time Password";
            default:
                throw new ArgumentOutOfRangeException(nameof(operation));
        }
    }

}
