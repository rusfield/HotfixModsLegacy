namespace HotfixMods.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static T ToMask<T>(this IEnumerable<T> values)
            where T : Enum
        {
            long builtValue = 0;
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                if (values.Contains(value))
                {
                    builtValue |= Convert.ToInt64(value);
                }
            }
            return (T)Enum.Parse(typeof(T), builtValue.ToString());
        }

        public static IEnumerable<T> ToValues<T>(this T flags)
            where T : Enum
        {
            var input = (long)(object)flags;
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                if (flags.HasFlag(value))
                    yield return value;
            }
        }

        public static string ToDisplayString<T>(this T value)
            //where T : Enum
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
