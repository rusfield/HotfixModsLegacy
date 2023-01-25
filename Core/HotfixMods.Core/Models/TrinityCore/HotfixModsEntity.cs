using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [HotfixesSchema]
    public class HotfixModsEntity
    {
        [IndexField]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int RecordId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
