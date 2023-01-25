using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class GradientEffect
    {
        public int Id { get; set; } = 1;
        public decimal Colors0_R { get; set; } = 0;
        public decimal Colors0_G { get; set; } = 0;
        public decimal Colors0_B { get; set; } = 0;
        public decimal Colors1_R { get; set; } = 0;
        public decimal Colors1_G { get; set; } = 0;
        public decimal Colors1_B { get; set; } = 0;
        public decimal Colors2_R { get; set; } = 0;
        public decimal Colors2_G { get; set; } = 0;
        public decimal Colors2_B { get; set; } = 0;
        public decimal Alpha1 { get; set; } = 1;
        public decimal Alpha2 { get; set; } = 1;
        public decimal EdgeColor_R { get; set; } = 0;
        public decimal EdgeColor_G { get; set; } = 0;
        public decimal EdgeColor_B { get; set; } = 0;
        public int Field_8_1_0_28440_014 { get; set; } = 0;
        public int Field_8_1_0_28440_015 { get; set; } = 0;
        public decimal Field_10_0_5_47118_016 { get; set; } = 0;
        public decimal Field_10_0_5_47118_017 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
