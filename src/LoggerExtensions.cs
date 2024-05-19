using System;
using Microsoft.Extensions.Logging;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    internal static class LoggerExtensions
    {
        public static void Debug(this ILogger logger, string message)
            => logger.LogDebug(
                "[ERROR HANDLER] > {Message}", message);


        public static void Error(this ILogger logger, Exception exception, string message)
            => logger.LogError(
                exception,
                "[ERROR HANDLER] > {Message}", message);

        public static void Error(this ILogger logger, Exception exception, ErrorProblemDetails problemDetails)
            => logger.LogError(
                exception,
                "[ERROR HANDLER] > Request: '{Request}', StatusCode: '{StatusCode}'", _sanitize(problemDetails.Instance), problemDetails.Status);

        public static void Error(this ILogger logger, Exception exception, ErrorProblemDetails problemDetails, string message)
            => logger.LogError(
                exception,
                "[ERROR HANDLER] > Request: '{Request}', StatusCode: '{StatusCode}' > {Message}", _sanitize(problemDetails.Instance), problemDetails.Status, message);



        // Function to sanitize user input before logging
        private static string _sanitize(string input)
            => input?.Replace(Environment.NewLine, " ") ?? "";
    }
}
