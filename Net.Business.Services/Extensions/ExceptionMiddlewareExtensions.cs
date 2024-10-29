using System.Net;
using Net.Business.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;

namespace Net.Business.Services
{
    public static class ExceptionMiddlewareExtensions
    {
        //, ILoggerManager logger
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {

                        await context.Response.WriteAsync(new DtoErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            ErrorMessage = contextFeature.Error.Message.ToString()
                        }.ToString());
                    }
                });
            });
        }
    }
}
