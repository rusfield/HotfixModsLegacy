using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class OutlineEffect
    {
        public int Id { get; set; } = 1;
        public uint PassiveHighlightColorId { get; set; } = 0;
        public uint HighlightColorId { get; set; } = 0;
        public int Priority { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public decimal Range { get; set; } = 0;
        public uint UnitConditionId1 { get; set; } = 0;
        public uint UnitConditionId2 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
