namespace HotfixMods.Infrastructure.InfoModels
{
    public class SpellVisualKitModelAttachInfo : IInfoModel
    {
        public string Offset0 { get; set; } = "The position of the attachment relative to the user.\r\nA positive value will move the attachment forward and a negative value will move the attachment backward.\r\nValue 0.5 will be quite noticable.";
        public string Offset1 { get; set; } = "The position of the attachment relative to the user.\r\nA positive value will move the attachment to the right and a negative value will move the attachment to the left.\r\nValue 0.5 will be quite noticable.";
        public string Offset2 { get; set; } = "The position of the attachment relative to the user.\r\nA positive value will move the attachment up and a negative value will move the attachment down.\r\nValue 0.5 will be quite noticable.";
        public string OffsetVariation0 { get; set; } = "TODO";
        public string OffsetVariation1 { get; set; } = "TODO";
        public string OffsetVariation2 { get; set; } = "TODO";
        public string SpellVisualEffectNameID { get; set; } = "TODO";
        public string AttachmentID { get; set; } = "TODO";
        public string PositionerID { get; set; } = "TODO";
        public string Yaw { get; set; } = "TODO";
        public string Pitch { get; set; } = "TODO";
        public string Roll { get; set; } = "TODO";
        public string YawVariation { get; set; } = "TODO";
        public string PitchVariation { get; set; } = "TODO";
        public string RollVariation { get; set; } = "TODO";
        public string Scale { get; set; } = "TODO";
        public string ScaleVariation { get; set; } = "TODO";
        public string StartAnimID { get; set; } = "TODO";
        public string AnimID { get; set; } = "TODO";
        public string EndAnimID { get; set; } = "TODO";
        public string AnimKitID { get; set; } = "TODO";
        public string Flags { get; set; } = "TODO";
        public string LowDefModelAttachID { get; set; } = "TODO";
        public string StartDelay { get; set; } = "TODO";
        public string Field_9_0_1_33978_021 { get; set; } = "TODO";
        public string ParentSpellVisualKitID { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}