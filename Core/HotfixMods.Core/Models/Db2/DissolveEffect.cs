using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class DissolveEffect
    {
        public int Id { get; set; } = 1;
        public decimal Ramp { get; set; }
        public decimal StartValue { get; set; }
        public decimal EndValue { get; set; }
        public decimal FadeInTime { get; set; }
        public decimal FadeOutTime { get; set; }
        public decimal Duration { get; set; }
        public sbyte AttachId { get; set; }
        public sbyte ProjectionType { get; set; }
        public int TextureBlendSetId { get; set; }
        public decimal Scale { get; set; }
        public int Flags { get; set; }
        public int CurveId { get; set; }
        public uint Priority { get; set; }
        public decimal FresnelIntensity { get; set; }
        public decimal Field_9_1_5_40496_014 { get; set; }
        public decimal Field_9_1_5_40496_015 { get; set; }
        public decimal Field_9_1_5_40496_016 { get; set; }
        public decimal Field_9_1_5_40496_017 { get; set; }
        public decimal Field_9_1_5_40496_018 { get; set; }
        public int Field_10_0_0_44649_019 { get; set; }
        public int Field_10_0_0_44649_020 { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
