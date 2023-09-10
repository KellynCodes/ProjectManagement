namespace ProjectManagement.Worker.Services
{
    public interface INotificationReceivedEventHandler
    {
        Task PollMessagesForEmailBroadcastAsync(CancellationToken cancellationToken);
        Task PollMessagesForEmailSendingAsync(CancellationToken cancellationToken);
        Task PollMessagesForSmsBroadcastAsync(CancellationToken cancellationToken);
        Task PollMessagesForSmsSendingAsync(CancellationToken cancellationToken);
    }
}