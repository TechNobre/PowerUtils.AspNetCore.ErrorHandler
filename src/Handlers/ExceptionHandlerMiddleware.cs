﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PowerUtils.AspNetCore.ErrorHandler.Handlers
{
    // ERROR with ProblemDetails: https://andrewlock.net/creating-a-custom-error-handler-middleware-function/
    // Official documentation: https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-5.0#exception-handler
    // Costumize error code response: https://weblog.west-wind.com/posts/2016/oct/16/error-handling-and-exceptionfilter-dependency-injection-for-aspnet-core-apis
    internal static class ExceptionHandlerMiddleware
    {
        internal static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app)
            => app.UseExceptionHandler(appError =>
                    appError.Run(async httpContext =>
                    {
                        var problemDetailsFactory = httpContext.RequestServices.GetRequiredService<ApiProblemDetailsFactory>();
                        var loggerFactory = httpContext.RequestServices.GetRequiredService<ILoggerFactory>();
                        var logger = loggerFactory.CreateLogger("ExceptionHandler");

                        // Create response
                        var exception = httpContext
                            .Features
                            .Get<IExceptionHandlerFeature>()?.Error;

                        ErrorProblemDetails problemDetails;
                        if(exception == null)
                        {
                            httpContext.ResetResponse(StatusCodes.Status500InternalServerError);
                            problemDetails = problemDetailsFactory.Create(httpContext);

                            logger.Error(exception, problemDetails.Instance, problemDetails.Status, "Unknown error");
                        }
                        else
                        {
                            // Inprovement exceptions when only will be one
                            if(exception is AggregateException aggregateException && aggregateException.InnerExceptions.Count == 1)
                            {
                                exception = aggregateException.InnerExceptions[0];
                            }

                            var options = httpContext.RequestServices.GetRequiredService<IOptions<ErrorHandlerOptions>>();

                            try
                            {
                                IEnumerable<KeyValuePair<string, ErrorDetails>> errors;
                                (httpContext.Response.StatusCode, errors) = exception.MappingToStatusCode(options.Value);
                                httpContext.ResetResponse(httpContext.Response.StatusCode);
                                problemDetails = problemDetailsFactory.Create(httpContext, errors);

                                logger.Error(exception, problemDetails.Instance, problemDetails.Status);
                            }
                            catch(Exception mappingException)
                            {
                                problemDetails = problemDetailsFactory.Create(httpContext, ImmutableDictionary<string, ErrorDetails>.Empty);
                                logger.Error(mappingException, "Error mapping exception");
                            }
                        }

                        // Write error details in body response
                        await httpContext.Response.WriteAsync(problemDetails);
                    })
        );
    }
}
