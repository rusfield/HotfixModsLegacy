using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class Item
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public byte ClassID { get; set; } = 0;
        public byte SubclassID { get; set; } = 0;
        public byte Material { get; set; } = 0;
        public sbyte InventoryType { get; set; } = 0;
        public byte SheatheType { get; set; } = 0;
        public sbyte SoundOverrideSubclassID { get; set; } = 0;
        public int IconFileDataID { get; set; } = 0;
        public byte ItemGroupSoundsID { get; set; } = 0;
        public int ContentTuningID { get; set; } = 0;
        public int ModifiedCraftingReagentItemID { get; set; } = 0;
        public int CraftingQualityID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
