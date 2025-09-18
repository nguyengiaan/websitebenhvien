namespace websitebenhvien.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Content Security Policy - Progressive approach
            var isDevelopment = context.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() ?? false;
            
            if (isDevelopment)
            {
                // More permissive CSP for development
                context.Response.Headers["Content-Security-Policy-Report-Only"] = 
                    "default-src 'self' 'unsafe-inline' 'unsafe-eval' data: blob: *; " +
                    "report-uri /api/csp-violation;";
            }
            else
            {
                // Stricter CSP for production
                context.Response.Headers["Content-Security-Policy"] = 
                    "default-src 'self'; " +
                    "script-src 'self' 'unsafe-inline' 'unsafe-eval' " +
                    "https://cdn.jsdelivr.net https://cdnjs.cloudflare.com https://www.gstatic.com " +
                    "https://translate.google.com https://translate.googleapis.com " +
                    "https://sp.zalo.me https://page.widget.zalo.me https://www.youtube.com " +
                    "https://za.zdn.vn https://px.dmp.zaloapp.com https://googleads.g.doubleclick.net " +
                    "https://static.doubleclick.net https://www.google.com https://play.google.com; " +
                    "style-src 'self' 'unsafe-inline' " +
                    "https://fonts.googleapis.com https://cdn.jsdelivr.net https://cdnjs.cloudflare.com " +
                    "https://page.widget.zalo.me; " +
                    "font-src 'self' data: " +
                    "https://fonts.gstatic.com https://cdn.jsdelivr.net https://cdnjs.cloudflare.com " +
                    "https://page.widget.zalo.me; " +
                    "img-src 'self' data: blob: https: " +
                    "https://i.ytimg.com https://yt3.ggpht.com https://cdn-icons-png.flaticon.com " +
                    "https://page.widget.zalo.me; " +
                    "connect-src 'self' " +
                    "https://api.widget.zalo.me https://widget.chat.zalo.me https://za.zalo.me " +
                    "https://translate.googleapis.com https://www.youtube.com https://play.google.com " +
                    "https://googleads.g.doubleclick.net https://px.dmp.zaloapp.com; " +
                    "frame-src 'self' https://www.youtube.com https://page.widget.zalo.me; " +
                    "media-src 'self' blob: data:; " +
                    "object-src 'none'; " +
                    "base-uri 'self'; " +
                    "form-action 'self'; " +
                    "frame-ancestors 'self'; " +
                    "report-uri /api/csp-violation;";
            }

            // HTTP Strict Transport Security (HSTS)
            if (context.Request.IsHttps)
            {
                context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
            }

            // X-Frame-Options (Anti-clickjacking)
            context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";

            // X-Content-Type-Options (Prevent MIME sniffing)
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";

            // Cross-Origin-Opener-Policy
            context.Response.Headers["Cross-Origin-Opener-Policy"] = "same-origin";

            // Referrer Policy
            context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

            // X-XSS-Protection (Legacy browsers)
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";

            // Permissions Policy
            context.Response.Headers["Permissions-Policy"] = 
                "geolocation=(), microphone=(), camera=(), payment=(), usb=(), " +
                "magnetometer=(), gyroscope=(), accelerometer=(), autoplay=(self)";

            // Remove server information
            context.Response.Headers.Remove("Server");
            context.Response.Headers.Remove("X-Powered-By");
            context.Response.Headers.Remove("X-AspNet-Version");

            await _next(context);
        }
    }

    public static class SecurityHeadersExtensions
    {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecurityHeadersMiddleware>();
        }
    }
}
