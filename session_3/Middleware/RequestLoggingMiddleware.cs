using System.Diagnostics;

namespace WebApplication1.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);  

            stopwatch.Stop();

            // log method, path, status code, elapsed ms here
            Console.WriteLine($"{context.Request.Method} {context.Request.Path} responded {context.Response.StatusCode} in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}