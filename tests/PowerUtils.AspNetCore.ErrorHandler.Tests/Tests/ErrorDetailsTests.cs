using FluentAssertions;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests
{
    public class ErrorDetailsTestsTests
    {
        [Fact]
        public void ErrorDetails_ToStringAndimplicit_Equals()
        {
            // Arrange
            var errors1 = new ErrorDetails { Code = "FakeCode", Description = "FakeDescription" };
            var errors2 = new ErrorDetails { Code = "FakeCode", Description = "FakeDescription" };


            // Act
            var act1 = errors1.ToString();
            string act2 = errors2;


            // Assert
            act1.Should().Be(act2);
        }
    }
}
