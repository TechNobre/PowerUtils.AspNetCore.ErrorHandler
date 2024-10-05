using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests
{
    public sealed class ModelStateExtensionsTests
    {
        [Fact]
        public void When_model_state_does_not_have_errors_should_return_empty_when_check_payload_too_large_and_return_error()
        {
            // Arrange
            var modelState = new ModelStateDictionary();


            // Act
            var act = modelState.CheckPayloadTooLargeAndReturnError();


            // Assert
            act.Should().BeEmpty();
        }

        [Theory]
        [InlineData("Failed to read the request form. Multipart body length limit 1048576")]
        [InlineData("21 exceeded.")]
        [InlineData("error")]
        public void When_model_state_has_errors_but_isnt_payload_too_large_should_return_empty_when_check_payload_too_large_and_return_error(string errorMessage)
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("key", errorMessage);


            // Act
            var act = modelState.CheckPayloadTooLargeAndReturnError();


            // Assert
            act.Should().BeEmpty();
        }

        [Fact]
        public void When_model_state_has_payload_too_large_error_should_return_error_when_check_payload_too_large_and_return_error()
        {
            // Arrange
            var lengthLimit = 1048576;

            var modelState = new ModelStateDictionary();
            modelState.AddModelError("key", $"Failed to read the request form. Multipart body length limit {lengthLimit} exceeded.");


            // Act
            var act = modelState
                .CheckPayloadTooLargeAndReturnError()
                .First().Value;


            // Assert
            act.Code.Should().Be($"MAX:{lengthLimit}");
            act.Description.Should().Be("The payload is too big.");
        }
    }
}
