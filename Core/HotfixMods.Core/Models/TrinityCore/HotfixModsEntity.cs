using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [HotfixesSchema]
    public class HotfixModsEntity
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public string? Name { get; set; } = "";
        public int RecordId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
