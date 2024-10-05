using FluentAssertions;
using PowerUtils.AspNetCore.ErrorHandler.Handlers;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests.Handlers
{
    public sealed class PropertyHandlerTests
    {
        [Fact]
        public void NastedProperty_Original_SameFormat()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.Original);


            // Act
            var act = handler("Prop1.Prop2");


            // Assert
            act.Should().Be("Prop1.Prop2");
        }

        [Fact]
        public void NastedProperty_SnakeCase_FormattedCamelCase()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.SnakeCase);


            // Act
            var act = handler("PropName.PropValue");


            // Assert
            act.Should().Be("prop_name.prop_value");
        }

        [Fact]
        public void NastedProperty_CamelCase_FormattedCamelCase()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.CamelCase);


            // Act
            var act = handler("Prop1.Prop2");


            // Assert
            act.Should().Be("prop1.prop2");
        }

        [Fact]
        public void OnlyOneLevel_CamelCase_FormattedCamelCase()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.CamelCase);


            // Act
            var act = handler("ClientName");


            // Assert
            act.Should().Be("clientName");
        }

        [Fact]
        public void Null_CamelCase_Empty()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.CamelCase);
            string propertyName = null;


            // Act
            var act = handler(propertyName);


            // Assert
            act.Should()
                .BeEmpty();
        }

        [Fact]
        public void Empty_CamelCase_Empty()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.CamelCase);
            var propertyName = "";


            // Act
            var act = handler(propertyName);


            // Assert
            act.Should()
                .BeEmpty();
        }

        [Fact]
        public void WhiteSpaces_CamelCase_Empty()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.CamelCase);
            var propertyName = "  ";


            // Act
            var act = handler(propertyName);


            // Assert
            act.Should()
                .BeEmpty();
        }

        [Fact]
        public void OneCharacter_CamelCase_Lower()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.CamelCase);
            var propertyName = "G";


            // Act
            var act = handler(propertyName);


            // Assert
            act.Should()
                .Be("g");
        }

        [Fact]
        public void OnlyOneDot_CamelCase_Dot()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.CamelCase);
            var propertyName = ".";


            // Act
            var act = handler(propertyName);


            // Assert
            act.Should()
                .Be(".");
        }

        [Fact]
        public void EndWitDot_CamelCase_Format()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.CamelCase);
            var propertyName = "Hello.";


            // Act
            var act = handler(propertyName);


            // Assert
            act.Should()
                .Be("hello.");
        }

        [Fact]
        public void Null_SnakeCase_Empty()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.SnakeCase);
            string propertyName = null;


            // Act
            var act = handler(propertyName);


            // Assert
            act.Should()
                .BeEmpty();
        }

        [Fact]
        public void Empty_SnakeCase_Empty()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.SnakeCase);
            var propertyName = "";


            // Act
            var act = handler(propertyName);


            // Assert
            act.Should()
                .BeEmpty();
        }

        [Fact]
        public void WhiteSpaces_SnakeCase_Empty()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.SnakeCase);
            var propertyName = "  ";


            // Act
            var act = handler(propertyName);


            // Assert
            act.Should()
                .BeEmpty();
        }

        [Fact]
        public void OneCharacter_SnakeCase_Lower()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.SnakeCase);
            var propertyName = "G";


            // Act
            var act = handler(propertyName);


            // Assert
            act.Should()
                .Be("g");
        }

        [Fact]
        public void OnlyOneDot_SnakeCase_Dot()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.SnakeCase);
            var propertyName = ".";


            // Act
            var act = handler(propertyName);


            // Assert
            act.Should()
                .Be(".");
        }

        [Fact]
        public void EndWitDot_SnakeCase_Format()
        {
            // Arrange
            var handler = PropertyHandler.Create(PropertyNamingPolicy.SnakeCase);
            var propertyName = "Hello.";


            // Act
            var act = handler(propertyName);


            // Assert
            act.Should()
                .Be("hello.");
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("TestSC", "test_sc")]
        [InlineData("testSC", "test_sc")]
        [InlineData("TestSnakeCase", "test_snake_case")]
        [InlineData("testSnakeCase", "test_snake_case")]
        [InlineData("TestSnakeCase123", "test_snake_case123")]
        [InlineData("_testSnakeCase123", "_test_snake_case123")]
        [InlineData("test_SC", "test_sc")]
        [InlineData("Test", "test")]
        [InlineData("TEST", "test")]
        [InlineData("Test Snake Case", "test_snake_case")]
        [InlineData("TEST SNAKE CASE", "test_snake_case")]
        [InlineData("Test  Snake Case", "test_snake_case")]
        [InlineData("Test SnakeCase", "test_snake_case")]
        [InlineData("Test Snake111 Case", "test_snake111_case")]
        [InlineData("TEST SNAKE111 CASE", "test_snake111_case")]
        [InlineData("Test Snake Case 111", "test_snake_case_111")]
        [InlineData("Test 123 Case 111", "test_123_case_111")]
        [InlineData("Test123 Case 111", "test123_case_111")]
        [InlineData("Test 123Case 111", "test_123case_111")]
        [InlineData("Test 123Case__", "test_123case__")]
        [InlineData("Test 123Case1__", "test_123case1__")]
        public void AnyString_ToSnakeCase_FormattedInSnakeCase(string input, string expected)
        {
            // Arrange &&  Act
            var act = input.ToSnakeCase();


            // Assert
            act.Should()
                .Be(expected);
        }
    }
}
