namespace ProjectManagement.Models.Configuration;
public static class ConfigurationBinder
{
    public static AppSetting BindConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        AppSetting appSetting = new AppSetting();
        configuration.GetSection("AppSetting").Bind(appSetting);
        services.AddSingleton(appSetting);
        return appSetting;
    }
}
