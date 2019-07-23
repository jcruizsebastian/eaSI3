using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace eaSI3Web.GlobalErrorHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //return context.Response.WriteAsync(new ErrorDetails()
            //{
            //    StatusCode = context.Response.StatusCode,
            //    Message = exception.Message
            //}.ToString());

            return context.Response.WriteAsync("Error " + context.Response.StatusCode + " - " + exception.Message);
        }
    }
}
