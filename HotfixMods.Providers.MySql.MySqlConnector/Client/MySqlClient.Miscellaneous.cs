using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotfixMods.Providers.MySql.MySqlConnector.Client
{
    public partial class MySqlClient
    {
        object GetValueWithDefault(Type type, object? value)
        {
            return type.ToString() switch
            {
                "System.SByte" => Convert.ToSByte(value),
                "System.Int16" => Convert.ToInt16(value),
                "System.Int32" => Convert.ToInt32(value),
                "System.Int64" => Convert.ToInt64(value),
                "System.Byte" => Convert.ToByte(value),
                "System.UInt16" => Convert.ToUInt16(value),
                "System.UInt32" => Convert.ToUInt32(value),
                "System.UInt64" => Convert.ToUInt64(value),
                "System.String" => Convert.ToString(value)!,
                "System.Decimal" => Convert.ToDecimal(value),
                _ => throw new Exception($"{type} not implemented.")
            };
        }


        // Simple SQL injection protection,
        // remove if needed.
        void ValidateInput(params string?[] inputs)
        {
            var regex = new Regex("^[A-Za-z0-9 ='_-]*$");
            foreach (var input in inputs)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    if (!regex.Match(input).Success)
                        throw new Exception($"'{input}' is not a valid input.");
                }
            }
        }
    }
}
