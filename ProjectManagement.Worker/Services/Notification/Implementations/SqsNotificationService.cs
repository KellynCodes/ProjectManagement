using ProjectManagement.Services.Domain.Notification.Dtos;
using ProjectManagement.Services.EmailProviders.Interfaces;
using ProjectManagement.Services.Utility;
using ProjectManagement.Worker.Services.Notification.Interfaces;
using System.Net;

namespace ProjectManagement.Worker.Services.Notification.Implementations;
public class SqsNotificationService : ISqsNotificationService
{
    //private readonly ISmsClient _smsClient;
    private readonly ILogger<Worker> _logger;
    private readonly IEmailProviderService _emailProcessor;
    public SqsNotificationService(IEmailProviderService emailProcessor, ILogger<Worker> logger = null)
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
        HttpStatusCode statusCode = await _emailProcessor.SendMailByMailKitAsync(content);
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
