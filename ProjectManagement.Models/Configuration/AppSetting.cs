using ProjectManagement.API.Configuration;
using ProjectManagement.Cache.Configuration;
namespace ProjectManagement.Models.Configuration;

public class AppSetting
{
    public string ConnectionString { get; set; } = null!;
    public string ConnString { get; set; } = null!;
    public ApiConfig Api { get; set; } = null!;
    public RedisConfig Redis { get; set; } = null!;
    public JwtConfig Jwt { get; set; } = null!;
    public AwsConfig Aws { get; set; } = null!;
    public MailKit MailKit { get; set; } = null!;
    public QueueConfiguration QueueConfiguration { get; set; } = null!;
}


public class ApiConfig
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ClientUrl { get; set; } = string.Empty;
}