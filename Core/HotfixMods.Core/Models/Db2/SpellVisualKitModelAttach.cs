using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualKitModelAttach
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public decimal Offset1 { get; set; } = 0;
        public decimal Offset2 { get; set; } = 0;
        public decimal Offset3 { get; set; } = 0;
        public decimal OffsetVariation1 { get; set; } = 0;
        public decimal OffsetVariation2 { get; set; } = 0;
        public decimal OffsetVariation3 { get; set; } = 0;
        public int SpellVisualEffectNameId { get; set; } = 0;
        public int AttachmentId { get; set; } = 0;
        public int PositionerId { get; set; } = 0;
        public decimal Yaw { get; set; } = 0;
        public decimal Pitch { get; set; } = 0;
        public decimal Roll { get; set; } = 0;
        public decimal YawVariation { get; set; } = 0;
        public decimal PitchVariation { get; set; } = 0;
        public decimal RollVariation { get; set; } = 0;
        public decimal Scale { get; set; } = 1;
        public decimal ScaleVariation { get; set; } = 0;
        public int StartAnimId { get; set; } = -1;
        public int AnimId { get; set; } = -1;
        public int EndAnimId { get; set; } = -1;
        public int AnimKitId { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public int LowDefModelAttachId { get; set; } = 0;
        public decimal StartDelay { get; set; } = 0;
        public decimal Field_9_0_1_33978_021 { get; set; } = 0;
        [ParentIndexField]
        public int ParentSpellVisualKitId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
