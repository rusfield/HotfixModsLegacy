using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualAnim
    {
        public int Id { get; set; } = 1;
        public int InitialAnimId { get; set; }
        public int LoopAnimId { get; set; }
        public ushort AnimKitId { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
