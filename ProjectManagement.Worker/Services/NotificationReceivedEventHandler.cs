using ProjectManagement.AsyncClient.Interfaces;
using ProjectManagement.Models.Configuration;
using ProjectManagement.Services.Domain.Notification.Dtos;

namespace ProjectManagement.Worker.Services;
public class NotificationReceivedEventHandler : INotificationReceivedEventHandler
{
    private readonly AppSetting _appSetting;
    private readonly Func<ICommandClient> _commandSenderFactory;
    private readonly INotificationReceivedService _notificationReceivedService;

    public NotificationReceivedEventHandler(Func<ICommandClient> commandSenderFactory, INotificationReceivedService notificationReceivedService, AppSetting appSetting)
    {
        _commandSenderFactory = commandSenderFactory;
        _notificationReceivedService = notificationReceivedService;
        _appSetting = appSetting;
    }

    public async Task PollMessagesForEmailSendingAsync(CancellationToken cancellationToken)
    {
        ICommandClient commandSender = _commandSenderFactory();
        await commandSender.PollQueueAsync<SendEmailNotification>(_appSetting.QueueConfiguration.NotificationQueueUrl, async (message, stoppingToken) =>
        {
            await _notificationReceivedService.HandleEmailSendingEvent(message, stoppingToken);
        }, cancellationToken);
    }
    public async Task PollMessagesForEmailBroadcastAsync(CancellationToken cancellationToken)
    {
        ICommandClient commandSender = _commandSenderFactory();
        await commandSender.PollQueueAsync<SendBroadcastEmailNotification>(_appSetting.QueueConfiguration.NotificationQueueUrl, async (message, stoppingToken) =>
        {
            await _notificationReceivedService.HandleEmailBroadcastEvent(message, stoppingToken);
        }, cancellationToken);
    }
    public async Task PollMessagesForSmsSendingAsync(CancellationToken cancellationToken)
    {
        ICommandClient commandSender = _commandSenderFactory();
        await commandSender.PollQueueAsync<SendSmsNotification>(_appSetting.QueueConfiguration.NotificationQueueUrl, async (message, stoppingToken) =>
        {
            await _notificationReceivedService.HandleSmsSendingEvent(message, stoppingToken);
        }, cancellationToken);
    }
    public async Task PollMessagesForSmsBroadcastAsync(CancellationToken cancellationToken)
    {
        ICommandClient commandSender = _commandSenderFactory();
        await commandSender.PollQueueAsync<SendBroadcastSmsNotification>(_appSetting.QueueConfiguration.NotificationQueueUrl, async (message, stoppingToken) =>
        {
            await _notificationReceivedService.HandleSmsBroadcastEvent(message, stoppingToken);
        }, cancellationToken);
    }
}
