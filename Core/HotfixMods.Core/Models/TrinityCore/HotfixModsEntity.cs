using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [HotfixesSchema]
    public class HotfixModsEntity
    {
        
        public ulong ID { get; set; } = 0;
        public string? Name { get; set; } = "";
        public ulong RecordID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
