using ProjectManagement.Services.Domain.Notification.Dtos;
using ProjectManagement.Services.Utility;
using System.Net;

namespace ProjectManagement.Services.EmailProviders.Interfaces;
public interface IEmailProviderService
{
    Task<HttpStatusCode> SendMailAsync(EmailTemplateModel emailTemplate);
    Task<HttpStatusCode> SendMailAsync(EmailTemplateModel model, string cpName);
    Task<EmailTemplateModel> ProcessEmailNotificationRequestAsync(SendEmailNotification emailContent);
    Task<EmailTemplateModel> ProcessEmailNotificationRequestAsync(SendBroadcastEmailNotification emailContent);
}
