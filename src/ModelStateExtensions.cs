using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    internal static class ModelStateExtensions
    {
        private const string PAYLOAD_PROPERTY_NAME = "Payload";

        public static IDictionary<string, ErrorDetails> MappingModelState(this ModelStateDictionary modelState)
        {
            var modelStateErrors = modelState.Where(s => s.Value.Errors.Count > 0);

            var errors = new Dictionary<string, ErrorDetails>();
            foreach(var modelStateError in modelStateErrors)
            {
                (var property, var error) = modelStateError._mappingModelStateErrors();
                errors.Add(property, error);
            }

            return errors;
        }

        public static IDictionary<string, ErrorDetails> CheckPayloadTooLargeAndReturnError(this ModelStateDictionary modelState)
        {
            var modelStateErrors = modelState.Where(s => s.Value.Errors.Count > 0);
            if(modelStateErrors.Count() != 1)
            {
                return new Dictionary<string, ErrorDetails>();
            }


            var errorMessage = modelStateErrors
                .Single()
                .Value.Errors
                .Select(s => s.ErrorMessage)
                .First()
                .ToLower();

            if(errorMessage.StartsWith("failed to read the request form. multipart body length limit ") &&
               errorMessage.EndsWith(" exceeded."))
            {
                var maxSize = errorMessage
                    .Replace("failed to read the request form. multipart body length limit ", "")
                    .Replace(" exceeded.", "");

                return new Dictionary<string, ErrorDetails>()
                {
                    [PAYLOAD_PROPERTY_NAME] = new($"MAX:{maxSize}", "The payload is too big.")
                };
            }

            return new Dictionary<string, ErrorDetails>();
        }

        private static (string Property, ErrorDetails Error) _mappingModelStateErrors(this KeyValuePair<string, ModelStateEntry> modelStateError)
        {
            if(modelStateError.Key.StartsWith("$."))
            {
                return (
                    modelStateError.Key[2..],
                    new("INVALID", "The payload is invalid."));
            }

            var error = modelStateError
                .Value
                .Errors
                .Select(s => s.ErrorMessage)
                .First();

            return _mappingError(modelStateError.Key, error);
        }

        private static (string Property, ErrorDetails Error) _mappingError(string property, string description)
        {
            if(property == "$")
            {
                return (PAYLOAD_PROPERTY_NAME, new("INVALID", "The payload is invalid."));
            }

            if("A non-empty request body is required.".Equals(description, StringComparison.InvariantCultureIgnoreCase))
            {
                return (PAYLOAD_PROPERTY_NAME, new("REQUIRED", "The payload is required."));
            }

            return (property, new("INVALID", description));
        }
    }
}
