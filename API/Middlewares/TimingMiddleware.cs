using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Middlewares
{
    public class TimingMiddleware : IMiddleware
    {
        private readonly ILogger<TimingMiddleware> _logger;

        public TimingMiddleware(ILogger<TimingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var startTime = DateTime.UtcNow;
            await next.Invoke(context);
            _logger.LogInformation($">>>>>>>>>>>>>>>>>>>>>>>>>> Request path {context.Request.Path}: took {(DateTime.UtcNow - startTime).TotalMilliseconds} ms");
        }
    }

    public static class TimingExtensions {
        public static IApplicationBuilder UseTiming(this IApplicationBuilder app) {
            return app.UseMiddleware<TimingMiddleware>();
        }
    }
}