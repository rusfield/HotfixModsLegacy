namespace HotfixMods.Infrastructure.InfoModels
{
    public class AnimKitInfo : IInfoModel
    {
        public string OneShotDuration = "TODO";
        public string OneShotStopAnimKitId = "The next AnimKit to play after current AnimKit has been played as one shot.";
        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = true;
    }
}
