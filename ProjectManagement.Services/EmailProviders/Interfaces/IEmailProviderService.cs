using ProjectManagement.Services.Domain.Notification.Dtos;
using ProjectManagement.Services.Utility;
using System.Net;

namespace ProjectManagement.Services.EmailProviders.Interfaces;
public interface IEmailProviderService
{
    Task<HttpStatusCode> SendMailByAwsSesAsync(EmailTemplateModel emailTemplate);
    Task<HttpStatusCode> SendMailByMailKitAsync(EmailTemplateModel model);
    Task<EmailTemplateModel> ProcessEmailNotificationRequestAsync(SendEmailNotification emailContent);
    Task<EmailTemplateModel> ProcessEmailNotificationRequestAsync(SendBroadcastEmailNotification emailContent);
}
