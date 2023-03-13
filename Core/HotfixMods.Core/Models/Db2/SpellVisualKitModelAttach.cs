using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualKitModelAttach
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public decimal Offset0 { get; set; } = 0;
        public decimal Offset1 { get; set; } = 0;
        public decimal Offset2 { get; set; } = 0;
        public decimal OffsetVariation0 { get; set; } = 0;
        public decimal OffsetVariation1 { get; set; } = 0;
        public decimal OffsetVariation2 { get; set; } = 0;
        public int SpellVisualEffectNameID { get; set; } = 0;
        public int AttachmentID { get; set; } = 0;
        public int PositionerID { get; set; } = 0;
        public decimal Yaw { get; set; } = 0;
        public decimal Pitch { get; set; } = 0;
        public decimal Roll { get; set; } = 0;
        public decimal YawVariation { get; set; } = 0;
        public decimal PitchVariation { get; set; } = 0;
        public decimal RollVariation { get; set; } = 0;
        public decimal Scale { get; set; } = 1;
        public decimal ScaleVariation { get; set; } = 0;
        public int StartAnimID { get; set; } = -1;
        public int AnimID { get; set; } = -1;
        public int EndAnimID { get; set; } = -1;
        public int AnimKitID { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public int LowDefModelAttachID { get; set; } = 0;
        public decimal StartDelay { get; set; } = 0;
        public decimal Field_9_0_1_33978_021 { get; set; } = 0;
        [ParentIndexField]
        public int ParentSpellVisualKitID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
