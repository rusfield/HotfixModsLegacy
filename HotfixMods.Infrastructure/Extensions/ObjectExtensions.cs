using System.Text.RegularExpressions;

namespace HotfixMods.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        // Mainly used by enums
        public static string ToDisplayString(this object value)
        {
            string result = "";
            if (value == null)
                return result;

            var words = value.ToString().Split("_");
            foreach (var word in words)
            {
                if (word.Length > 1)
                    result += $"{word.Substring(0, 1).ToUpper()}{word.Substring(1, word.Length - 1).ToLower()} ";
                else if (word.Length == 1)
                    result += $"{word.ToString().ToUpper()} ";
            }
            return result.Trim();
        }


    }
}
