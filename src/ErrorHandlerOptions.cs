using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Http;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    public interface IExceptionMapper
    {
        (int Status, IEnumerable<KeyValuePair<string, ErrorDetails>> Errors) Handle(Exception exception);
    }


    public class ExceptionMapper<TException> : IExceptionMapper
        where TException : Exception
    {
        public Func<TException, (int Status, IEnumerable<KeyValuePair<string, ErrorDetails>>)> Handler { get; set; }


        public (int Status, IEnumerable<KeyValuePair<string, ErrorDetails>> Errors) Handle(Exception exception)
            => Handler(exception as TException);
    }


    public class ErrorHandlerOptions
    {
        /// <summary>
        /// Default value: CamelCase
        /// </summary>
        public PropertyNamingPolicy PropertyNamingPolicy { get; set; } = PropertyNamingPolicy.CamelCase;

        public IDictionary<Type, IExceptionMapper> ExceptionMappers { get; set; } = new Dictionary<Type, IExceptionMapper>();

        public ErrorHandlerOptions()
        {
            ExceptionMappers.Add(
                typeof(NotImplementedException),
                new ExceptionMapper<NotImplementedException>()
                {
                    Handler = (_) => (StatusCodes.Status501NotImplemented, new Dictionary<string, ErrorDetails>())
                }
            );

            ExceptionMappers.Add(
                typeof(TimeoutException),
                new ExceptionMapper<TimeoutException>()
                {
                    Handler = (_) => (StatusCodes.Status504GatewayTimeout, new Dictionary<string, ErrorDetails>())
                }
            );
        }
    }

    public static class ErrorHandlerOptionsExtensions
    {
        public static void ExceptionMapper<TException>(this ErrorHandlerOptions options, Func<TException, (int Status, IEnumerable<KeyValuePair<string, ErrorDetails>> Errors)> configureMapper)
            where TException : Exception
        {
            var exceptionType = typeof(TException);

            if(options.ExceptionMappers.ContainsKey(exceptionType))
            {
                options.ExceptionMappers[exceptionType] = new ExceptionMapper<TException>
                {
                    Handler = configureMapper
                };
            }
            else
            {
                options.ExceptionMappers.Add(exceptionType, new ExceptionMapper<TException>
                {
                    Handler = configureMapper
                });
            }
        }

        public static void ExceptionMapper<TException>(this ErrorHandlerOptions options, Func<TException, int> configureMapper)
            where TException : Exception
            => options.ExceptionMapper<TException>(e => (configureMapper(e), ImmutableDictionary<string, ErrorDetails>.Empty));

        public static void ExceptionMapper<TException>(this ErrorHandlerOptions options, Func<TException, (int Status, string Property, string Code, string Description)> configureMapper)
            where TException : Exception => options.ExceptionMapper<TException>(e => (configureMapper(e).Status, new Dictionary<string, ErrorDetails>()
            {
                [configureMapper(e).Property ?? ""] = new(configureMapper(e).Code, configureMapper(e).Description)
            }));
    }
}
