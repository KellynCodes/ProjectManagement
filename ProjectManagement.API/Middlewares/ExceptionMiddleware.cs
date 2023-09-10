using Microsoft.AspNetCore.Diagnostics;
using ProjectManagement.Models.Utility;
using System.Net;
using System.Text.Json;

namespace ProjectManagement.Middlewares;
public static class ExceptionMiddleware
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                IExceptionHandlerFeature? contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    app.Logger.LogError($"{contextFeature.Error}");
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    string text = JsonSerializer.Serialize(new ErrorResult
                    {
                        IsSuccessful = false,
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "Unexpected Request Failure",
                    });
                    await context.Response.WriteAsync(text);

                }
            });
        });
    }
}