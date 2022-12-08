using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ShadowyEffect
    {
        public int Id { get; set; } = 1;
        public int PrimaryColor { get; set; } = 0;
        public int SecondaryColor { get; set; } = 0;
        public decimal Duration { get; set; } = 0;
        public decimal Value { get; set; } = 0;
        public decimal FadeInTime { get; set; } = 0;
        public decimal FadeOutTime { get; set; } = 0;
        public sbyte AttachPos { get; set; } = -1;
        public sbyte Flags { get; set; } = 0;
        public decimal InnerStrength { get; set; } = 1;
        public decimal OuterStrength { get; set; } = 1;
        public decimal InitialDelay { get; set; } = 0;
        public int CurveId { get; set; } = 0;
        public uint Priority { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
