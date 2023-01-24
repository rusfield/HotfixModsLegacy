namespace HotfixMods.Infrastructure.InfoModels
{
    public class AnimKitConfigInfo : IInfoModel
    {
        public string ConfigFlags = "Configurations for the current segment.\r\n\r\nThese can in many cases be left off.";
        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}
