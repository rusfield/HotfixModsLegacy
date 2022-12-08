using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class EdgeGlowEffect
    {
        public int Id { get; set; } = 1;
        public decimal Duration { get; set; } = 0;
        public decimal FadeIn { get; set; } = 0;
        public decimal FadeOut { get; set; } = 0;
        public decimal FresnelCoefficient { get; set; } = 0;
        public decimal GlowRed { get; set; } = 0;
        public decimal GlowGreen { get; set; } = 0;
        public decimal GlowBlue { get; set; } = 0;
        public decimal GlowAlpha { get; set; } = 1;
        public decimal GlowMultiplier { get; set; } = 1;
        public sbyte Flags { get; set; } = 0;
        public decimal InitialDelay { get; set; } = 0;
        public int CurveId { get; set; } = 0;
        public uint Priority { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
