using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace websitebenhvien.Middleware
{
    public static class CompressionExtensions
    {
        public static IServiceCollection AddCustomCompression(this IServiceCollection services)
        {
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.SmallestSize;
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                
                // Add MIME types to compress
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    "text/html",
                    "text/css",
                    "text/javascript",
                    "application/javascript",
                    "application/json",
                    "text/json",
                    "image/svg+xml",
                    "application/xml",
                    "text/xml"
                });
            });

            return services;
        }
    }
}
