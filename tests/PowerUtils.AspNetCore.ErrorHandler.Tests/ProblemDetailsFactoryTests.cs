using Microsoft.Extensions.Options;
using Moq;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Fakes;
using PowerUtils.xUnit.Extensions;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests;

public class ProblemDetailsFactoryTests
{
    [Fact(DisplayName = "Create a ProblemDetailsResponse from HttpContext without values")]
    public void Create_HttpContextWithoutValues_ProblemDetailsResponse()
    {
        // Arrange
        var httpContext = new FakeHttpContext();


        // Act
        var act = ProblemDetailsFactory.Create(httpContext);


        // Assert
        act.Status.Should().Be(0);
        act.Type.Should().BeNull();
        act.Title.Should().BeNull();
        act.Title.Should().BeNull();
        act.TraceID.Should().StartWith("guid:");
    }

    [Fact(DisplayName = "Try format a property with PropertyNamingPolicy 'Original' - Should return the same format")]
    public void PropertyNamingPolicy_Original_SameFormat()
    {
        // Arrange
        var options = new ErrorHandlerOptions();
        options.PropertyNamingPolicy = PropertyNamingPolicy.Original;

        var mockOptions = new Mock<IOptions<ErrorHandlerOptions>>();
        mockOptions.SetupGet(s => s.Value).Returns(options);

        var factory = new ProblemDetailsFactory(mockOptions.Object);


        // Act
        var act = factory.InvokeNonPublicMethod<string>("_formatPropertyName", "Prop1.Prop2");


        // Assert
        act.Should().Be("Prop1.Prop2");
    }

    [Fact(DisplayName = "Try format a property with PropertyNamingPolicy 'CamelCase' - Should return the property formatted with CamelCase")]
    public void PropertyNamingPolicy_CamelCase_FormattedCamelCase()
    {
        // Arrange
        var options = new ErrorHandlerOptions();
        options.PropertyNamingPolicy = PropertyNamingPolicy.CamelCase;

        var mockOptions = new Mock<IOptions<ErrorHandlerOptions>>();
        mockOptions.SetupGet(s => s.Value).Returns(options);

        var factory = new ProblemDetailsFactory(mockOptions.Object);


        // Act
        var act = factory.InvokeNonPublicMethod<string>("_formatPropertyName", "Prop1.Prop2");


        // Assert
        act.Should().Be("prop1.prop2");
    }

    [Fact(DisplayName = "Try format a property with PropertyNamingPolicy 'CamelCase' with only one level - Should return the property formatted with CamelCase")]
    public void PropertyNamingPolicy_CamelCaseOnlyOneLevel_FormattedCamelCase()
    {
        // Arrange
        var options = new ErrorHandlerOptions();
        options.PropertyNamingPolicy = PropertyNamingPolicy.CamelCase;

        var mockOptions = new Mock<IOptions<ErrorHandlerOptions>>();
        mockOptions.SetupGet(s => s.Value).Returns(options);

        var factory = new ProblemDetailsFactory(mockOptions.Object);


        // Act
        var act = factory.InvokeNonPublicMethod<string>("_formatPropertyName", "ClientName");


        // Assert
        act.Should().Be("clientName");
    }


    [Fact(DisplayName = "Try format a property with PropertyNamingPolicy 'SnakeCase' - Should return the property formatted with SnakeCase")]
    public void PropertyNamingPolicy_SnakeCase_FormattedCamelCase()
    {
        // Arrange
        var options = new ErrorHandlerOptions();
        options.PropertyNamingPolicy = PropertyNamingPolicy.SnakeCase;

        var mockOptions = new Mock<IOptions<ErrorHandlerOptions>>();
        mockOptions.SetupGet(s => s.Value).Returns(options);

        var factory = new ProblemDetailsFactory(mockOptions.Object);


        // Act
        var act = factory.InvokeNonPublicMethod<string>("_formatPropertyName", "PropName.PropValue");


        // Assert
        act.Should().Be("prop_name.prop_value");
    }
}
