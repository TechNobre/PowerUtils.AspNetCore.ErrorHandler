using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests
{
    public sealed class LoggerExtensionsTests
    {
        private readonly ILogger _logger = Substitute.For<ILogger>();


        [Fact]
        public void Should_be_call_debug_when_pass_message()
        {
            // Arrange
            var message = "my custom message";


            // Act
            _logger.Debug(message);


            // Assert
            _logger
                .Received(1)
                .Log(
                    LogLevel.Debug,
                    Arg.Any<EventId>(),
                    Arg.Is<object>(o => o.ToString() == $"[ERROR HANDLER] > {message}"),
                    null,
                    Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public void Should_be_call_error_when_pass_exception_and_message()
        {
            // Arrange
            var exception = new Exception("fake exception");
            var message = "my custom message";


            // Act
            _logger.Error(exception, message);



            // Assert
            _logger
                .Received(1)
                .Log(
                    LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Is<object>(o => o.ToString() == $"[ERROR HANDLER] > {message}"),
                    exception,
                    Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public void Should_be_call_error_when_pass_exception_and_problemDetails()
        {
            // Arrange
            var exception = new Exception("fake exception for problemDetails");
            var problemDetails = new ErrorProblemDetails
            {
                Instance = "fake instance",
                Status = 500
            };


            // Act
            _logger.Error(exception, problemDetails);


            // Assert
            _logger
                .Received(1)
                .Log(
                    LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Is<object>(o => o.ToString() == $"[ERROR HANDLER] > Request: '{problemDetails.Instance}', StatusCode: '{problemDetails.Status}'"),
                    exception,
                    Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public void Should_be_call_error_when_pass_exception_and_problemDetails_and_message()
        {
            // Arrange
            var exception = new Exception("fake exception for problemDetails and message");
            var problemDetails = new ErrorProblemDetails
            {
                Instance = "fake instance",
                Status = 400
            };
            var message = "my custom message for problemDetails";


            // Act
            _logger.Error(exception, problemDetails, message);


            // Assert
            _logger
                .Received(1)
                .Log(
                    LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Is<object>(o => o.ToString() == $"[ERROR HANDLER] > Request: '{problemDetails.Instance}', StatusCode: '{problemDetails.Status}' > {message}"),
                    exception,
                    Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public void When_sanitizing_null_input_should_return_empty_string()
        {
            // Arrange
            var exception = new Exception("fake exception for problemDetails and message");
            var problemDetails = new ErrorProblemDetails
            {
                Instance = null,
                Status = 401
            };
            var message = "my custom message for problemDetails";


            // Act
            _logger.Error(exception, problemDetails, message);


            // Assert
            _logger
                .Received(1)
                .Log(
                    LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Is<object>(o => o.ToString() == $"[ERROR HANDLER] > Request: '', StatusCode: '{problemDetails.Status}' > {message}"),
                    exception,
                    Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public void When_sanitizing_new_line_input_should_replace_with_space()
        {
            // Arrange
            var exception = new Exception("fake exception for problemDetails and message");
            var problemDetails = new ErrorProblemDetails
            {
                Instance = $"fake{Environment.NewLine}instance",
                Status = 404
            };
            var message = "my custom message for problemDetails";


            // Act
            _logger.Error(exception, problemDetails, message);


            // Assert
            _logger
                .Received(1)
                .Log(
                    LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Is<object>(o => o.ToString() == $"[ERROR HANDLER] > Request: 'fake instance', StatusCode: '{problemDetails.Status}' > {message}"),
                    exception,
                    Arg.Any<Func<object, Exception, string>>());
        }
    }
}
