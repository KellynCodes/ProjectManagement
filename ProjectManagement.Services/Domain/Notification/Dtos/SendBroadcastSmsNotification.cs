namespace ProjectManagement.Services.Domain.Notification.Dtos
{
    public record SendBroadcastSmsNotification
    {
        public Guid SmsListId { get; set; }
        public string CountryCode { get; set; } = null!;
        public string Text { get; set; } = null!;
    }
}
