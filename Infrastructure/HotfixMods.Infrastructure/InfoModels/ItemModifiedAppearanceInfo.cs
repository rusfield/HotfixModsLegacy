namespace HotfixMods.Infrastructure.InfoModels
{
    public class ItemModifiedAppearanceInfo : IInfoModel
    {
        public string ItemID { get; set; } = "TODO";
        public string ItemAppearanceModifierID { get; set; } = "TODO";
        public string ItemAppearanceID { get; set; } = "TODO";
        public string OrderIndex { get; set; } = "TODO";
        public string TransmogSourceTypeEnum { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}