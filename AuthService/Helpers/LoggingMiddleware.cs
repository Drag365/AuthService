using AuthService.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NLog;
using System.Diagnostics;
using System.Security.Claims;

namespace AuthService.Helpers
{
    public class LoggingMiddleware
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!ToggleLoggingController.IsLoggingEnabled())
            {
                await _next(context);
                return;
            }

            var watch = new Stopwatch();
            watch.Start();

            await _next(context);

            watch.Stop();

            var userName = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var actionName = context.Request.RouteValues["action"];
            var responseTime = watch.ElapsedMilliseconds;
            var eventInfo = new LogEventInfo(NLog.LogLevel.Info, _logger.Name, "Message from logging middleware");
            eventInfo.Properties["userName"] = userName;
            eventInfo.Properties["responseTime"] = responseTime.ToString();
            eventInfo.Properties["statusCode"] = context.Response.StatusCode.ToString();
            eventInfo.Properties["action"] = actionName;

            // Skip logging for chunk uploads
            if (context.Request.Path == "/api/fileupload/upload")
            {
                return;
            }

            _logger.Log(eventInfo);
        }
    }
}
