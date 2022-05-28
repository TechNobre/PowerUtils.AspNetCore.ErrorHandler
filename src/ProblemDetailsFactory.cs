using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
                result.Errors.Add(_formatPropertyName(error.Key), error.Value);
            }

            return result;
        }

        internal ProblemDetailsResponse Create(ActionContext actionContext)
        {
            var result = Create(actionContext.HttpContext);

            result.Errors = _mappingModelState(actionContext.ModelState);

            return result;
        }


        private IDictionary<string, string> _mappingModelState(ModelStateDictionary modelState)
        {
            var modelStateErrors = modelState.Where(s => s.Value.Errors.Count > 0);

            var errors = new Dictionary<string, string>();
            foreach(var modelStateError in modelStateErrors)
            {
                (var property, var error) = _mappingModelStateError(modelStateError);
                errors.Add(property, error);
            }

            return errors;
        }

        private (string Property, string Error) _mappingModelStateError(KeyValuePair<string, ModelStateEntry> modelStateError)
        {
            if(modelStateError.Key.StartsWith("$."))
            {
                return (
                    _formatPropertyName(modelStateError.Key[2..]),
                    "INVALID"
                );
            }

            var error = modelStateError
                .Value
                .Errors
                .Select(s => s.ErrorMessage)
                .First();

            return _checkRequestBody(modelStateError.Key, error);
        }

        private (string Property, string Error) _checkRequestBody(string property, string error)
        {
            if(property == "$")
            {
                return (_formatPropertyName("RequestBody"), "INVALID");
            }

            if("A non-empty request body is required.".Equals(error, StringComparison.InvariantCultureIgnoreCase))
            {
                return (_formatPropertyName("RequestBody"), "REQUIRED");
            }

            return (_formatPropertyName(property), error);
        }

        private string _formatPropertyName(string propertyName)
            // PropertyNamingPolicy.CamelCase is default value
            => _options.Value.PropertyNamingPolicy switch
            {
                PropertyNamingPolicy.Original => propertyName,
                PropertyNamingPolicy.SnakeCase => propertyName.FormatToSnakeCase(),
                _ => propertyName.FormatToCamelCase(),
            };

    }
}
