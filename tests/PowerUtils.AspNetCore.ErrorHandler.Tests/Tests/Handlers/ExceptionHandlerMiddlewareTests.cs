using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PowerUtils.AspNetCore.ErrorHandler.Handlers;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests.Handlers
{
    public sealed class ExceptionHandlerMiddlewareTests
    {
        private readonly ErrorHandlerOptions _errorHandlerOptions = new ErrorHandlerOptions();
        private readonly ILogger _logger = Substitute.For<ILogger>();
        private readonly HttpContext _httpContext;

        public ExceptionHandlerMiddlewareTests()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.Configure<ApiBehaviorOptions>(options => { });
            serviceCollection.Configure<ErrorHandlerOptions>(options =>
            {
                options.ExceptionMappers = _errorHandlerOptions.ExceptionMappers;
                ;
            });

            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ApiProblemDetailsFactory>();

            var loggerFactory = Substitute.For<ILoggerFactory>();
            loggerFactory.CreateLogger("ExceptionHandler").Returns(_logger);

            serviceCollection.AddSingleton(loggerFactory);

            _httpContext = new DefaultHttpContext();
            _httpContext.RequestServices = serviceCollection.BuildServiceProvider();
        }


        [Fact]
        public async Task When_handle_doesnt_have_exception_should_log_error_message_with_status_code_500_and_unknown_error()
        {
            // Arrange && Act
            await ExceptionHandlerMiddleware.Handler(_httpContext);


            // Assert
            _logger
                .Received(1)
                .Log(
                    LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Is<object>(o => o.ToString().EndsWith("StatusCode: '500' > Unknown error")),
                    Arg.Any<Exception>(),
                    Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public async Task When_handle_has_exception_should_log_error_message_with_status_code_500_and_exception()
        {
            // Arrange
            var exception = new Exception("fake exception");

            _httpContext.Features.Set<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>(new Microsoft.AspNetCore.Diagnostics.ExceptionHandlerFeature
            {
                Error = exception
            });


            // Act
            await ExceptionHandlerMiddleware.Handler(_httpContext);


            // Assert
            _logger
                .Received(1)
                .Log(
                    LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Is<object>(o => o.ToString().EndsWith("StatusCode: '500'")),
                    exception,
                    Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public async Task When_handle_has_exception_and_throw_another_handling_current_exception_should_log_error_message_with_status_code_500_and_error_mapping_exception()
        {
            // Arrange
            var exception = new InvalidCastException("Fake invalid cast exception");

            _httpContext.Features.Set<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>(new Microsoft.AspNetCore.Diagnostics.ExceptionHandlerFeature
            {
                Error = exception
            });

            var internalException = new Exception("Fake error mapping exception");

            _errorHandlerOptions.ExceptionMappers.Add(
                typeof(InvalidCastException),
                new ExceptionMapper<InvalidCastException>()
                {
                    Handler = (_) => throw internalException
                });


            // Act
            await ExceptionHandlerMiddleware.Handler(_httpContext);


            // Assert
            _logger
                .Received(1)
                .Log(
                    LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Is<object>(o => o.ToString() == "[ERROR HANDLER] > Error mapping exception"),
                    internalException,
                    Arg.Any<Func<object, Exception, string>>());
        }
    }
}
