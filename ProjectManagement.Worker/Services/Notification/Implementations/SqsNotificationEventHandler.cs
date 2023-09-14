using ProjectManagement.AsyncClient.Interfaces;
using ProjectManagement.Models.Configuration;
using ProjectManagement.Services.Domain.Notification.Dtos;
using ProjectManagement.Worker.Services.Notification.Interfaces;

namespace ProjectManagement.Worker.Services.Notification.Implementations;
public class SqsNotificationEventHandler : ISqsNotificationEventHandler
{
    private readonly AppSetting _appSetting;
    private readonly Func<ICommandClient> _commandSenderFactory;
    private readonly ISqsNotificationService _notificationReceivedService;

    public SqsNotificationEventHandler(Func<ICommandClient> commandSenderFactory, ISqsNotificationService notificationReceivedService, AppSetting appSetting)
    {
        _commandSenderFactory = commandSenderFactory;
        _notificationReceivedService = notificationReceivedService;
        _appSetting = appSetting;
    }

    public async Task PollMessagesForEmailSendingAsync(CancellationToken cancellationToken)
    {
        ICommandClient commandSender = _commandSenderFactory();
        await commandSender.PollQueueAsync<SendEmailNotification>(_appSetting.QueueConfiguration.EmailQueueUrl,
        async (message, stoppingToken) =>
        {
            await _notificationReceivedService.HandleEmailSendingEvent(message!, stoppingToken);
        }, cancellationToken);
    }
    public async Task PollMessagesForEmailBroadcastAsync(CancellationToken cancellationToken)
    {
        ICommandClient commandSender = _commandSenderFactory();
        await commandSender.PollQueueAsync<SendBroadcastEmailNotification>(_appSetting.QueueConfiguration.EmailQueueUrl,
        async (message, stoppingToken) =>
        {
            await _notificationReceivedService.HandleEmailBroadcastEvent(message!, stoppingToken);
        }, cancellationToken);
    }
    public async Task PollMessagesForSmsSendingAsync(CancellationToken cancellationToken)
    {
        ICommandClient commandSender = _commandSenderFactory();
        await commandSender.PollQueueAsync<SendSmsNotification>(_appSetting.QueueConfiguration.EmailQueueUrl,
        async (message, stoppingToken) =>
        {
            await _notificationReceivedService.HandleSmsSendingEvent(message!, stoppingToken);
        }, cancellationToken);
    }
    public async Task PollMessagesForSmsBroadcastAsync(CancellationToken cancellationToken)
    {
        ICommandClient commandSender = _commandSenderFactory();
        await commandSender.PollQueueAsync<SendBroadcastSmsNotification>(_appSetting.QueueConfiguration.EmailQueueUrl,
        async (message, stoppingToken) =>
        {
            await _notificationReceivedService.HandleSmsBroadcastEvent(message!, stoppingToken);
        }, cancellationToken);
    }
}
