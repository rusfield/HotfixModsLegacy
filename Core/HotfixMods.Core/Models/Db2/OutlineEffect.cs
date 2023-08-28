using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class OutlineEffect
    {
        
        public int ID { get; set; } = 0;
        public uint PassiveHighlightColorID { get; set; } = 0;
        public uint HighlightColorID { get; set; } = 0;
        public int Priority { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public decimal Range { get; set; } = 0;
        public uint UnitConditionId0 { get; set; } = 0;
        public uint UnitConditionId1 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
