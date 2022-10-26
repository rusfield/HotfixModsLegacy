using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class Item
    {
        public int Id { get; set; }
        public byte ClassId { get; set; }
        public byte SubclassId { get; set; }
        public byte Material { get; set; }
        public sbyte InventoryType { get; set; }
        public byte SheatheType { get; set; }
        public sbyte Sound_override_subclassId { get; set; }
        public int IconFileDataId { get; set; }
        public byte ItemGroupSoundsId { get; set; }
        public int ContentTuningId { get; set; }
        public int ModifiedCraftingReagentItemId { get; set; }
        public int CraftingQualityId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
