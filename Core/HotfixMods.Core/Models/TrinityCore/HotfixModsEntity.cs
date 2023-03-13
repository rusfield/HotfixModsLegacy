using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [HotfixesSchema]
    public class HotfixModsEntity
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public string? Name { get; set; } = "";
        public int RecordID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
