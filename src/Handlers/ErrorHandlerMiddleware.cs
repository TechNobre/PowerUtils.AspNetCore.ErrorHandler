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
        private readonly ApiProblemDetailsFactory _problemDetailsFactory;


        public ErrorHandlerMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger,
            ApiProblemDetailsFactory problemDetailsFactory
        )
        {
            _next = next;
            _logger = logger;
            _problemDetailsFactory = problemDetailsFactory;
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
                return Task.CompletedTask;
            }

            var problemDetails = _problemDetailsFactory.Create(httpContext);

            _logger.Debug(problemDetails);

            return httpContext.WriteProblemDetailsResponseAsync(problemDetails);
        }
    }
}
