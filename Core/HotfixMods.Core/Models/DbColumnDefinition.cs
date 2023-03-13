namespace HotfixMods.Core.Models
{
    public class DbColumnDefinition
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsLocalized { get; set; }
        public bool IsIndex { get; set; }
        public bool IsParentIndex { get; set; }
        public string? ReferenceDb2 { get; set; }
        public string? ReferenceDb2Field { get; set; }

        public Type GetServerType()
        {
            if (IsIndex)
            {
                return typeof(uint);
            }
            else if (IsParentIndex)
            {
                // Force unsigned
                return Type.ToString() switch
                {
                    "System.SByte" => typeof(byte),
                    "System.Int16" => typeof(ushort),
                    "System.Int32" => typeof(uint),
                    "System.Int64" => typeof(ulong),
                    _ => Type
                };
            }
            else
            {
                return Type;
            }
        }
    }
}
