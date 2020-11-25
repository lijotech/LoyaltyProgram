using System;
using Microsoft.AspNetCore.Http;
using MemberAPI.Data.Security.v1;
using System.Threading.Tasks;

namespace MemberAPI.Security.v1
{
    public class LoggingMiddleware
    {
        private readonly ILoggerManager _logger;
        private readonly RequestDelegate _next;

        public LoggingMiddleware(ILoggerManager logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Items["CorrelationId"] = Guid.NewGuid().ToString();
            _logger.LogInfo($"About to start {context.Request.Method} " +
                $"{context.Request.Path} request");

            await _next(context);

            _logger.LogInfo($"Request completed with status code: {context.Response.StatusCode} ");
        }
    }
}
