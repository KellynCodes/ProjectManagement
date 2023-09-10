namespace ProjectManagement.Services.Utility;
public class EmailTemplateModel
{
    public string Subject { get; set; } = string.Empty;
    public string EmailBodyHtml { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TemplateKey { get; set; } = string.Empty;
    public string DeliveryDate { get; set; } = string.Empty;
    public List<string> CCs { get; set; }
    public List<string> BCCs { get; set; }
    public Dictionary<string, string> Replacements { get; set; } = new Dictionary<string, string>();
}
