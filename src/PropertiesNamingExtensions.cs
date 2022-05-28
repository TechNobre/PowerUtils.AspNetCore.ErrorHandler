using PowerUtils.Text;

namespace PowerUtils.AspNetCore.ErrorHandler
{
    internal static class PropertiesNamingExtensions
    {
        public static string FormatToCamelCase(this string propertyName)
        {
            if(string.IsNullOrWhiteSpace(propertyName))
            {
                return "";
            }

            var propertyParts = propertyName.Split('.');
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

        public static string FormatToSnakeCase(this string propertyName)
        {
            if(string.IsNullOrWhiteSpace(propertyName))
            {
                return "";
            }

            var propertyParts = propertyName.Split('.');

            for(var count = 0; count < propertyParts.Length; count++)
            {
                propertyParts[count] = propertyParts[count].ToSnakeCase();
            }


            return string.Join(".", propertyParts);
        }
    }
}
