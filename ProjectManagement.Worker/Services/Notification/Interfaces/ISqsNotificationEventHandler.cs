namespace ProjectManagement.Worker.Services.Notification.Interfaces
{
    public interface ISqsNotificationEventHandler
    {
        Task PollMessagesForEmailBroadcastAsync(CancellationToken cancellationToken);
        Task PollMessagesForSmsBroadcastAsync(CancellationToken cancellationToken);
        Task PollMessagesForEmailSendingAsync(CancellationToken cancellationToken);
        Task PollMessagesForSmsSendingAsync(CancellationToken cancellationToken);
    }
}