using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualKitModelAttach
    {
        public int Id { get; set; }
        public decimal Offset1 { get; set; }
        public decimal Offset2 { get; set; }
        public decimal Offset3 { get; set; }
        public decimal OffsetVariation1 { get; set; }
        public decimal OffsetVariation2 { get; set; }
        public decimal OffsetVariation3 { get; set; }
        public int SpellVisualEffectNameId { get; set; }
        public int AttachmentId { get; set; }
        public int PositionerId { get; set; }
        public decimal Yaw { get; set; }
        public decimal Pitch { get; set; }
        public decimal Roll { get; set; }
        public decimal YawVariation { get; set; }
        public decimal PitchVariation { get; set; }
        public decimal RollVariation { get; set; }
        public decimal Scale { get; set; }
        public decimal ScaleVariation { get; set; }
        public int StartAnimId { get; set; }
        public int AnimId { get; set; }
        public int EndAnimId { get; set; }
        public int AnimKitId { get; set; }
        public int Flags { get; set; }
        public int LowDefModelAttachId { get; set; }
        public decimal StartDelay { get; set; }
        public decimal Field_9_0_1_33978_021 { get; set; }
        public int ParentSpellVisualKitId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
