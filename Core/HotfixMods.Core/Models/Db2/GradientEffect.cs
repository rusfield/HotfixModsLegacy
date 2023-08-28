using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class GradientEffect
    {
        
        public int ID { get; set; } = 0;
        public decimal Colors0R { get; set; } = 0;
        public decimal Colors0G { get; set; } = 0;
        public decimal Colors0B { get; set; } = 0;
        public decimal Colors1R { get; set; } = 0;
        public decimal Colors1G { get; set; } = 0;
        public decimal Colors1B { get; set; } = 0;
        public decimal Colors2R { get; set; } = 0;
        public decimal Colors2G { get; set; } = 0;
        public decimal Colors2B { get; set; } = 0;
        public decimal Alpha1 { get; set; } = 1;
        public decimal Alpha2 { get; set; } = 1;
        public decimal EdgeColorR { get; set; } = 0;
        public decimal EdgeColorG { get; set; } = 0;
        public decimal EdgeColorB { get; set; } = 0;
        public int Field_8_1_0_28440_014 { get; set; } = 0;
        public int Field_8_1_0_28440_015 { get; set; } = 0;
        public decimal Field_10_0_5_47118_016 { get; set; } = 0;
        public decimal Field_10_0_5_47118_017 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
