using Amazon.S3;
using Amazon.SimpleEmail;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using ProjectManagement.AsyncClient.Implementation;
using ProjectManagement.AsyncClient.Interfaces;
using ProjectManagement.Models.Configuration;
using ProjectManagement.Services.EmailProviders.Implementations;
using ProjectManagement.Services.EmailProviders.Interfaces;
using ProjectManagement.Worker;
using ProjectManagement.Worker.Services.Notification.Implementations;
using ProjectManagement.Worker.Services.Notification.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var appSettings = new AppSetting();
        configuration.GetSection("AppSetting").Bind(appSettings);
        services.AddSingleton(appSettings);
        services.AddTransient<ISqsNotificationEventHandler, SqsNotificationEventHandler>();
        services.AddTransient<IS3Client, S3Client>();
        services.AddTransient<IEmailProviderService, EmailProviderService>();

        services.AddTransient<ICommandClient, CommandClient>(); // Registering ICommandClient implementation
        services.AddTransient<Func<ICommandClient>>(sp => () => sp.GetService<ICommandClient>());

        // Register the INotificationReceivedService (assuming its implementation is NotificationReceivedService)
        services.AddTransient<ISqsNotificationService, SqsNotificationService>();

        services.AddHostedService<Worker>();
        services.AddDefaultAWSOptions(configuration.GetAWSOptions("AppSetting:AWS"));

        //Register AWS SQS Client
        services.AddAWSService<IAmazonSQS>();

        //Register AWS SNS Client
        services.AddAWSService<IAmazonSimpleNotificationService>();

        //Register AWS SES Client
        services.AddAWSService<IAmazonSimpleEmailService>();

        //Register AWS S3 Client
        services.AddAWSService<IAmazonS3>();
        /*
                services.AddSingleton<Func<ICommandClient>>(
                      sp => sp.GetRequiredService<CommandClient>
              );*/



    })
    .Build();

host.Run();


