using ProjectManagement.Worker.Services.Notification.Interfaces;

namespace ProjectManagement.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISqsNotificationEventHandler _notificationReceivedEventHandler;
        public Worker(ILogger<Worker> logger, ISqsNotificationEventHandler notificationReceivedEventHandler)
        {
            _logger = logger;
            _notificationReceivedEventHandler = notificationReceivedEventHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _notificationReceivedEventHandler.PollMessagesForEmailSendingAsync(stoppingToken);
            }
        }
    }
}