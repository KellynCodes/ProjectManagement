using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProjectManagement.Data.Seeder;
using ProjectManagement.Extensions;
using ProjectManagement.Middlewares;
using ProjectManagement.Models.Configuration;
using ProjectManagement.Models.DatabaseContexts;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
IConfiguration Configuration = builder.Configuration;
IServiceCollection services = builder.Services;

AppSetting normandyHostelManagerApiConfig = Configuration.Get<AppSetting>()!;

AppSetting appSetting = builder.Services.BindConfigurations(builder.Configuration);

services.AddRedisCache(appSetting.Redis);
services.AddAsyncComponents(Configuration);

services.RegisterDbContext(appSetting.ConnString);
services.RegisterAuthentication(appSetting.Jwt);
// Add services to the container.
services.SetupAppServices();
services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//Register Automapper
services.AddAutoMapper(Assembly.Load("ProjectManagement.Services"));

// Add services to the container.
IServiceProvider serviceProvider = services.BuildServiceProvider();
ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();

services.AddControllers(x =>
{
    x.EnableEndpointRouting = false;
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddApiVersioning(setup =>
                {
                    setup.DefaultApiVersion = new ApiVersion(1, 0);
                    setup.AssumeDefaultVersionWhenUnspecified = true;
                    setup.ReportApiVersions = true;
                })
                .AddVersionedApiExplorer(setup =>
                {
                    setup.GroupNameFormat = "'v'VVV";
                    setup.SubstituteApiVersionInUrl = true;
                });
//.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseHttpLogging();

    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.ConfigureExceptionHandler();
app.MapControllers();

await context.Database.MigrateAsync();
await Seed.EnsurePopulatedAsync(app);
await app.RunAsync();
