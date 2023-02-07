using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class DissolveEffect
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public decimal Ramp { get; set; } = 0;
        public decimal StartValue { get; set; } = 0;
        public decimal EndValue { get; set; } = 0;
        public decimal FadeInTime { get; set; } = 0;
        public decimal FadeOutTime { get; set; } = 0;
        public decimal Duration { get; set; } = 0;
        public sbyte AttachID { get; set; } = -1;
        public sbyte ProjectionType { get; set; } = 0;
        public int TextureBlendSetID { get; set; } = 0;
        public decimal Scale { get; set; } = 1;
        public int Flags { get; set; } = 0;
        public int CurveID { get; set; } = 0;
        public uint Priority { get; set; } = 0;
        public decimal FresnelIntensity { get; set; } = 0;
        public decimal Field_9_1_5_40496_014 { get; set; } = 1;
        public decimal Field_9_1_5_40496_015 { get; set; } = 1;
        public decimal Field_9_1_5_40496_016 { get; set; } = 1;
        public decimal Field_9_1_5_40496_017 { get; set; } = 1;
        public decimal Field_9_1_5_40496_018 { get; set; } = 1;
        public int Field_10_0_0_44649_019 { get; set; } = -1;
        public int Field_10_0_0_44649_020 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
