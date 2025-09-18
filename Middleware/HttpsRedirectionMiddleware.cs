namespace websitebenhvien.Middleware
{
    public class HttpsRedirectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpsRedirectionMiddleware> _logger;

        public HttpsRedirectionMiddleware(RequestDelegate next, ILogger<HttpsRedirectionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Force HTTPS for production
            if (!context.Request.IsHttps && !IsLocalhost(context.Request))
            {
                var httpsUrl = $"https://{context.Request.Host}{context.Request.PathBase}{context.Request.Path}{context.Request.QueryString}";
                
                _logger.LogInformation($"Redirecting HTTP request to HTTPS: {httpsUrl}");
                
                context.Response.StatusCode = 301; // Permanent redirect
                context.Response.Headers.Location = httpsUrl;
                return;
            }

            // Add security headers for HTTPS requests
            if (context.Request.IsHttps)
            {
                context.Response.Headers.Add("Upgrade-Insecure-Requests", "1");
            }

            await _next(context);
        }

        private static bool IsLocalhost(HttpRequest request)
        {
            return request.Host.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ||
                   request.Host.Host.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase) ||
                   request.Host.Host.StartsWith("192.168.", StringComparison.OrdinalIgnoreCase) ||
                   request.Host.Host.StartsWith("10.", StringComparison.OrdinalIgnoreCase);
        }
    }

    public static class HttpsRedirectionExtensions
    {
        public static IApplicationBuilder UseCustomHttpsRedirection(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpsRedirectionMiddleware>();
        }
    }
}
