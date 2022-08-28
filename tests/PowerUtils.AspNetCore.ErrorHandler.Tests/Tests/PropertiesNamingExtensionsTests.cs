using FluentAssertions;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests
{
    public class PropertiesNamingExtensionsTests
    {

        [Fact]
        public void Null_FormatToCamelCase_Empty()
        {
            // Arrange
            string propertyName = null;


            // Act
            var act = propertyName.FormatToCamelCase();


            // Assert
            act.Should()
                .BeEmpty();
        }

        [Fact]
        public void Empty_FormatToCamelCase_Empty()
        {
            // Arrange
            var propertyName = "";


            // Act
            var act = propertyName.FormatToCamelCase();


            // Assert
            act.Should()
                .BeEmpty();
        }

        [Fact]
        public void WhiteSpaces_FormatToCamelCase_Empty()
        {
            // Arrange
            var propertyName = "  ";


            // Act
            var act = propertyName.FormatToCamelCase();


            // Assert
            act.Should()
                .BeEmpty();
        }

        [Fact]
        public void OneCharacter_FormatToCamelCase_Lower()
        {
            // Arrange
            var propertyName = "G";


            // Act
            var act = propertyName.FormatToCamelCase();


            // Assert
            act.Should()
                .Be("g");
        }

        [Fact]
        public void OnlyOneDot_FormatToCamelCase_Dot()
        {
            // Arrange
            var propertyName = ".";


            // Act
            var act = propertyName.FormatToCamelCase();


            // Assert
            act.Should()
                .Be(".");
        }

        [Fact]
        public void EndWitDot_FormatToCamelCase_Format()
        {
            // Arrange
            var propertyName = "Hello.";


            // Act
            var act = propertyName.FormatToCamelCase();


            // Assert
            act.Should()
                .Be("hello.");
        }

        [Fact]
        public void Null_FormatToSnakeCase_Empty()
        {
            // Arrange
            string propertyName = null;


            // Act
            var act = propertyName.FormatToSnakeCase();


            // Assert
            act.Should()
                .BeEmpty();
        }

        [Fact]
        public void Empty_FormatToSnakeCase_Empty()
        {
            // Arrange
            var propertyName = "";


            // Act
            var act = propertyName.FormatToSnakeCase();


            // Assert
            act.Should()
                .BeEmpty();
        }

        [Fact]
        public void WhiteSpaces_FormatToSnakeCase_Empty()
        {
            // Arrange
            var propertyName = "  ";


            // Act
            var act = propertyName.FormatToSnakeCase();


            // Assert
            act.Should()
                .BeEmpty();
        }

        [Fact]
        public void OneCharacter_FormatToSnakeCase_Lower()
        {
            // Arrange
            var propertyName = "G";


            // Act
            var act = propertyName.FormatToSnakeCase();


            // Assert
            act.Should()
                .Be("g");
        }

        [Fact]
        public void OnlyOneDot_FormatToSnakeCase_Dot()
        {
            // Arrange
            var propertyName = ".";


            // Act
            var act = propertyName.FormatToSnakeCase();


            // Assert
            act.Should()
                .Be(".");
        }

        [Fact]
        public void EndWitDot_FormatToSnakeCase_Format()
        {
            // Arrange
            var propertyName = "Hello.";


            // Act
            var act = propertyName.FormatToSnakeCase();


            // Assert
            act.Should()
                .Be("hello.");
        }
    }
}
