using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    public sealed class ApiProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _apiBehaviorOptions;
        private readonly ErrorHandlerOptions _errorHandlerOptions;

        public ApiProblemDetailsFactory(
            IOptions<ApiBehaviorOptions> apiBehaviorOptions,
            IOptions<ErrorHandlerOptions> errorHandlerOptions
        )
        {
            _apiBehaviorOptions = apiBehaviorOptions?.Value ?? throw new ArgumentNullException(nameof(apiBehaviorOptions));
            _errorHandlerOptions = errorHandlerOptions?.Value ?? throw new ArgumentNullException(nameof(errorHandlerOptions));
        }

        internal ErrorProblemDetails Create(HttpContext httpContext)
        {
            var problemDetails = new ErrorProblemDetails();

            _applyDefaults(httpContext, problemDetails);

            return problemDetails;
        }


        public ErrorProblemDetails Create(HttpContext httpContext, IEnumerable<KeyValuePair<string, string>> errors)
        {
            var result = Create(httpContext);

            foreach(var error in errors)
            {
                result.Errors
                    .Add(
                        _formatPropertyName(error.Key),
                        error.Value
                    );
            }

            return result;
        }

        internal ErrorProblemDetails Create(ActionContext actionContext)
        {
            var payloadTooLargeError = actionContext.ModelState.CheckPayloadTooLargeAndReturnError();
            if(payloadTooLargeError.Count == 1)
            { // When the payload is too large, we return a 413 status code
                actionContext.HttpContext.Response.StatusCode = StatusCodes.Status413RequestEntityTooLarge;

                return Create(
                   actionContext.HttpContext,
                   payloadTooLargeError
                );
            }


            actionContext.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            return Create(
                actionContext.HttpContext,
                actionContext.ModelState.MappingModelState()
            );
        }

        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null
        )
        {
            statusCode ??= httpContext.GetStatusCode();

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            _applyDefaults(httpContext, problemDetails);

            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null
        )
        {
            if(modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            statusCode ??= httpContext.GetStatusCode();

            var problemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            if(title != null)
            {
                // For validation problem details, don't overwrite the default title with null.
                problemDetails.Title = title;
            }

            _applyDefaults(httpContext, problemDetails);

            return problemDetails;
        }

        private void _applyDefaults(HttpContext httpContext, ProblemDetails problemDetails)
        {
            problemDetails.Status ??= httpContext.GetStatusCode();

            if(_apiBehaviorOptions.ClientErrorMapping.TryGetValue(problemDetails.Status.Value, out var clientErrorData))
            {
                problemDetails.Type ??= clientErrorData.Link;
                problemDetails.Title ??= clientErrorData.Title;
            }
            else
            {
                problemDetails.Type ??= ProblemDetailsDefaults.Defaults[0].Link;
                problemDetails.Title ??= ProblemDetailsDefaults.Defaults[0].Title;
            }

            problemDetails.Instance = httpContext.GetRequestEndpoint();

            problemDetails.Extensions["traceId"] = httpContext.GetCorrelationId();

            _applyDetails(problemDetails);
        }

        private static void _applyDetails(ProblemDetails problemDetails)
        {
            if(!string.IsNullOrWhiteSpace(problemDetails.Detail))
            {
                return;
            }

            problemDetails.Detail = problemDetails.Status switch
            {
                401 => "A authentication error has occurred.",
                403 => "A permissions error has occurred.",
                404 => "The entity was not found.",
                409 => "The entity already exists.",

                _ when problemDetails.Status >= 400 && problemDetails.Status < 500 => "One or more validation errors occurred.",

                _ => "An unexpected error has occurred."
            };
        }

        // PropertyNamingPolicy.CamelCase is default value
        private string _formatPropertyName(string propertyName)
            => _errorHandlerOptions.PropertyNamingPolicy switch
            {
                PropertyNamingPolicy.Original => propertyName,
                PropertyNamingPolicy.SnakeCase => propertyName.FormatToSnakeCase(),
                _ => propertyName.FormatToCamelCase(),
            };
    }
}
