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
                "System.SByte" => (sbyte)(value ?? 0),
                "System.Int16" => (short)(value ?? 0),
                "System.Int32" => (int)(value ?? 0),
                "System.Int64" => (long)(value ?? 0),
                "System.Byte" => (byte)(value ?? 0),
                "System.UInt16" => (ushort)(value ?? 0),
                "System.UInt32" => (uint)(value ?? 0),
                "System.UInt64" => (ulong)(value ?? 0),
                "System.String" => (string)(value ?? ""),
                "System.Decimal" => (decimal)(value ?? 0),
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
