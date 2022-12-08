using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class OutlineEffect
    {
        public int Id { get; set; } = 1;
        public uint PassiveHighlightColorId { get; set; }
        public uint HighlightColorId { get; set; }
        public int Priority { get; set; }
        public int Flags { get; set; }
        public decimal Range { get; set; }
        public uint UnitConditionId1 { get; set; }
        public uint UnitConditionId2 { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
