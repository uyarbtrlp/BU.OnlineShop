using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace BU.OnlineShop.Shared.Exceptions
{
    public static class ExceptionHandler
    {
        public static void UseErrorHandler(this IApplicationBuilder app)
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
                        if (contextFeature.Error is ExceptionBase exception)
                        {
                            if (exception.StatusCode.HasValue)
                            {
                                context.Response.StatusCode = (int)exception.StatusCode.Value;
                            }
                            await context.Response.WriteAsync(exception.ToJson());
                        }
                        else
                        {
                            var exceptionBase = new ExceptionBase()
                            {
                                Code = null,
                                Message = "An unhandled exception occured",
                                StatusCode = (HttpStatusCode?)context.Response.StatusCode,

                            };
                            await context.Response.WriteAsync(exceptionBase.ToJson());
                        }
                    }
                });
            });
        }
    }
}
