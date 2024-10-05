using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests
{
    public sealed class ErrorHandlerOptionsTests
    {
        [Fact]
        public void Error_handler_options_should_be_created_with_default_mapper_to_unauthorized_access_exception()
        {
            // Arrange
            var options = new ErrorHandlerOptions();
            var exception = new UnauthorizedAccessException();
            var type = typeof(UnauthorizedAccessException);


            // Act
            (var status, var errors) = options.ExceptionMappers[type].Handle(exception);


            // Assert
            status.Should().Be(401);
        }

        [Fact]
        public void Error_handler_options_should_be_created_with_default_mapper_to_not_implemented_exception()
        {
            // Arrange
            var options = new ErrorHandlerOptions();
            var exception = new NotImplementedException();
            var type = typeof(NotImplementedException);


            // Act
            (var status, var errors) = options.ExceptionMappers[type].Handle(exception);


            // Assert
            status.Should().Be(501);
        }

        [Fact]
        public void Error_handler_options_should_be_created_with_default_mapper_to_timeout_exception()
        {
            // Arrange
            var options = new ErrorHandlerOptions();
            var exception = new TimeoutException();
            var type = typeof(TimeoutException);


            // Act
            (var status, var errors) = options.ExceptionMappers[type].Handle(exception);


            // Assert
            status.Should().Be(504);
        }

        [Fact]
        public void When_override_exception_handler_should_return_new_behavior()
        {
            // Arrange
            var options = new ErrorHandlerOptions();
            var exception = new TimeoutException();
            var type = typeof(TimeoutException);
            var status = 789;


            // Act
            options.ExceptionMapper<TimeoutException>(_ => (status, new Dictionary<string, ErrorDetails>()));
            (var act, _) = options.ExceptionMappers[type].Handle(exception);


            // Assert
            act.Should().Be(status);
        }

        [Theory]
        [InlineData(789, "property", "code", "description", "property")]
        [InlineData(123, "MokePro", "MokeCd", "MokeDes", "MokePro")]
        [InlineData(987, null, "FakeCode", "FakeDescription", "")]
        public void When_override_exception_handler_passing_all_properties_should_return_new_behavior(int status, string property, string code, string description, string expectedProperty = null)
        {
            // Arrange
            var options = new ErrorHandlerOptions();
            var exception = new TimeoutException();
            var type = typeof(TimeoutException);


            // Act
            options.ExceptionMapper<TimeoutException>(_ => (status, property, code, description));
            (var actStatus, var actError) = options.ExceptionMappers[type].Handle(exception);


            // Assert
            actStatus.Should().Be(status);

            var error = actError.First(f => f.Key == expectedProperty).Value;
            error.Code.Should().Be(code);
            error.Description.Should().Be(description);
        }
    }
}
