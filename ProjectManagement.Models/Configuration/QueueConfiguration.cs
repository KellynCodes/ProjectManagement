namespace ProjectManagement.Models.Configuration;
public class QueueConfiguration
{
    public string EmailQueueUrl { get; set; } = null!;
    public int TimeOutInSeconds { get; set; }
    public int VisibilityInSeconds { get; set; }
}
