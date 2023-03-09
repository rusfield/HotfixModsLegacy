using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Core.Models;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService
    {
        public async Task<List<ItemModifiedAppearance>> GetAvailableItemModifiedAppearancesAsync(int itemId)
        {
            try
            {
                return await GetAsync<ItemModifiedAppearance>(false, new DbParameter(nameof(ItemModifiedAppearance.ItemID), itemId));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return new();
        }

        public string ItemAppearanceModifierIdToString(int itemAppearanceModifierId)
        {
            return itemAppearanceModifierId switch
            {
                0 => "Normal",
                1 => "Heroic",
                3 => "Mythic",
                4 => "LFR",
                _ => $"Modifier ID {itemAppearanceModifierId}"
            };
        }

        async Task SetIdAndVerifiedBuild(ItemDto dto, List<ItemXItemEffect> itemXItemEffects, ItemSearchName? itemSearchName, List<ItemDisplayInfoModelMatRes> itemDisplayInfoModelMatRes)
        {
            // Step 1: Init IDs of single entities
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            var itemId = await GetIdByConditionsAsync<Item>(dto.Item.ID, dto.IsUpdate);
            var itemModifiedAppearanceId = await GetIdByConditionsAsync<ItemModifiedAppearance>(dto.ItemModifiedAppearance?.ID, dto.IsUpdate);
            var itemAppearanceId = await GetIdByConditionsAsync<ItemAppearance>(dto.ItemAppearance?.ID, dto.IsUpdate);
            var itemDisplayInfoId = await GetIdByConditionsAsync<ItemDisplayInfo>(dto.ItemDisplayInfo?.ID, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextItemDisplayInfoMaterialResId = await GetNextIdAsync<ItemDisplayInfoMaterialRes>();
            var nextItemXItemEffectId = await GetNextIdAsync<ItemXItemEffect>();
            var nextItemEffectId = await GetNextIdAsync<ItemEffect>();
            var nextItemDisplayInfoModelMatResId = await GetNextIdAsync<ItemDisplayInfoModelMatRes>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = itemId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.Item.ID = itemId;
            dto.Item.VerifiedBuild = VerifiedBuild;

            if (itemSearchName != null)
            {
                itemSearchName.ID = itemId;
                itemSearchName.VerifiedBuild = VerifiedBuild;
            }

            if (dto.ItemSparse != null)
            {
                dto.ItemSparse.ID = itemId;
                dto.ItemSparse.VerifiedBuild = VerifiedBuild;
            }

            if (dto.ItemModifiedAppearance != null)
            {
                dto.ItemModifiedAppearance.ID = itemModifiedAppearanceId;
                dto.ItemModifiedAppearance.ItemID = (int)itemId;
                dto.ItemModifiedAppearance.ItemAppearanceID = (int)itemAppearanceId;
                dto.ItemModifiedAppearance.VerifiedBuild = VerifiedBuild;

                if (dto.ItemAppearance != null)
                {
                    dto.ItemAppearance.ID = itemAppearanceId;
                    dto.ItemAppearance.ItemDisplayInfoID = (int)itemDisplayInfoId;
                    dto.ItemAppearance.VerifiedBuild = VerifiedBuild;

                    if (dto.ItemDisplayInfo != null)
                    {
                        dto.ItemDisplayInfo.ID = itemDisplayInfoId;
                        dto.ItemDisplayInfo.VerifiedBuild = VerifiedBuild;

                        dto.ItemDisplayInfoMaterialRes?.ForEach(x =>
                        {
                            x.ItemDisplayInfoID = (int)itemDisplayInfoId;
                            x.VerifiedBuild = VerifiedBuild;
                            if (x.ID == 0 || !dto.IsUpdate)
                                x.ID = nextItemDisplayInfoMaterialResId++;
                        });

                        if (dto.ItemDisplayInfo.ModelMaterialResourcesID0 != 0)
                        {
                            itemDisplayInfoModelMatRes.Add(new()
                            {
                                ID = nextItemDisplayInfoModelMatResId++,
                                ItemDisplayInfoID = (int)itemDisplayInfoId,
                                ModelIndex = 0,
                                MaterialResourcesID = dto.ItemDisplayInfo.ModelMaterialResourcesID0,
                                TextureType = 2, // TODO: Check
                                VerifiedBuild = VerifiedBuild
                            });
                        }

                        if (dto.ItemDisplayInfo.ModelMaterialResourcesID1 != 0)
                        {
                            itemDisplayInfoModelMatRes.Add(new()
                            {
                                ID = nextItemDisplayInfoModelMatResId++,
                                ItemDisplayInfoID = (int)itemDisplayInfoId,
                                ModelIndex = 1,
                                MaterialResourcesID = dto.ItemDisplayInfo.ModelMaterialResourcesID1,
                                TextureType = 2, // TODO: Check
                                VerifiedBuild = VerifiedBuild
                            });
                        }
                    }
                }
            }

            dto.EffectGroups.ForEach(eg =>
            {
                eg.ItemEffect.ID = nextItemEffectId++;
                eg.ItemEffect.VerifiedBuild = VerifiedBuild;
            });


            // Step 4: Prepare entities not in DTO
            if (dto.IsUpdate)
            {
                itemXItemEffects.RemoveAll(i => !dto.EffectGroups.Any(eg => eg.ItemEffect.ID == i.ItemEffectID));
            }
            else
            {
                itemXItemEffects.Clear();
            }

            dto.EffectGroups.ForEach(eg =>
            {
                var itemXItemEffect = itemXItemEffects.FirstOrDefault(i => i.ItemEffectID == eg.ItemEffect.ID);
                if (null == itemXItemEffect)
                {
                    itemXItemEffects.Add(new()
                    {
                        ID = nextItemXItemEffectId++,
                        ItemEffectID = (int)eg.ItemEffect.ID,
                        ItemID = (int)itemId,
                        VerifiedBuild = VerifiedBuild
                    });
                }
                else
                {
                    itemXItemEffect.ItemID = (int)itemId;
                    itemXItemEffect.VerifiedBuild = VerifiedBuild;
                }
            });

        }
    }
}
