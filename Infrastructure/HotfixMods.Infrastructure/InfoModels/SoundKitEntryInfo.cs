namespace HotfixMods.Infrastructure.InfoModels
{
    public class SoundKitEntryInfo : IInfoModel
    {
        public string SoundKitID { get; set; } = "TODO";
        public string FileDataID { get; set; } = "TODO";
        public string Frequency { get; set; } = "TODO";
        public string Volume { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}