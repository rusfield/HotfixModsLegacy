namespace HotfixMods.Infrastructure.InfoModels
{
    public class BeamEffectInfo : IInfoModel
    {
        public string BeamID { get; set; } = "TODO";
        public string SourceMinDistance { get; set; } = "TODO";
        public string FixedLength { get; set; } = "TODO";
        public string Flags { get; set; } = "TODO";
        public string SourceOffset { get; set; } = "TODO";
        public string DestOffset { get; set; } = "TODO";
        public string SourceAttachID { get; set; } = "TODO";
        public string DestAttachID { get; set; } = "TODO";
        public string SourcePositionerID { get; set; } = "TODO";
        public string DestPositionerID { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}