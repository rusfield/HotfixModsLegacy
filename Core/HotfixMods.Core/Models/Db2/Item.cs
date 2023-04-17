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
        [Db2Description("Material will decide what kind of sound this item makes in various situations, for example during sheathing or unsheathing a weapon.")]
        public byte Material { get; set; } = 0;
        public sbyte InventoryType { get; set; } = 0;
        [Db2Description("The way the item is displayed on the character when sheathed. Only weapons or other items held in hands should have a value.$Mismatching the Sheathe Type may give the item an unusual look, for example placing one-handed weapons on the back.")]
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
