using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class EdgeGlowEffect
    {
        public int Id { get; set; } = 1;
        public decimal Duration { get; set; }
        public decimal FadeIn { get; set; }
        public decimal FadeOut { get; set; }
        public decimal FresnelCoefficient { get; set; }
        public decimal GlowRed { get; set; }
        public decimal GlowGreen { get; set; }
        public decimal GlowBlue { get; set; }
        public decimal GlowAlpha { get; set; }
        public decimal GlowMultiplier { get; set; }
        public sbyte Flags { get; set; }
        public decimal InitialDelay { get; set; }
        public int CurveId { get; set; }
        public uint Priority { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
