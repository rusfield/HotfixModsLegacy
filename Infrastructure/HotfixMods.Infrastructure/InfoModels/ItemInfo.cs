namespace HotfixMods.Infrastructure.InfoModels
{
    public class ItemInfo : IInfoModel
    {
        public string ClassID { get; set; } = "TODO";
        public string SubclassID { get; set; } = "TODO";
        public string Material { get; set; } = "TODO";
        public string InventoryType { get; set; } = "TODO";
        public string SheatheType { get; set; } = "TODO";
        public string SoundOverrideSubclassID { get; set; } = "TODO";
        public string IconFileDataID { get; set; } = "TODO";
        public string ItemGroupSoundsID { get; set; } = "TODO";
        public string ContentTuningID { get; set; } = "TODO";
        public string ModifiedCraftingReagentItemID { get; set; } = "TODO";
        public string CraftingQualityID { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = true;
    }
}