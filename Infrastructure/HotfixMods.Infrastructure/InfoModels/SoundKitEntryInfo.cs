namespace HotfixMods.Infrastructure.InfoModels
{
    public class SoundKitEntryInfo : IInfoModel
    {
        public string SoundKitId { get; set; } = "TODO";
        public string FileDataId { get; set; } = "TODO";
        public string Frequency { get; set; } = "TODO";
        public string Volume { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}