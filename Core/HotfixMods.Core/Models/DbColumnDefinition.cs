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
    }
}
