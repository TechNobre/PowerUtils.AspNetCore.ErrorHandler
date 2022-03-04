using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using PowerUtils.Net.Constants;
using PowerUtils.Text;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    internal class ProblemDetailsFactory
    {
        private readonly IOptions<ErrorHandlerOptions> _options;

        public ProblemDetailsFactory(IOptions<ErrorHandlerOptions> options)
            => _options = options;

        internal static ProblemDetailsResponse Create(HttpContext httpContext)
        {
            var result = new ProblemDetailsResponse();

            result.Status = httpContext.GetStatusCode() ?? 0;
            result.Type = result.Status.GetStatusCodeLinkOrDefault();
            result.Title = result.Status == 0 ? null : ReasonPhrases.GetReasonPhrase(result.Status);

            result.Instance = httpContext.GetRequestEndpoint();

            result.TraceID = httpContext.GetCorrelationId();

            result.Errors = new Dictionary<string, string>();

            return result;
        }


        internal ProblemDetailsResponse Create(HttpContext httpContext, IDictionary<string, string> errors)
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
            var error = modelStateError
                .Value
                .Errors
                .Select(s => s.ErrorMessage)
                .First();

            if(modelStateError.Key.StartsWith("$."))
            {
                return (
                    _formatPropertyName(modelStateError.Key[2..]),
                    "INVALID"
                );
            }

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
                PropertyNamingPolicy.SnakeCase => _formatPropertyToSnakeCase(propertyName),
                _ => _formatPropertyToCamelCase(propertyName),
            };

        private static string _formatPropertyToCamelCase(string propertyName)
        {
            var propertyParts = propertyName.Split('.');
            if(propertyParts.Length == 1)
            {
                return char.ToLowerInvariant(propertyName[0]) + propertyName[1..];
            }


            for(var count = 0; count < propertyParts.Length; count++)
            {
                propertyParts[count] = char.ToLowerInvariant(propertyParts[count][0]) + propertyParts[count][1..];
            }


            return string.Join(".", propertyParts);
        }

        private static string _formatPropertyToSnakeCase(string propertyName)
        {
            var propertyParts = propertyName.Split('.');
            
            for(var count = 0; count < propertyParts.Length; count++)
            {
                propertyParts[count] = propertyParts[count].ToSnakeCase();
            }


            return string.Join(".", propertyParts);
        }
    }
}
