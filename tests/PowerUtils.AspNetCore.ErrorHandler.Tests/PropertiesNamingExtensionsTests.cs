namespace PowerUtils.AspNetCore.ErrorHandler.Tests;
public class PropertiesNamingExtensionsTests
{

    [Fact(DisplayName = "Format to camel case null value - Should return empty")]
    public void FormatToCamelCase_Null_Empty()
    {
        // Arrange
        string propertyName = null;


        // Act
        var act = propertyName.FormatToCamelCase();


        // Assert
        act.Should()
            .BeEmpty();
    }

    [Fact(DisplayName = "Format to camel case empty value - Should return empty")]
    public void FormatToCamelCase_Empty_Empty()
    {
        // Arrange
        var propertyName = "";


        // Act
        var act = propertyName.FormatToCamelCase();


        // Assert
        act.Should()
            .BeEmpty();
    }

    [Fact(DisplayName = "Format to camel case white spaces value - Should return empty")]
    public void FormatToCamelCase_WhiteSpaces_Empty()
    {
        // Arrange
        var propertyName = "  ";


        // Act
        var act = propertyName.FormatToCamelCase();


        // Assert
        act.Should()
            .BeEmpty();
    }

    [Fact(DisplayName = "Format to camel case one character - Should return the character lowercase")]
    public void FormatToCamelCase_OneCharacter_Lower()
    {
        // Arrange
        var propertyName = "G";


        // Act
        var act = propertyName.FormatToCamelCase();


        // Assert
        act.Should()
            .Be("g");
    }

    [Fact(DisplayName = "Format to camel case a text with only one dot - Should return a dot")]
    public void FormatToCamelCase_OnlyOneDot_Dot()
    {
        // Arrange
        var propertyName = ".";


        // Act
        var act = propertyName.FormatToCamelCase();


        // Assert
        act.Should()
            .Be(".");
    }

    [Fact(DisplayName = "Format to camel case a text ended with an dot - Should format")]
    public void FormatToCamelCase_EndWitDot_Format()
    {
        // Arrange
        var propertyName = "Hello.";


        // Act
        var act = propertyName.FormatToCamelCase();


        // Assert
        act.Should()
            .Be("hello.");
    }



    [Fact(DisplayName = "Format to snake case null value - Should return empty")]
    public void FormatToSnakeCase_Null_Empty()
    {
        // Arrange
        string propertyName = null;


        // Act
        var act = propertyName.FormatToSnakeCase();


        // Assert
        act.Should()
            .BeEmpty();
    }

    [Fact(DisplayName = "Format to snake case empty value - Should return empty")]
    public void FormatToSnakeCase_Empty_Empty()
    {
        // Arrange
        var propertyName = "";


        // Act
        var act = propertyName.FormatToSnakeCase();


        // Assert
        act.Should()
            .BeEmpty();
    }

    [Fact(DisplayName = "Format to snake case white spaces value - Should return empty")]
    public void FormatToSnakeCase_WhiteSpaces_Empty()
    {
        // Arrange
        var propertyName = "  ";


        // Act
        var act = propertyName.FormatToSnakeCase();


        // Assert
        act.Should()
            .BeEmpty();
    }

    [Fact(DisplayName = "Format to snake case one character - Should return the character lowercase")]
    public void FormatToSnakeCase_OneCharacter_Lower()
    {
        // Arrange
        var propertyName = "G";


        // Act
        var act = propertyName.FormatToSnakeCase();


        // Assert
        act.Should()
            .Be("g");
    }

    [Fact(DisplayName = "Format to snake case a text with only one dot - Should return a dot")]
    public void FormatToSnakeCase_OnlyOneDot_Dot()
    {
        // Arrange
        var propertyName = ".";


        // Act
        var act = propertyName.FormatToSnakeCase();


        // Assert
        act.Should()
            .Be(".");
    }

    [Fact(DisplayName = "Format to snake case a text ended with an dot - Should format")]
    public void FormatToSnakeCase_EndWitDot_Format()
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
