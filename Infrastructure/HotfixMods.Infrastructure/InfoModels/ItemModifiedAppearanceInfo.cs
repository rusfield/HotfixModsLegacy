namespace HotfixMods.Infrastructure.InfoModels
{
    public class ItemModifiedAppearanceInfo : IInfoModel
    {
        public string ItemId { get; set; } = "TODO";
        public string ItemAppearanceModifierId { get; set; } = "TODO";
        public string ItemAppearanceId { get; set; } = "TODO";
        public string OrderIndex { get; set; } = "TODO";
        public string TransmogSourceTypeEnum { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}