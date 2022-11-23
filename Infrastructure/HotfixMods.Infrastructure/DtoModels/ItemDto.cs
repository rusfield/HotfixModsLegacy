using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class ItemDto : BaseDto
    {
        public ItemDto(): base(nameof(Item)) { }


        public Item Item { get; set; } = new();
        public ItemAppearance ItemAppearance { get; set; } = new();
        public ItemDisplayInfo ItemDisplayInfo { get; set; } = new();
        public ItemModifiedAppearance ItemModifiedAppearance { get; set; } = new();
        public ItemSearchName ItemSearchName { get; set; } = new();
        public ItemSparse ItemSparse { get; set; } = new();
        public List<ItemDisplayInfoMaterialRes> ItemDisplayInfoMaterialRes { get; set; } = new();
        public List<ItemEffect> ItemEffects { get; set; } = new();
        public List<ItemXItemEffect> ItemXIteItems { get; set; } = new();
    }
}
