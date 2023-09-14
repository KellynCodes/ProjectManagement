namespace ProjectManagement.Models.Configuration
{
    public class MailKit
    {
        public string EmailFrom { get; set; } = null!;
        public string SmtpHost { get; set; } = null!;
        public string SmtpPort { get; set; } = null!;
        public string SmtpPass { get; set; } = null!;
    }
}
