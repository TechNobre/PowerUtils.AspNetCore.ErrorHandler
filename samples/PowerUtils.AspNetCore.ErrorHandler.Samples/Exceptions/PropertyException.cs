using System;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Exceptions
{
    public class PropertyException : Exception
    {
        public PropertyException(string message)
            : base(message) { }

        public string Property { get; set; }
        public string Code { get; set; }
    }
}
