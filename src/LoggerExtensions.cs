using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    internal static class LoggerExtensions
    {
        public static void Error(this ILogger logger, Exception exception, string request, int? statusCode)
            => logger.LogError(
                exception,
                $"[ERROR HANDLER] > Request: '{request}', StatusCode: '{statusCode}'"
            );

        public static void Error(this ILogger logger, Exception exception, string request, int? statusCode, string message)
            => logger.LogError(
                exception,
                $"[ERROR HANDLER] > Request: '{request}', StatusCode: '{statusCode}' > {message}"
            );



        public static void Debug(this ILogger logger, string message)
            => logger.LogDebug(
                $"[ERROR HANDLER] > {message}"
            );

        public static void Debug(this ILogger logger, HttpContext httpContext, string message)
            => logger.LogDebug(
                $"[ERROR HANDLER] > Request: '{httpContext.GetRequestEndpoint()}', StatusCode: '{httpContext.GetStatusCode()}' > {message}"
            );
    }
}
