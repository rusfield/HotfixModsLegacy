using HotfixMods.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if(flags.HasFlag(value))
                    yield return value;
            }
        }

        public static string ToDisplayString<T>(this T enumValue)
            //where T : Enum
        {
            string result = "";
            var enumStrings = enumValue.ToString().Split("_");
            foreach (var enumString in enumStrings)
            {
                if (enumString.Length > 1)
                    result += $"{enumString.Substring(0, 1).ToUpper()}{enumString.Substring(1, enumString.Length - 1).ToLower()} ";
                else if (enumString.Length == 1)
                    result += $"{enumString.ToString().ToUpper()} ";
            }
            return result.Trim();
        }
    }
}
