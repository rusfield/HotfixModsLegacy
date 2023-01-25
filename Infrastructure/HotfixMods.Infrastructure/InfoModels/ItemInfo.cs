namespace HotfixMods.Infrastructure.InfoModels
{
    public class ItemInfo : IInfoModel
    {
        public string ClassId { get; set; } = "TODO";
        public string SubclassId { get; set; } = "TODO";
        public string Material { get; set; } = "TODO";
        public string InventoryType { get; set; } = "TODO";
        public string SheatheType { get; set; } = "TODO";
        public string SoundOverrideSubclassId { get; set; } = "TODO";
        public string IconFileDataId { get; set; } = "TODO";
        public string ItemGroupSoundsId { get; set; } = "TODO";
        public string ContentTuningId { get; set; } = "TODO";
        public string ModifiedCraftingReagentItemId { get; set; } = "TODO";
        public string CraftingQualityId { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}