using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class ItemDto : DtoBase
    {
        public ItemDto(): base(nameof(Item)) { }

        public Item Item { get; set; } = new();
        public ItemAppearance ItemAppearance { get; set; } = new();
        public ItemDisplayInfo ItemDisplayInfo { get; set; } = new();
        public ItemModifiedAppearance ItemModifiedAppearance { get; set; } = new();
        public ItemSearchName ItemSearchName { get; set; } = new();
        public ItemSparse ItemSparse { get; set; } = new();
        public List<ItemDisplayInfoMaterialRes> ItemDisplayInfoMaterialRes { get; set; } = new();
        public List<EffectGroup> EffectGroups { get; set; } = new();

        public class EffectGroup
        {
            public ItemEffect ItemEffect { get; set; } = new();
            public ItemXItemEffect ItemXItemEffect { get; set; } = new();
        }
        // TODO: ItemDisplayInfoModelMatRes
    }
}
