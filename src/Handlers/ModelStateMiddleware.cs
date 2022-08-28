using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PowerUtils.AspNetCore.ErrorHandler.Handlers
{
    internal static class ModelStateMiddleware
    {
        internal static IServiceCollection AddModelStateMiddleware(this IServiceCollection services) =>
            services.Configure<ApiBehaviorOptions>(options =>
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var problemDetailsFactory = actionContext.HttpContext.RequestServices.GetRequiredService<ApiProblemDetailsFactory>();
                    var loggerFactory = actionContext.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger("ModelStateHandler");

                    var problemDetails = problemDetailsFactory.Create(actionContext);

                    logger.Debug(problemDetails);


                    actionContext.HttpContext.ResetResponse();
                    actionContext.HttpContext.Response.StatusCode = problemDetails.Status ?? ProblemDetailsDefaults.FALLBACK_STATUS_CODE;


                    return new ObjectResult(problemDetails)
                    {
                        StatusCode = problemDetails.Status,
                        ContentTypes = { ProblemDetailsDefaults.PROBLEM_MEDIA_TYPE_JSON }
                    };
                });
    }
}
