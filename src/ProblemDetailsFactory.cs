using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using PowerUtils.Net.Constants;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    public class ProblemDetailsFactory
    {
        private readonly IOptions<ErrorHandlerOptions> _options;

        public ProblemDetailsFactory(IOptions<ErrorHandlerOptions> options)
            => _options = options;

        internal static ProblemDetailsResponse Create(HttpContext httpContext)
        {
            var result = new ProblemDetailsResponse();

            result.Status = httpContext.GetStatusCode() ?? 0; // Default value is 0
            result.Type = result.Status.GetStatusCodeLinkOrDefault();
            result.Title = result.Status == 0 ? null : ReasonPhrases.GetReasonPhrase(result.Status);

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
            if(payloadTooLargeError != null)
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
            => _options.Value.PropertyNamingPolicy switch
            {
                PropertyNamingPolicy.Original => propertyName,
                PropertyNamingPolicy.SnakeCase => propertyName.FormatToSnakeCase(),
                _ => propertyName.FormatToCamelCase(),
            };
    }
}
