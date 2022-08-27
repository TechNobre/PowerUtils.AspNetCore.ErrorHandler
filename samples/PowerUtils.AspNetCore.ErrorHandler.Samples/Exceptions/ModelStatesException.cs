using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Exceptions
{
    public abstract class ModelStatesException : Exception
    {
        public int Status { get; set; }
        public IDictionary<string, string> Errors { get; set; }
    }

    public class NotFoundException : ModelStatesException
    {
        public NotFoundException()
        {
            Status = StatusCodes.Status404NotFound;
            Errors = new Dictionary<string, string>()
            {
                { "Prop1", "NOT_FOUND" }
            };
        }
    }

    public class DuplicatedException : ModelStatesException
    {
        public DuplicatedException()
        {
            Status = StatusCodes.Status409Conflict;
            Errors = new Dictionary<string, string>()
            {
                { "Prop2", "DUPLICATED" }
            };
        }
    }
}
