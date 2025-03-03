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