using System;
using System.Globalization;
using System.Text;

namespace PowerUtils.AspNetCore.ErrorHandler.Handlers
{
    internal static class PropertyHandler
    {
        public static Func<string, string> Create(PropertyNamingPolicy propertyNamingPolicy)
            => propertyNamingPolicy switch
            {
                PropertyNamingPolicy.Original => Original,
                PropertyNamingPolicy.SnakeCase => SnakeCase,
                _ => CamelCase,
            };

        public static string Original(string property) => property;

        public static string CamelCase(string property)
        {
            if(string.IsNullOrWhiteSpace(property))
            {
                return "";
            }

            var propertyParts = property.Split('.');
            for(var count = 0; count < propertyParts.Length; count++)
            {
                // Prevent System.IndexOutOfRangeException: 'Index was outside the bounds of the array.' for cases "Hello."
                if(propertyParts[count].Length == 0)
                {
                    continue;
                }

                propertyParts[count] = char.ToLowerInvariant(propertyParts[count][0]) + propertyParts[count][1..];
            }


            return string.Join(".", propertyParts);
        }

        public static string SnakeCase(string property)
        {
            if(string.IsNullOrWhiteSpace(property))
            {
                return "";
            }

            var propertyParts = property.Split('.');

            for(var count = 0; count < propertyParts.Length; count++)
            {
                propertyParts[count] = propertyParts[count].ToSnakeCase();
            }


            return string.Join(".", propertyParts);
        }

        /// <summary>
        /// Convert a text to snake case format
        /// </summary>
        /// <param name="input">Text to be transformed</param>
        /// <returns>Text transformed</returns>
        public static string ToSnakeCase(this string input)
        { // Reference1: https://stackoverflow.com/questions/63055621/how-to-convert-camel-case-to-snake-case-with-two-capitals-next-to-each-other
          // Reference1: https://github.com/efcore/EFCore.NamingConventions/blob/main/EFCore.NamingConventions/Internal/SnakeCaseNameRewriter.cs
            if(string.IsNullOrEmpty(input))
            {
                return input;
            }

            var builder = new StringBuilder(input.Length + Math.Min(2, input.Length / 5));
            var previousCategory = default(UnicodeCategory?);

            for(var currentIndex = 0; currentIndex < input.Length; currentIndex++)
            {
                var currentChar = input[currentIndex];
                if(currentChar == '_')
                {
                    builder.Append('_');
                    previousCategory = null;
                    continue;
                }

                var currentCategory = char.GetUnicodeCategory(currentChar);
                switch(currentCategory)
                {
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:
                        _snakeCaseHandleUppercaseLetterAndTitlecaseLetter(
                            input,
                            currentIndex,
                            previousCategory,
                            builder
                        );

                        currentChar = char.ToLower(currentChar, CultureInfo.InvariantCulture);
                        break;

                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        if(previousCategory == UnicodeCategory.SpaceSeparator)
                        {
                            builder.Append('_');
                        }
                        break;

                    default:
                        if(previousCategory != null)
                        {
                            previousCategory = UnicodeCategory.SpaceSeparator;
                        }
                        continue;
                }

                builder.Append(currentChar);
                previousCategory = currentCategory;
            }

            return builder.ToString();
        }

        private static void _snakeCaseHandleUppercaseLetterAndTitlecaseLetter(string input, int currentIndex, UnicodeCategory? previousCategory, StringBuilder builder)
        {
            if(previousCategory == UnicodeCategory.SpaceSeparator ||
                previousCategory == UnicodeCategory.LowercaseLetter ||
                (previousCategory != UnicodeCategory.DecimalDigitNumber &&
                previousCategory != null &&
                currentIndex > 0 &&
                currentIndex + 1 < input.Length &&
                char.IsLower(input[currentIndex + 1])))
            {
                builder.Append('_');
            }
        }
    }
}
