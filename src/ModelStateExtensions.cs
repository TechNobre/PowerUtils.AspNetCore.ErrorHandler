using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    internal static class ModelStateExtensions
    {
        private const string BODY_PROPERTY_NAME = "RequestBody";

        public static IDictionary<string, string> MappingModelState(this ModelStateDictionary modelState)
        {
            var modelStateErrors = modelState.Where(s => s.Value.Errors.Count > 0);

            var errors = new Dictionary<string, string>();
            foreach(var modelStateError in modelStateErrors)
            {
                (var property, var error) = modelStateError._mappingModelStateErrors();
                errors.Add(property, error);
            }

            return errors;
        }

        public static IDictionary<string, string> CheckPayloadTooLargeAndReturnError(this ModelStateDictionary modelState)
        {
            var modelStateErrors = modelState.Where(s => s.Value.Errors.Count > 0);
            if(modelStateErrors.Count() != 1)
            {
                return new Dictionary<string, string>();
            }


            var errorMessage = modelStateErrors
                .Single()
                .Value.Errors
                .Select(s => s.ErrorMessage)
                .First()
                .ToLower();

            if("failed to read the request form. multipart body length limit 1048576 exceeded.".Equals(errorMessage))
            {
                var maxSize = errorMessage
                    .Replace("failed to read the request form. multipart body length limit ", "")
                    .Replace(" exceeded.", "");

                return new Dictionary<string, string>()
                {
                    { BODY_PROPERTY_NAME, "MAX:" + maxSize }
                };
            }

            return new Dictionary<string, string>();
        }

        private static (string Property, string Error) _mappingModelStateErrors(this KeyValuePair<string, ModelStateEntry> modelStateError)
        {
            if(modelStateError.Key.StartsWith("$."))
            {
                return (
                    modelStateError.Key[2..],
                    "INVALID"
                );
            }

            var error = modelStateError
                .Value
                .Errors
                .Select(s => s.ErrorMessage)
                .First();

            return _mappingError(modelStateError.Key, error);
        }

        private static (string Property, string Error) _mappingError(string property, string error)
        {
            if(property == "$")
            {
                return (BODY_PROPERTY_NAME, "INVALID");
            }

            if("A non-empty request body is required.".Equals(error, StringComparison.InvariantCultureIgnoreCase))
            {
                return (BODY_PROPERTY_NAME, "REQUIRED");
            }

            return (property, error);
        }
    }
}
