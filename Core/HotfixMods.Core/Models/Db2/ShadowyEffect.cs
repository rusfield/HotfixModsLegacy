using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ShadowyEffect
    {
        public int Id { get; set; } = 1;
        public int PrimaryColor { get; set; }
        public int SecondaryColor { get; set; }
        public decimal Duration { get; set; }
        public decimal Value { get; set; }
        public decimal FadeInTime { get; set; }
        public decimal FadeOutTime { get; set; }
        public sbyte AttachPos { get; set; }
        public sbyte Flags { get; set; }
        public decimal InnerStrength { get; set; }
        public decimal OuterStrength { get; set; }
        public decimal InitialDelay { get; set; }
        public int CurveId { get; set; }
        public uint Priority { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
