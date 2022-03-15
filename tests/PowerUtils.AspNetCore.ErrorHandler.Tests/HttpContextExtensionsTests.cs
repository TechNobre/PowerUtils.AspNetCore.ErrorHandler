using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Fakes;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests;

public class HttpContextExtensionsTests
{
    [Fact(DisplayName = "Get Correlation Id with TraceIdentifier in http context - Should return the value in HttpContext")]
    public void GetCorrelationId_WithTraceIdentifier_TraceIdentifier()
    {
        // Arrange
        var httpContext = new FakeHttpContext();
        httpContext.TraceIdentifier = "fake";


        // Act
        var act = httpContext.GetCorrelationId();


        // Assert
        act.Should().Be("fake");
    }

    [Fact(DisplayName = "Get Correlation Id from null httpContext - Should return a Guid")]
    public void GetCorrelationId_NullHttpContext_Guid()
    {
        // Arrange
        FakeHttpContext httpContext = null;


        // Act
        var act = httpContext.GetCorrelationId();


        // Assert
        act.Should().StartWith("guid:");
    }

    [Fact(DisplayName = "Get Status Code from null httpContext - Should return null")]
    public void GetStatusCode_HttpContextNull_Null()
    {
        // Arrange
        FakeHttpContext httpContext = null;


        // Act
        var act = httpContext.GetStatusCode();


        // Assert
        act.Should().BeNull();
    }

    [Fact(DisplayName = "Get Status Code from null Response - Should return null")]
    public void GetStatusCode_ResponseNull_Null()
    {
        // Arrange
        var httpContext = new FakeHttpContext(null);


        // Act
        var act = httpContext.GetStatusCode();


        // Assert
        act.Should().BeNull();
    }

    [Fact(DisplayName = "Check if the status code is success from http context with status code 200 - Should return False")]
    public void IsNotSuccess_StatusCode200_False()
    {
        // Arrange
        var httpContext = new FakeHttpContext();
        httpContext.Response.StatusCode = 200;


        // Act
        var act = httpContext.IsNotSuccess();


        // Assert
        act.Should().BeFalse();
    }

    [Fact(DisplayName = "Check if the status code is success from null http context - Should return True")]
    public void IsNotSuccess_NullHttpContext_True()
    {
        // Arrange
        FakeHttpContext httpContext = null;


        // Act
        var act = httpContext.IsNotSuccess();


        // Assert
        act.Should().BeTrue();
    }

    [Fact(DisplayName = "Get request endpoint from null http context - Should return null")]
    public void GetRequestEndpoint_NullHttpContext_Null()
    {
        // Arrange
        FakeHttpContext httpContext = null;


        // Act
        var act = httpContext.GetRequestEndpoint();


        // Assert
        act.Should().BeNull();
    }

    [Fact(DisplayName = "Reset response with null response - Should kept null response")]
    public void ResetResponse_WithNullResponse_ReturnHeader()
    {
        // Arrange
        var httpContext = new FakeHttpContext(null);


        // Act
        httpContext.ResetResponse();


        // Assert
        httpContext.Response.Should()
            .BeNull();
    }

    [Fact(DisplayName = "Reset response with header AccessControlAllowOrigin - Should return the header AccessControlAllowOrigin and should remove the 'header'")]
    public void ResetResponse_WithHeaders_ReturnHeader()
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
