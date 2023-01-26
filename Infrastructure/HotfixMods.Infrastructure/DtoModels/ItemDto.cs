using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class ItemDto : DtoBase
    {
        public ItemDto(): base(nameof(Item)) { }

        public Item Item { get; set; } = new();
        public ItemAppearance? ItemAppearance { get; set; }
        public ItemDisplayInfo? ItemDisplayInfo { get; set; }
        public ItemModifiedAppearance? ItemModifiedAppearance { get; set; }
        //public ItemSearchName? ItemSearchName { get; set; } // Will be populated automatically
        public ItemSparse? ItemSparse { get; set; }
        public List<ItemDisplayInfoMaterialRes>? ItemDisplayInfoMaterialRes { get; set; }
        public List<EffectGroup> EffectGroups { get; set; } = new();

        public class EffectGroup
        {
            public ItemEffect ItemEffect { get; set; } = new();
            //public ItemXItemEffect ItemXItemEffect { get; set; } = new(); // Will be populated automatically
        }
        // TODO: ItemDisplayInfoModelMatRes
    }
}
