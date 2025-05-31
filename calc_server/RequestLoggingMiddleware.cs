using System.Diagnostics;
using log4net;

namespace calc_server;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILog _logger = LogManager.GetLogger("request-logger");

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var requestNumber = RequestTracker.GetAndIncrementCounter();
        ThreadContext.Properties["RequestNumber"] = requestNumber;

        var path = context.Request.Path;
        var method = context.Request.Method;

        _logger.Info($"Incoming request | #{requestNumber} | resource: {path} | HTTP Verb {method}");

        var stopwatch = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            _logger.Debug($"request #{requestNumber} duration: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}