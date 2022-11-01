using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Razor.Business
{
    public static class ComponentHelper
    {
        public static string GetHelperText<T>()
        {
            var nType = Nullable.GetUnderlyingType(typeof(T))?.ToString();
            var type = typeof(T).ToString();
            return (nType ?? type) switch
            {
                "System.SByte" => "(int8)",
                "System.Int16" => "(int16)",
                "System.Int32" => "(int32)",
                "System.Int64" => "(int64)",
                "System.Byte" => "(uint8)",
                "System.UInt16" => "(uint16)",
                "System.UInt32" => "(uint32)",
                "System.UInt64" => "(uint64)",
                "System.String" => "(text)",
                "System.Decimal" => "(float)",
                _ => $"({typeof(T)})"
            };
        }
    }
}
