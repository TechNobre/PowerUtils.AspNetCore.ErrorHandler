using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using PowerUtils.AspNetCore.ErrorHandler.Tests.Fakes;
using PowerUtils.xUnit.Extensions;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests
{
    public class ProblemDetailsFactoryTests
    {
        [Fact]
        public void HttpContextWithoutValues_Create_ProblemDetailsResponse()
        {
            // Arrange
            var apiBehaviorOptions = new Mock<IOptions<ApiBehaviorOptions>>();
            apiBehaviorOptions
                .SetupGet(s => s.Value)
                .Returns(new ApiBehaviorOptions());

            var options = new ErrorHandlerOptions();
            options.PropertyNamingPolicy = PropertyNamingPolicy.Original;

            var errorHandlerOptions = new Mock<IOptions<ErrorHandlerOptions>>();
            errorHandlerOptions
                .SetupGet(s => s.Value)
                .Returns(options);

            var factory = new ProblemDetailsFactory(
               apiBehaviorOptions.Object,
               errorHandlerOptions.Object
            );

            var httpContext = new FakeHttpContext();


            // Act
            var act = factory.Create(httpContext);


            // Assert
            act.Status.Should().Be(0);
            act.Type.Should().NotBeNull();
            act.Title.Should().NotBeNull();
            act.Instance.Should().BeNull();
            act.TraceID.Should().StartWith("guid:");
        }

        [Fact]
        public void Original_PropertyNamingPolicy_SameFormat()
        {
            // Arrange
            var apiBehaviorOptions = new Mock<IOptions<ApiBehaviorOptions>>();

            var options = new ErrorHandlerOptions();
            options.PropertyNamingPolicy = PropertyNamingPolicy.Original;

            var errorHandlerOptions = new Mock<IOptions<ErrorHandlerOptions>>();
            errorHandlerOptions
                .SetupGet(s => s.Value)
                .Returns(options);

            var factory = new ProblemDetailsFactory(
               apiBehaviorOptions.Object,
               errorHandlerOptions.Object
            );


            // Act
            var act = factory.InvokeNonPublicMethod<string>("_formatPropertyName", "Prop1.Prop2");


            // Assert
            act.Should().Be("Prop1.Prop2");
        }

        [Fact]
        public void CamelCase_PropertyNamingPolicy_FormattedCamelCase()
        {
            // Arrange
            var apiBehaviorOptions = new Mock<IOptions<ApiBehaviorOptions>>();

            var options = new ErrorHandlerOptions();
            options.PropertyNamingPolicy = PropertyNamingPolicy.CamelCase;

            var errorHandlerOptions = new Mock<IOptions<ErrorHandlerOptions>>();
            errorHandlerOptions
                .SetupGet(s => s.Value)
                .Returns(options);

            var factory = new ProblemDetailsFactory(
                apiBehaviorOptions.Object,
                errorHandlerOptions.Object
            );


            // Act
            var act = factory.InvokeNonPublicMethod<string>("_formatPropertyName", "Prop1.Prop2");


            // Assert
            act.Should().Be("prop1.prop2");
        }

        [Fact]
        public void CamelCaseOnlyOneLevel_PropertyNamingPolicy_FormattedCamelCase()
        {
            // Arrange
            var apiBehaviorOptions = new Mock<IOptions<ApiBehaviorOptions>>();

            var options = new ErrorHandlerOptions();
            options.PropertyNamingPolicy = PropertyNamingPolicy.CamelCase;

            var errorHandlerOptions = new Mock<IOptions<ErrorHandlerOptions>>();
            errorHandlerOptions
                .SetupGet(s => s.Value)
                .Returns(options);

            var factory = new ProblemDetailsFactory(
                apiBehaviorOptions.Object,
                errorHandlerOptions.Object
            );


            // Act
            var act = factory.InvokeNonPublicMethod<string>("_formatPropertyName", "ClientName");


            // Assert
            act.Should().Be("clientName");
        }


        [Fact]
        public void SnakeCase_PropertyNamingPolicy_FormattedCamelCase()
        {
            // Arrange
            var apiBehaviorOptions = new Mock<IOptions<ApiBehaviorOptions>>();

            var options = new ErrorHandlerOptions();
            options.PropertyNamingPolicy = PropertyNamingPolicy.SnakeCase;

            var mockOptions = new Mock<IOptions<ErrorHandlerOptions>>();
            mockOptions.SetupGet(s => s.Value).Returns(options);

            var errorHandlerOptions = new Mock<IOptions<ErrorHandlerOptions>>();
            errorHandlerOptions
                .SetupGet(s => s.Value)
                .Returns(options);

            var factory = new ProblemDetailsFactory(
                apiBehaviorOptions.Object,
                errorHandlerOptions.Object
            );


            // Act
            var act = factory.InvokeNonPublicMethod<string>("_formatPropertyName", "PropName.PropValue");


            // Assert
            act.Should().Be("prop_name.prop_value");
        }
    }
}
