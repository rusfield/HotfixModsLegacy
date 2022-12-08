using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class BarrageEffect
    {
        public int Id { get; set; } = 1;
        public int Flags { get; set; }
        public decimal Field_8_1_0_28616_001Max { get; set; }
        public decimal Field_8_1_0_28616_001Min { get; set; }
        public int Field_8_1_0_28440_003Max { get; set; }
        public int Field_8_1_0_28440_003Min { get; set; }
        public int Field_8_1_0_28440_005 { get; set; }
        public int ModelCountMax { get; set; }
        public int ModelCountMin { get; set; }
        public sbyte AttachmentPoint { get; set; }
        public int SpellVisualEffectNameId { get; set; }
        public decimal ConeAngle { get; set; }
        public decimal Range { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
