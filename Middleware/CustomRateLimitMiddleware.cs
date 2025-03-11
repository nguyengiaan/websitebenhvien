using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace websitebenhvien.Middleware
{
    public class CustomRateLimitMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomRateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) when (context.Response.StatusCode == 429)
            {
                context.Response.Clear();
                context.Response.StatusCode = 429;
                await context.Response.WriteAsJsonAsync(new
                {
                    Status = 429,
                    Message = "Quá nhiều yêu cầu. Vui lòng thử lại sau."
                });
            }
        }
    }

    // Extension method để dễ dàng thêm middleware vào pipeline
    public static class CustomRateLimitMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomRateLimit(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomRateLimitMiddleware>();
        }
    }
}