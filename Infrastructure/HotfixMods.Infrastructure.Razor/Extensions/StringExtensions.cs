using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Razor.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string str, int length = 10)
        {
            if (str.Length <= length)
                return str;

            return str.Substring(0, length - 1) + "...";
        }
    }
}
