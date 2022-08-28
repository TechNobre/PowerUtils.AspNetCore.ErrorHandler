using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Fakes;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests
{
    public class HttpContextExtensionsTests
    {
        [Fact]
        public void WithTraceIdentifier_GetCorrelationId_TraceIdentifier()
        {
            // Arrange
            var httpContext = new FakeHttpContext();
            httpContext.TraceIdentifier = "fake";


            // Act
            var act = httpContext.GetCorrelationId();


            // Assert
            act.Should().Be("fake");
        }

        [Fact]
        public void NullHttpContext_GetCorrelationId_Guid()
        {
            // Arrange
            FakeHttpContext httpContext = null;


            // Act
            var act = httpContext.GetCorrelationId();


            // Assert
            act.Should().StartWith("guid:");
        }

        [Fact]
        public void HttpContextNull_GetStatusCode_Null()
        {
            // Arrange
            FakeHttpContext httpContext = null;


            // Act
            var act = httpContext.GetStatusCode();


            // Assert
            act.Should().BeNull();
        }

        [Fact]
        public void ResponseNull_GetStatusCode_Null()
        {
            // Arrange
            var httpContext = new FakeHttpContext(null);


            // Act
            var act = httpContext.GetStatusCode();


            // Assert
            act.Should().BeNull();
        }

        [Fact]
        public void StatusCode200_IsNotSuccess_False()
        {
            // Arrange
            var httpContext = new FakeHttpContext();
            httpContext.Response.StatusCode = 200;


            // Act
            var act = httpContext.IsNotSuccess();


            // Assert
            act.Should().BeFalse();
        }

        [Fact]
        public void NullHttpContext_IsNotSuccess_True()
        {
            // Arrange
            FakeHttpContext httpContext = null;


            // Act
            var act = httpContext.IsNotSuccess();


            // Assert
            act.Should().BeTrue();
        }

        [Fact]
        public void NullHttpContext_GetRequestEndpoint_Null()
        {
            // Arrange
            FakeHttpContext httpContext = null;


            // Act
            var act = httpContext.GetRequestEndpoint();


            // Assert
            act.Should().BeNull();
        }

        [Fact]
        public void WithNullResponse_ResetResponse_ReturnHeader()
        {
            // Arrange
            var httpContext = new FakeHttpContext(null);


            // Act
            httpContext.ResetResponse();


            // Assert
            httpContext.Response.Should()
                .BeNull();
        }

        [Fact]
        public void WithHeaders_ResetResponse_ReturnHeader()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Response.Headers.Add(HeaderNames.AccessControlAllowOrigin, "*");
            httpContext.Response.Headers.Add("Test", "*");


            // Act
            httpContext.ResetResponse();


            // Assert
            httpContext.Response.Headers.Should()
                .HaveCount(5);
            httpContext.Response.Headers.Should()
                .ContainKey(HeaderNames.AccessControlAllowOrigin);
            httpContext.Response.Headers.Should()
                .NotContainKey("Test");
        }
    }
}
