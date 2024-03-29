﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    internal static class ExceptionsExtensions
    {
        public static (int Status, IEnumerable<KeyValuePair<string, ErrorDetails>> Errors) MappingToStatusCode(this Exception exception, ErrorHandlerOptions options)
        {
            var exceptionType = exception.GetType();

            var mapper = options.ExceptionMappers
                .SingleOrDefault(s => exceptionType == s.Key || exceptionType.IsSubclassOf(s.Key));

            if(mapper.Key == null)
            {
                return (StatusCodes.Status500InternalServerError, new Dictionary<string, ErrorDetails>());
            }

            return mapper.Value.Handle(exception);
        }
    }
}
