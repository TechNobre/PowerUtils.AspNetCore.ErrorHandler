using System;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException()
            : base("custom exception")
        { }
    }
}
