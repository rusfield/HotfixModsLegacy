using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [HotfixesSchema]
    public class HotfixModsEntity
    {
        [IndexField]
        public uint Id { get; set; } = 0;
        public string? Name { get; set; } = "";
        public uint RecordId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
