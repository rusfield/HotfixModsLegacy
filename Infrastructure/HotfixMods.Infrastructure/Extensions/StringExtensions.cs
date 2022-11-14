using System.Text.RegularExpressions;


namespace HotfixMods.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToTableName(this string str)
        {
            // If there comes any exceptions, add them here
            var output = str switch
            {
                _ => Regex.Replace(str, @"(?<!_|^)([A-Z])", "_$1")
            };

            return output.ToLower();
        }

        public static string ToDisplayName(this string str, string? appendBefore = null, string? appendAfter = null)
        {
            str = str.Replace("_", " ");

            // If there ever comes any exceptions, add them here
            var output = str switch
            {
                _ => Regex.Replace(str, @"(?<!_|^)([A-Z])", " $1")
            };

            if(output.EndsWith("Id", StringComparison.InvariantCulture))
                output = output.Substring(0, output.Length - 2) + "ID";

            return $"{appendBefore ?? ""}{output}{appendAfter ?? ""}";
        }
    }
}
