using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService
    {
        async Task SetIdAndVerifiedBuild(ItemDto dto, List<ItemXItemEffect> itemXItemEffects, ItemSearchName? itemSearchName)
        {
            // Step 1: Init IDs of single entities
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.Id, dto.IsUpdate);
            var itemId = await GetIdByConditionsAsync<Item>(dto.Item.Id, dto.IsUpdate);
            var itemModifiedAppearanceId = await GetIdByConditionsAsync<ItemModifiedAppearance>(dto.ItemModifiedAppearance?.Id, dto.IsUpdate);
            var itemAppearanceId = await GetIdByConditionsAsync<ItemAppearance>(dto.ItemAppearance?.Id, dto.IsUpdate);
            var itemDisplayInfoId = await GetIdByConditionsAsync<ItemDisplayInfo>(dto.ItemDisplayInfo?.Id, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextItemDisplayInfoMaterialResId = await GetNextIdAsync<ItemDisplayInfoMaterialRes>();
            var nextItemXItemEffectId = await GetNextIdAsync<ItemXItemEffect>();
            var nextItemEffectId = await GetNextIdAsync<ItemEffect>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.Id = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordId = itemId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.Item.Id = itemId;
            dto.Item.VerifiedBuild = VerifiedBuild;

            if(itemSearchName != null)
            {
                itemSearchName.Id = itemId;
                itemSearchName.VerifiedBuild = VerifiedBuild;
            }

            if (dto.ItemSparse != null)
            {
                dto.ItemSparse.Id = itemId;
                dto.ItemSparse.VerifiedBuild = VerifiedBuild;
            }

            if (dto.ItemModifiedAppearance != null)
            {
                dto.ItemModifiedAppearance.Id = itemModifiedAppearanceId;
                dto.ItemModifiedAppearance.ItemId = itemId;
                dto.ItemModifiedAppearance.ItemAppearanceId= itemAppearanceId;
                dto.ItemModifiedAppearance.VerifiedBuild = VerifiedBuild;

                if (dto.ItemAppearance != null)
                {
                    dto.ItemAppearance.Id = itemAppearanceId;
                    dto.ItemAppearance.ItemDisplayInfoId = itemDisplayInfoId;
                    dto.ItemAppearance.VerifiedBuild = VerifiedBuild;

                    if(dto.ItemDisplayInfo != null)
                    {
                        dto.ItemDisplayInfo.Id = itemDisplayInfoId;
                        dto.ItemDisplayInfo.VerifiedBuild = VerifiedBuild;

                        dto.ItemDisplayInfoMaterialRes?.ForEach(x =>
                        {
                            x.ItemDisplayInfoId= itemDisplayInfoId;
                            x.VerifiedBuild = VerifiedBuild;
                            if (x.Id == 0 || !dto.IsUpdate)
                                x.Id = nextItemDisplayInfoMaterialResId++;
                        });
                    }
                }
            }

            dto.EffectGroups.ForEach(eg =>
            {
                if (eg.ItemEffect.Id == 0 || !dto.IsUpdate)
                    eg.ItemEffect.Id = nextItemEffectId++;

                eg.ItemEffect.VerifiedBuild = VerifiedBuild;
            });


            // Step 4: Prepare entities not in DTO
            if (dto.IsUpdate)
            {
                itemXItemEffects.RemoveAll(i => !dto.EffectGroups.Any(eg => eg.ItemEffect.Id == i.ItemEffectId));
            }
            else
            {
                itemXItemEffects.Clear();
            }

            dto.EffectGroups.ForEach(eg =>
            {
                var itemXItemEffect = itemXItemEffects.FirstOrDefault(i => i.ItemEffectId == eg.ItemEffect.Id);
                if(null == itemXItemEffect)
                {
                    itemXItemEffects.Add(new()
                    {
                        Id = nextItemXItemEffectId++,
                        ItemEffectId = eg.ItemEffect.Id,
                        ItemId = itemId,
                        VerifiedBuild = VerifiedBuild
                    });
                }
                else
                {
                    itemXItemEffect.ItemId = itemId;
                    itemXItemEffect.VerifiedBuild = VerifiedBuild;
                }
            });

        }
    }
}
