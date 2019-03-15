using Microsoft.AspNetCore.Builder;

namespace eaSI3Web.GlobalErrorHandling
{
    public static class AppBuilderExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
