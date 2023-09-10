using Amazon.S3;
using Amazon.SimpleEmail;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using ProjectManagement.AsyncClient.Implementation;
using ProjectManagement.AsyncClient.Interfaces;

namespace ProjectManagement.Extensions;

public static class AsyncClientRegisterExtension
{
    public static void AddAsyncComponents(this IServiceCollection services, IConfiguration configuration)
    {
        //Register AWS Default Configuration
        services.AddDefaultAWSOptions(configuration.GetAWSOptions("AppSetting:AWS"));

        //Register AWS SQS Client
        services.AddAWSService<IAmazonSQS>();

        //Register AWS SNS Client
        services.AddAWSService<IAmazonSimpleNotificationService>();

        //Register AWS SES Client
        services.AddAWSService<IAmazonSimpleEmailService>();

        //Register AWS S3 Client
        services.AddAWSService<IAmazonS3>();

        services.AddSingleton<Func<ICommandClient>>(
              sp => sp.GetRequiredService<CommandClient>
      );

        services.AddSingleton<ICommandClient, CommandClient>();
        services.AddTransient<IS3Client, S3Client>();

    }
}
