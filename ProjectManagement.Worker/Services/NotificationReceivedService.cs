using ProjectManagement.Services.Domain.Notification.Dtos;
using ProjectManagement.Services.EmailProviders.Interfaces;
using ProjectManagement.Services.Utility;
using System.Net;

namespace ProjectManagement.Worker.Services;
public class NotificationReceivedService : INotificationReceivedService
{
    //private readonly ISmsClient _smsClient;
    private readonly ILogger<Worker> _logger;
    private readonly IEmailProviderService _emailProcessor;
    public NotificationReceivedService(IEmailProviderService emailProcessor, ILogger<Worker> logger = null)
    {
        _logger = logger;
        _emailProcessor = emailProcessor;
    }
    public Task HandleEmailBroadcastEvent(SendBroadcastEmailNotification command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task HandleEmailSendingEvent(SendEmailNotification command, CancellationToken cancellationToken)
    {
        EmailTemplateModel content = await _emailProcessor.ProcessEmailNotificationRequestAsync(command);
        HttpStatusCode statusCode = await _emailProcessor.SendMailAsync(content, "Jedra.com");
        if (statusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation($"Mail has been sent to {content.Email} on {DateTime.UtcNow.ToLongDateString()}.");

        }
    }

    public Task HandleSmsBroadcastEvent(SendBroadcastSmsNotification command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task HandleSmsSendingEvent(SendSmsNotification command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
