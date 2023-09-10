namespace ProjectManagement.Services.Domain.Notification.Dtos;

public record SendSmsNotification
{
    public string PhoneNumber { get; set; } = null!;
    public string CountryCode { get; set; } = null!;
    public string Text { get; set; } = null!;
}
