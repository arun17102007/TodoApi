using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;
using TodoApi.Utils;

namespace TodoApi.Middleware
{
    /// <summary>
    /// Exception Middleware to handle exceptions globally
    /// </summary>
    public static class GlobalExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            if (app == null) return;
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context => 
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        logger.Error(contextFeature.Error, contextFeature.Error.Message);
                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode, 
                            Message = "Internal Server Error"
                        }.ToString());

                    }
                });
            });
        }
    }
}
