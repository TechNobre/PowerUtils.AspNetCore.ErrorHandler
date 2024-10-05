using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Fakes;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests
{
    public sealed class ApiProblemDetailsFactoryTests
    {
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions = Substitute.For<IOptions<ApiBehaviorOptions>>();
        private readonly IOptions<ErrorHandlerOptions> _errorHandlerOptions = Substitute.For<IOptions<ErrorHandlerOptions>>();
        private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();

        public ApiProblemDetailsFactoryTests()
            => _apiBehaviorOptions.Value
                .Returns(new ApiBehaviorOptions());



        [Fact]
        public void When_create_problem_details_without_errors_should_have_unknown_error()
        {
            // Arrange
            _errorHandlerOptions.Value
                .Returns(new ErrorHandlerOptions
                {
                    PropertyNamingPolicy = PropertyNamingPolicy.Original
                });

            var factory = new ApiProblemDetailsFactory(
                _httpContextAccessor,
                _apiBehaviorOptions,
                _errorHandlerOptions);

            var httpContext = new FakeHttpContext();


            // Act
            var act = factory.Create(httpContext);


            // Assert
            act.Status.Should().Be(0);
            act.Type.Should().Be("https://tools.ietf.org/html/rfc7231#section-6");
            act.Title.Should().Be("Unknown error");
            act.Instance.Should().BeNull();
            act.TraceId.Should().BeNull();
        }

        [Theory]
        [InlineData(0, "An unexpected error has occurred.")]
        [InlineData(400, "One or more validation errors occurred.")]
        [InlineData(401, "A authentication error has occurred.")]
        [InlineData(403, "A permissions error has occurred.")]
        [InlineData(404, "The entity was not found.")]
        [InlineData(409, "The entity already exists.")]
        [InlineData(422, "One or more validation errors occurred.")]
        [InlineData(500, "An unexpected error has occurred.")]
        [InlineData(700, "An unexpected error has occurred.")]
        public void Validate_property_detail_by_status_code(int statusCode, string details)
        {
            // Arrange
            var factory = new ApiProblemDetailsFactory(
                _httpContextAccessor,
                _apiBehaviorOptions,
                _errorHandlerOptions);


            // Act
            var act = factory.CreateProblem(statusCode: statusCode);


            // Assert
            act.Detail.Should().Be(details);
        }

        [Fact]
        public void When_property_detail_is_filled_should_not_change()
        {
            // Arrange
            var details = "#Fake#Details";

            var factory = new ApiProblemDetailsFactory(
                _httpContextAccessor,
                _apiBehaviorOptions,
                _errorHandlerOptions);


            // Act
            var act = factory.CreateProblem(detail: details);


            // Assert
            act.Detail.Should().Be(details);
        }

        [Fact]
        public void When_there_are_more_than_one_error_property_detail_should_be_description_of_the_first_error()
        {
            // Arrange
            var factory = new ApiProblemDetailsFactory(
                _httpContextAccessor,
                _apiBehaviorOptions,
                Options.Create(new ErrorHandlerOptions()));

            var errors = new Dictionary<string, ErrorDetails>
            {
                ["error1"] = new ErrorDetails { Description = "Error one" },
                ["error2"] = new ErrorDetails { Description = "Error two" }
            };


            // Act
            var act = factory.CreateProblem(errors: errors);


            // Assert
            act.Detail.Should().Be("Error one");
        }

        [Fact]
        public void When_create_problem_details_without_errors_property_detail_should_be_unexpected_error()
        {
            // Arrange
            var factory = new ApiProblemDetailsFactory(
                _httpContextAccessor,
                _apiBehaviorOptions,
                Options.Create(new ErrorHandlerOptions()));


            // Act
            var act = factory.CreateProblem();


            // Assert
            act.Detail.Should().Be("An unexpected error has occurred.");
        }
    }
}
