using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class BarrageEffect
    {
        public int Id { get; set; } = 1;
        public int Flags { get; set; } = 0;
        public decimal Field_8_1_0_28616_001Max { get; set; } = 0;
        public decimal Field_8_1_0_28616_001Min { get; set; } = 0;
        public int Field_8_1_0_28440_003Max { get; set; } = 0;
        public int Field_8_1_0_28440_003Min { get; set; } = 0;
        public int Field_8_1_0_28440_005 { get; set; } = 0;
        public int ModelCountMax { get; set; } = 0;
        public int ModelCountMin { get; set; } = 0;
        public sbyte AttachmentPoint { get; set; } = -1;
        public int SpellVisualEffectNameId { get; set; } = 0;
        public decimal ConeAngle { get; set; } = 0;
        public decimal Range { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
