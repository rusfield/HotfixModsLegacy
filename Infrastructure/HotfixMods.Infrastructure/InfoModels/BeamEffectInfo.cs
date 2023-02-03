namespace HotfixMods.Infrastructure.InfoModels
{
    public class BeamEffectInfo : IInfoModel
    {
        public string BeamId { get; set; } = "TODO";
        public string SourceMinDistance { get; set; } = "TODO";
        public string FixedLength { get; set; } = "TODO";
        public string Flags { get; set; } = "TODO";
        public string SourceOffset { get; set; } = "TODO";
        public string DestOffset { get; set; } = "TODO";
        public string SourceAttachId { get; set; } = "TODO";
        public string DestAttachId { get; set; } = "TODO";
        public string SourcePositionerId { get; set; } = "TODO";
        public string DestPositionerId { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}