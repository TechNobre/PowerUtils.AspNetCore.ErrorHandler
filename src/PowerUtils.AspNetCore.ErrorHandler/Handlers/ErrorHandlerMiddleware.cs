using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PowerUtils.AspNetCore.ErrorHandler.Handlers
{
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-5.0#create-a-middleware-pipeline-with-iapplicationbuilder
    internal class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;


        public ErrorHandlerMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger
        )
        {
            _next = next;
            _logger = logger;
        }


        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);

            if(httpContext.IsNotSuccess())
            { // Only can override BAD RESPNSES 4XX and 5XX
                await _handleProblem(httpContext);
            }
        }


        private Task _handleProblem(HttpContext httpContext)
        {
            // Only override the responses were not started.
            // E.g: `Controller.BadRequest`, `Controller.NotFound` or after `ModelStateHandler`
            if(httpContext.Response.HasStarted)
            {
                _logger.Debug(httpContext, "Response already started");

                return Task.CompletedTask;
            }

            var problemDetails = ProblemDetailsFactory.Create(httpContext);

            _logger.LogDebug(problemDetails);

            return httpContext.WriteProblemDetailsResponseAsync(problemDetails);
        }
    }
}
