using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    public class ProblemDetailsFactory
    {
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;
        private readonly IOptions<ErrorHandlerOptions> _errorHandlerOptions;

        public ProblemDetailsFactory(
            IOptions<ApiBehaviorOptions> apiBehaviorOptions,
            IOptions<ErrorHandlerOptions> errorHandlerOptions
        )
        {
            _apiBehaviorOptions = apiBehaviorOptions;
            _errorHandlerOptions = errorHandlerOptions;
        }

        internal ProblemDetailsResponse Create(HttpContext httpContext)
        {
            var result = new ProblemDetailsResponse();

            result.Status = httpContext.GetStatusCode() ?? 0; // Default value is 0
            if(_apiBehaviorOptions.Value.ClientErrorMapping.TryGetValue(result.Status, out var clientErrorData))
            {
                result.Type = clientErrorData.Link;
                result.Title = clientErrorData.Title;
            }
            else
            {
                result.Type = ProblemDetailsDefaults.Defaults[0].Link;
                result.Title = ProblemDetailsDefaults.Defaults[0].Title;
            }

            result.Instance = httpContext.GetRequestEndpoint();

            result.TraceID = httpContext.GetCorrelationId();

            return result;
        }


        public ProblemDetailsResponse Create(HttpContext httpContext, IEnumerable<KeyValuePair<string, string>> errors)
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


        internal ProblemDetailsResponse Create(ActionContext actionContext)
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

        // PropertyNamingPolicy.CamelCase is default value
        private string _formatPropertyName(string propertyName)
            => _errorHandlerOptions.Value.PropertyNamingPolicy switch
            {
                PropertyNamingPolicy.Original => propertyName,
                PropertyNamingPolicy.SnakeCase => propertyName.FormatToSnakeCase(),
                _ => propertyName.FormatToCamelCase(),
            };
    }
}
