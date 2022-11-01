using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToTableName(this string str)
        {
            // If there ever comes any exceptions, add them here
            return str switch
            {
                _ => Regex.Replace(str, @"(?<!_|^)([A-Z])", "_$1")
            };
        }

        public static string ToDisplayName(this string str)
        {
            // If there ever comes any exceptions, add them here
            return str switch
            {
                _ => Regex.Replace(str, @"(?<!_|^)([A-Z])", " $1")
            };
        }
    }
}
