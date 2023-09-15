using Newtonsoft.Json;
using ProjectManagement.Models.Configuration;
using ProjectManagement.Services.Domain.User.Dto;
using ProjectManagement.Worker.Services.Notification.Interfaces;
using System.Net;

namespace ProjectManagement.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISqsNotificationEventHandler _notificationReceivedEventHandler;
        private readonly HttpClient _client = new HttpClient();
        private readonly AppSetting _appSetting;
        public Worker(ILogger<Worker> logger,
            ISqsNotificationEventHandler notificationReceivedEventHandler,
            AppSetting appSetting
           )
        {
            _logger = logger;
            _appSetting = appSetting;
            _notificationReceivedEventHandler = notificationReceivedEventHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {


                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await _notificationReceivedEventHandler.PollMessagesForEmailSendingAsync(stoppingToken);
                    HttpResponseMessage responseMessage = await _client.GetAsync($"{_appSetting.Api.BaseUrl}/task/due-date-reminder", stoppingToken);
                    string taskJson = await responseMessage.Content.ReadAsStringAsync(stoppingToken);
                    UserResponseDto response = JsonConvert.DeserializeObject<UserResponseDto>(taskJson)!;
                    if (responseMessage.StatusCode != HttpStatusCode.OK)
                    {
                        _logger.LogInformation($"Error fetching task \n ErrorMessage ===>[ {responseMessage}");
                        continue;
                    }
                    _logger.LogInformation($"Due Task fetched and sent to the queue \n {response}");
                }
                catch (HttpRequestException e)
                {
                    _logger.LogError($"Request error: {e.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something happened while processing the background task. {ex.Message}");
                }
            }
        }
    }
}