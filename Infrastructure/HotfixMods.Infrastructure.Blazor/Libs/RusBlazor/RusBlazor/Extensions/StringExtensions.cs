using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RusBlazor.Extensions
{
    public static class StringExtensions
    {
        public static bool TryParseAsT<T>(this string str, out T value)
        {
            var type = typeof(T);
            value = default;

            if (string.IsNullOrWhiteSpace(str))
                return false;

            var tryParseMethod = type.GetMethod("TryParse", new[] { typeof(string), type.MakeByRefType() });

            if (tryParseMethod != null)
            {
                var parameters = new object[] { str, null };
                bool success = (bool)tryParseMethod.Invoke(null, parameters);

                if (success)
                {
                    value = (T)parameters[1];
                }

                return success;
            }

            return false;
        }
    }
}
