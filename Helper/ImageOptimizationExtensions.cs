namespace websitebenhvien.Helper
{
    public static class ImageOptimizationExtensions
    {
        public static IServiceCollection AddImageOptimization(this IServiceCollection services)
        {
            // Simple image optimization without external dependencies
            // Will be implemented with basic System.Drawing or future ImageSharp integration
            return services;
        }

        public static WebApplication UseImageOptimization(this WebApplication app)
        {
            // Placeholder for future image optimization middleware
            return app;
        }
    }
}
