﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Fakes;
using PowerUtils.xUnit.Extensions;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests
{
    public class ApiProblemDetailsFactoryTests
    {
        private readonly Mock<IOptions<ApiBehaviorOptions>> _apiBehaviorOptions = new();
        private readonly Mock<IOptions<ErrorHandlerOptions>> _errorHandlerOptions = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();

        public ApiProblemDetailsFactoryTests()
            => _apiBehaviorOptions
                .SetupGet(s => s.Value)
                .Returns(new ApiBehaviorOptions());



        [Fact]
        public void HttpContextWithoutValues_Create_ProblemDetailsResponse()
        {
            // Arrange
            _errorHandlerOptions
                .SetupGet(s => s.Value)
                .Returns(new ErrorHandlerOptions
                {
                    PropertyNamingPolicy = PropertyNamingPolicy.Original
                });

            var factory = new ApiProblemDetailsFactory(
                _httpContextAccessor.Object,
                _apiBehaviorOptions.Object,
                _errorHandlerOptions.Object
            );

            var httpContext = new FakeHttpContext();

            // Act
            var act = factory.Create(httpContext);


            // Assert
            act.Status.Should().Be(0);
            act.Type.Should().NotBeNull();
            act.Title.Should().NotBeNull();
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
        public void StatusCode_ApplyDetail_StringDetails(int statusCode, string details)
        {
            // Arrange
            var problemDetails = new ProblemDetails { Status = statusCode };


            // Act
            ObjectInvoker.Invoke(typeof(ApiProblemDetailsFactory), "_applyDetail", problemDetails);


            // Assert
            problemDetails.Detail.Should().Be(details);
        }

        [Fact]
        public void ProblemDetailsWithDetailsFilled_ApplyDetail_SameDetails()
        {
            // Arrange
            var details = "#Fake#Details";
            var problemDetails = new ProblemDetails { Detail = details };


            // Act
            ObjectInvoker.Invoke(typeof(ApiProblemDetailsFactory), "_applyDetail", problemDetails);


            // Assert
            problemDetails.Detail.Should().Be(details);
        }
    }
}
