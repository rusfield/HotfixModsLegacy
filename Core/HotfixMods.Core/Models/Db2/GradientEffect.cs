using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class GradientEffect
    {
        public int Id { get; set; } = 1;
        public decimal Colors0_r { get; set; } = 0;
        public decimal Colors0_g { get; set; } = 0;
        public decimal Colors0_b { get; set; } = 0;
        public decimal Colors1_r { get; set; } = 0;
        public decimal Colors1_g { get; set; } = 0;
        public decimal Colors1_b { get; set; } = 0;
        public decimal Colors2_r { get; set; } = 0;
        public decimal Colors2_g { get; set; } = 0;
        public decimal Colors2_b { get; set; } = 0;
        public decimal Alpha1 { get; set; } = 1;
        public decimal Alpha2 { get; set; } = 1;
        public decimal EdgeColor_r { get; set; } = 0;
        public decimal EdgeColor_g { get; set; } = 0;
        public decimal EdgeColor_b { get; set; } = 0;
        public int Field_8_1_0_28440_014 { get; set; } = 0;
        public int Field_8_1_0_28440_015 { get; set; } = 0;
        public decimal Field_10_0_5_47118_016 { get; set; } = 0;
        public decimal Field_10_0_5_47118_017 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
