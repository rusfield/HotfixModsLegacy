using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Blazor.Business
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

        public static Type? TryGetTabType(string propertyName, Type dtoType)
        {
            string pagesNamespace = "HotfixMods.Infrastructure.Blazor.Pages";
            pagesNamespace += $".{dtoType.Name.Replace("Dto", "")}Tabs";
            var type = Type.GetType($"{pagesNamespace}.{propertyName}_Tab");

            return type;
        }
    }
}
