using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Cache.Implementation;
using ProjectManagement.Cache.Interfaces;
using ProjectManagement.Data.Implementations;
using ProjectManagement.Data.Interfaces;
using ProjectManagement.Models.Configuration;
using ProjectManagement.Models.DatabaseContexts;
using ProjectManagement.Models.Identity;
using ProjectManagement.Services.Domain.ProjecT;
using ProjectManagement.Services.Domain.Task;
using ProjectManagement.Services.Domain.User;
using ProjectManagement.Services.Domains.Auth;
using ProjectManagement.Services.Domains.Notification;
using ProjectManagement.Services.Domains.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ProjectManagement.Extensions;
public static class ServiceCollectionExtensions
{

    public static void SetupAppServices(this IServiceCollection services)
    {
        services.AddTransient<ICacheService, CacheService>();
        services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();
        services.AddScoped<IAccountLockoutService, AccountLockoutService>();
        services.AddScoped<INotificationManagerService, NotificationManagerService>();
        services.AddScoped<IOtpCodeService, OtpCodeService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<JwtConfig>();
        services.AddSingleton<QueueConfiguration>();
    }


    public static void RegisterDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseSqlServer(connectionString, s =>
            {
                s.MigrationsAssembly("ProjectManagement.Models");
                s.EnableRetryOnFailure(3);
            });
        });
    }

    public static void RegisterAuthentication(this IServiceCollection services, JwtConfig jwtConfig)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
        }).AddEntityFrameworkStores<ApplicationDbContext>()
         .AddDefaultTokenProviders();

        services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromMinutes(5));

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.JwtKey));
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = serverSecret,
                ValidIssuer = jwtConfig.JwtIssuer,
                ValidAudience = jwtConfig.JwtAudience,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });

        services.AddAuthorization();
    }
}
