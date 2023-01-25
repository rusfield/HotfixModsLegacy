using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class Item
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public byte ClassId { get; set; } = 0;
        public byte SubclassId { get; set; } = 0;
        public byte Material { get; set; } = 0;
        public sbyte InventoryType { get; set; } = 0;
        public byte SheatheType { get; set; } = 0;
        public sbyte SoundOverrideSubclassId { get; set; } = 0;
        public int IconFileDataId { get; set; } = 0;
        public byte ItemGroupSoundsId { get; set; } = 0;
        public int ContentTuningId { get; set; } = 0;
        public int ModifiedCraftingReagentItemId { get; set; } = 0;
        public int CraftingQualityId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
