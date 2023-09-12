namespace ProjectManagement.Worker.Services.Notification.Interfaces
{
    internal interface ITaskNotificationEventHandler
    {
        Task PollForTaskDueDateWithin48Hrs(CancellationToken cancellationToken);
        Task PollForCompletedTask(CancellationToken cancellationToken);
        Task PollFor(CancellationToken cancellationToken);
    }
}
