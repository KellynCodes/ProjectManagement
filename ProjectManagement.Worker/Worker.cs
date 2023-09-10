using ProjectManagement.Worker.Services;

namespace ProjectManagement.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly INotificationReceivedEventHandler _notificationReceivedEventHandler;
        public Worker(ILogger<Worker> logger, INotificationReceivedEventHandler notificationReceivedEventHandler)
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