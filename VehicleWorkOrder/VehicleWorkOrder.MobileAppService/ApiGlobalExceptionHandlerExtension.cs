namespace VehicleWorkOrder.MobileAppService
{
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Data.SqlClient;
    using Serilog;

    public static class ApiGlobalExceptionHandlerExtension
    {

        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature == null)
                    {
                        var text = "An unexpected fault happened. Try again later.";
                        //logger.LogError(500, text);
                        logger.Error(text);
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync(text);
                        return;
                    }

                    var statusCode = 0;
                    var response = string.Empty;
                    var message = string.Empty;
                    switch (contextFeature.Error)
                    {
                        case SqlException sqlException:
                            statusCode = (int) HttpStatusCode.InternalServerError;
                            response = "SQL exception happened.";
                            message = response;
                            break;
                        case ArgumentException argumentException:
                            statusCode = (int) HttpStatusCode.BadRequest;
                            response = argumentException.Message;
                            message = "Invalid Argument";
                            break;
                        case InvalidOperationException operationException:
                            statusCode = (int) HttpStatusCode.InternalServerError;
                            response = operationException.Message;
                            message = "Invalid Operation";
                            break;

                        default:
                            response = "An unexpected fault happened. Try again later.";
                            statusCode = (int)HttpStatusCode.InternalServerError;
                            message = response;
                            break;
                    }

                    context.Response.StatusCode = statusCode;
                    //logger.LogError(statusCode, contextFeature.Error, contextFeature.Error.InnerException == null ? "" : contextFeature.Error.InnerException.Message);
                    logger.Error(contextFeature.Error, message);
                    await context.Response.WriteAsync(response);
                });
            });
        }
    }
}
