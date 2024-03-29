﻿using HotfixMods.Core.Models;
using HotfixMods.Core.Providers;
using HotfixMods.Core.Enums;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels.Items;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService : Service
    {
        public ItemService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider) { }

        public async Task<List<DashboardModel>> GetDashboardAsync()
        {
            var items = await _mySql.GetAsync<ItemSparse>(c => c.VerifiedBuild == VerifiedBuild);
            var result = new List<DashboardModel>();
            foreach (var item in items)
            {
                result.Add(new DashboardModel()
                {
                    Id = item.Id,
                    Name = item.Display,
                    AvatarUrl = "TODO",
                    Comment = "TODO"
                });
            }

            // Newest on top
            return result.OrderByDescending(i => i.Id).ToList();
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteFromHotfixesAsync(id);
            await DeleteFromCharactersAsync(id);
        }

        public async Task SaveAsync(ItemDto dto)
        {
            var hotfixId = await GetNextHotfixIdAsync();
            dto.InitHotfixes(hotfixId, VerifiedBuild);

            if (dto.Effects.Count > 10)
                throw new Exception("Effects can not exceed 10.");

            if (dto.IsUpdate)
            {
                var itemXItemEffects = await _mySql.GetAsync<ItemXItemEffect>(i => i.ItemId == dto.Id);
                if (itemXItemEffects.Count() > 0)
                {
                    foreach (var itemXItemEffect in itemXItemEffects)
                    {
                        var itemEffect = await _mySql.GetSingleAsync<ItemEffect>(i => i.Id == itemXItemEffect.ItemEffectId);
                        if (itemEffect != null)
                            await _mySql.DeleteAsync(itemEffect);
                    }
                    await _mySql.DeleteAsync(itemXItemEffects.ToArray());
                }
            }

            await _mySql.AddOrUpdateAsync(BuildHotfixModsData(dto));
            await _mySql.AddOrUpdateAsync(BuildItem(dto));
            await _mySql.AddOrUpdateAsync(BuildItemAppearance(dto));
            await _mySql.AddOrUpdateAsync(BuildItemDisplayInfo(dto));
            await _mySql.AddOrUpdateAsync(BuildItemModifiedAppearance(dto));
            await _mySql.AddOrUpdateAsync(BuildItemSearchName(dto));
            await _mySql.AddOrUpdateAsync(BuildItemSparse(dto));
            await _mySql.AddOrUpdateAsync(BuildItemDisplayInfoMaterialRes(dto));
            await _mySql.AddOrUpdateAsync(BuildItemXItemEffects(dto).ToArray());
            await _mySql.AddOrUpdateAsync(BuildItemEffects(dto).ToArray());

            await AddHotfixes(dto.GetHotfixes());
        }

        public async Task<ItemDto> GetNewAsync(Action<string, string, int>? progressCallback = null)
        {
            return new ItemDto()
            {
                Id = await GetNextIdAsync(),
                DisplayType = DisplayTypes.CHEST,
                InventoryType = InventoryTypes.CHEST_BODY,
                ItemClass = ItemClasses.ARMOR,
                ItemSubClass = 1,
                Effects = new()
            };
        }

        public async Task<List<ItemDto>> GetByIdAsync(int itemId, Action<string, string, int>? progressCallback = null)
        {
            if (progressCallback == null)
                progressCallback = ConsoleProgressCallback;

            var result = new List<ItemDto>();

            progressCallback("Item", "Loading from ItemSparse", 10);
            var itemSparse = await _mySql.GetSingleAsync<ItemSparse>(i => i.Id == itemId) ?? await _db2.GetSingleAsync<ItemSparse>(i => i.Id == itemId);
            if (null == itemSparse)
            {
                progressCallback("Item", "ItemSparse not found", 100);
                return result;
            }

            progressCallback("Item", "Loading from Item", 25);
            var item = await _mySql.GetSingleAsync<Item>(i => i.Id == itemId) ?? await _db2.GetSingleAsync<Item>(i => i.Id == itemId);
            if (null == item)
            {
                progressCallback("Item", "Item not found", 100);
                return result;
            }

            progressCallback("Item", "Setting Id", 40);
            var id = itemId;
            var isUpdate = false;
            if (id >= IdRangeFrom && id < IdRangeTo)
            {
                isUpdate = true;
            }
            else
            {
                id = await GetNextIdAsync();
            }

            progressCallback("Item", "Loading from ItemModifiedAppearance", 50);

            var itemModifiedAppearances = (await _mySql.GetAsync<ItemModifiedAppearance>(i => i.Id == itemId)).ToList();
            if (!itemModifiedAppearances.Any())
                itemModifiedAppearances = (await _db2.GetAsync<ItemModifiedAppearance>(i => i.ItemId == itemId)).ToList();
            if (!itemModifiedAppearances.Any())
            {
                progressCallback("Item", "Item has no appearance", 70);
                progressCallback($"Item", "Loading effects", 80);
                var itemXEffects = await _mySql.GetAsync<ItemXItemEffect>(i => i.ItemId == itemId);
                if (itemXEffects.Count() == 0)
                    itemXEffects = await _db2.GetAsync<ItemXItemEffect>(i => i.ItemId == itemId);

                var effects = new List<ItemEffectDto>();
                foreach (var itemXEffect in itemXEffects)
                {
                    var itemEffect = await _mySql.GetSingleAsync<ItemEffect>(i => i.Id == itemXEffect.ItemEffectId) ?? await _db2.GetSingleAsync<ItemEffect>(i => i.Id == itemXEffect.ItemEffectId);
                    if (itemEffect != null)
                    {
                        effects.Add(new ItemEffectDto()
                        {
                            TriggerType = itemEffect.TriggerType,
                            CategoryCoolDownMSec = itemEffect.CategoryCoolDownMSec,
                            Charges = itemEffect.Charges,
                            CoolDownMSec = itemEffect.CoolDownMSec,
                            SpellCategoryId = itemEffect.SpellCategoryId,
                            SpellId = itemEffect.SpellId
                        });
                    }
                }

                
                result.Add(new ItemDto()
                {
                    Id = id,
                    ItemDisplayInfoId = id,
                    Bonding = itemSparse.Bonding,
                    Flags0 = itemSparse.Flags0,
                    Flags1 = itemSparse.Flags1,
                    Flags2 = itemSparse.Flags2,
                    Flags3 = itemSparse.Flags3,
                    AllowableClasses = itemSparse.AllowableClass,
                    AllowableRaces = itemSparse.AllowableRace,
                    RequiredLevel = itemSparse.RequiredLevel,
                    ItemLevel = itemSparse.ItemLevel,
                    OverallQuality = itemSparse.OverallQualityId,
                    Material = itemSparse.Material,
                    ItemClass = item.ClassId,
                    Name = itemSparse.Display,
                    ItemSubClass = item.SubClassId,
                    IconId = item.IconFileDataId,
                    Description = itemSparse.Description,
                    InventoryType = item.InventoryType,
                    ItemGroupSoundsId = item.ItemGroupSoundsId,
                    StatModifierBonusStat0 = itemSparse.StatModifierBonusStat0,
                    StatModifierBonusStat1 = itemSparse.StatModifierBonusStat1,
                    StatModifierBonusStat2 = itemSparse.StatModifierBonusStat2,
                    StatModifierBonusStat3 = itemSparse.StatModifierBonusStat3,
                    StatModifierBonusStat4 = itemSparse.StatModifierBonusStat4,
                    StatModifierBonusStat5 = itemSparse.StatModifierBonusStat5,
                    StatModifierBonusStat6 = itemSparse.StatModifierBonusStat6,
                    StatModifierBonusStat7 = itemSparse.StatModifierBonusStat7,
                    StatModifierBonusStat8 = itemSparse.StatModifierBonusStat8,
                    StatModifierBonusStat9 = itemSparse.StatModifierBonusStat9,
                    StatPercentEditor0 = itemSparse.StatPercentEditor0,
                    StatPercentEditor1 = itemSparse.StatPercentEditor1,
                    StatPercentEditor2 = itemSparse.StatPercentEditor2,
                    StatPercentEditor3 = itemSparse.StatPercentEditor3,
                    StatPercentEditor4 = itemSparse.StatPercentEditor4,
                    StatPercentEditor5 = itemSparse.StatPercentEditor5,
                    StatPercentEditor6 = itemSparse.StatPercentEditor6,
                    StatPercentEditor7 = itemSparse.StatPercentEditor7,
                    StatPercentEditor8 = itemSparse.StatPercentEditor8,
                    StatPercentEditor9 = itemSparse.StatPercentEditor9,
                    Effects = effects,
                    SocketMatchEnchantmentId = itemSparse.SocketMatchEnchantmentId,
                    SocketType0 = itemSparse.SocketType0,
                    SocketType1 = itemSparse.SocketType1,
                    SocketType2 = itemSparse.SocketType2,
                    SheatheType = itemSparse.SheatheType,

                    IsUpdate = isUpdate,
                    SearchResultName = itemSparse.Display

                });

                progressCallback($"Result", $"Returning {result.Count} {(result.Count == 1 ? "item" : "items")}", 100);
                return result;
            }

            int index = 0;
            foreach (var itemModifiedAppearance in itemModifiedAppearances)
            {
                index++;
                int progress = 50 + (int)(45.0 / itemModifiedAppearances.Count * index);
                progressCallback($"Appearance {index} of {itemModifiedAppearances.Count}", "Loading from ItemAppearance", progress);
                var itemAppearance = await _mySql.GetSingleAsync<ItemAppearance>(i => i.Id == itemModifiedAppearance.ItemAppearanceId) ?? await _db2.GetSingleAsync<ItemAppearance>(i => i.Id == itemModifiedAppearance.ItemAppearanceId);
                if (null == itemAppearance)
                {
                    progressCallback($"Appearance {index} of {itemModifiedAppearances.Count}", "ItemAppearance not found", progress);
                    continue;
                }

                progressCallback($"Appearance {index} of {itemModifiedAppearances.Count}", "Loading from ItemDisplayInfo", progress);
                var itemDisplayInfo = await _mySql.GetSingleAsync<ItemDisplayInfo>(i => i.Id == itemModifiedAppearance.ItemAppearanceId) ?? await _db2.GetSingleAsync<ItemDisplayInfo>(i => i.Id == itemAppearance.ItemDisplayInfoId);
                if (null == itemDisplayInfo)
                {
                    progressCallback($"Appearance {index} of {itemModifiedAppearances.Count}", "ItemDisplayInfo not found", progress);
                    continue;
                }

                progressCallback($"Appearance {index} of {itemModifiedAppearances.Count}", "Loading from ItemDisplayInfoMaterialRes", progress);
                var itemDisplayInfoMaterialResources = await _mySql.GetAsync<ItemDisplayInfoMaterialRes>(i => i.ItemDisplayInfoId == itemDisplayInfo.Id);
                if (!itemDisplayInfoMaterialResources.Any())
                    itemDisplayInfoMaterialResources = await _db2.GetAsync<ItemDisplayInfoMaterialRes>(i => i.ItemDisplayInfoId == itemDisplayInfo.Id);

                // Check for raid drop or artifact
                string appearanceName = "None";
                if (itemModifiedAppearances.Count() == 4)
                {
                    appearanceName = (itemModifiedAppearance.OrderIndex) switch
                    {
                        0 => "LFR",
                        1 => "Normal",
                        2 => "Heroic",
                        3 => "Mythic",
                        _ => "Unknown"
                    };
                }
                else if (itemModifiedAppearances.Count() == 24)
                {
                    appearanceName = (itemModifiedAppearance.OrderIndex) switch
                    {
                        0 => "Artifact (Default)",
                        1 => "Pillar of Creation",
                        2 => "Light's Charge",
                        3 => "First Major Order Campaign",
                        4 => "Forged to Battle",
                        5 => "Power Realized",
                        6 => "Part of History",
                        7 => "This Side Up",
                        8 => "Improving History",
                        9 => "Unleashed Monstrosities",
                        10 => "Keystone Master",
                        11 => "Glory of the Legion Hero",
                        12 => "The Prestige",
                        13 => "Crest of Heroism",
                        14 => "Crest of Carnage",
                        15 => "Crest of Devastation",
                        16 => "Mage Tower (Default)",
                        17 => "Mage Tower (Kil'jaeden)",
                        18 => "Mage Tower (10 RBGs)",
                        19 => "Mage Tower (Legion dungeons)",
                        20 => "Hidden (Default)",
                        21 => "Hidden (30 dungeons)",
                        22 => "Hidden (200 world quests)",
                        23 => "Hidden (1000 players)",
                        _ => "Unknown"
                    };
                }

                progressCallback($"Appearance {index} of {itemModifiedAppearances.Count}", "Loading effects", progress);
                var itemXEffects = await _mySql.GetAsync<ItemXItemEffect>(i => i.ItemId == itemModifiedAppearance.ItemId);
                if (itemXEffects.Count() == 0)
                    itemXEffects = await _db2.GetAsync<ItemXItemEffect>(i => i.ItemId == itemModifiedAppearance.ItemId);

                var effects = new List<ItemEffectDto>();
                foreach (var itemXEffect in itemXEffects)
                {
                    var itemEffect = await _mySql.GetSingleAsync<ItemEffect>(i => i.Id == itemXEffect.ItemEffectId) ?? await _db2.GetSingleAsync<ItemEffect>(i => i.Id == itemXEffect.ItemEffectId);
                    if (itemEffect != null)
                    {
                        effects.Add(new ItemEffectDto()
                        {
                            TriggerType = itemEffect.TriggerType,
                            CategoryCoolDownMSec = itemEffect.CategoryCoolDownMSec,
                            Charges = itemEffect.Charges,
                            CoolDownMSec = itemEffect.CoolDownMSec,
                            SpellCategoryId = itemEffect.SpellCategoryId,
                            SpellId = itemEffect.SpellId
                        });
                    }
                }

                var itemDto = new ItemDto()
                {
                    Id = id,
                    ItemDisplayInfoId = id,
                    GeosetGroup0 = itemDisplayInfo.GeosetGroup0,
                    GeosetGroup1 = itemDisplayInfo.GeosetGroup1,
                    GeosetGroup2 = itemDisplayInfo.GeosetGroup2,
                    GeosetGroup3 = itemDisplayInfo.GeosetGroup3,
                    GeosetGroup4 = itemDisplayInfo.GeosetGroup4,
                    GeosetGroup5 = itemDisplayInfo.GeosetGroup5,
                    GeosetGroupAttachment0 = itemDisplayInfo.AttachmentGeosetGroup0,
                    GeosetGroupAttachment1 = itemDisplayInfo.AttachmentGeosetGroup1,
                    GeosetGroupAttachment2 = itemDisplayInfo.AttachmentGeosetGroup2,
                    GeosetGroupAttachment3 = itemDisplayInfo.AttachmentGeosetGroup3,
                    GeosetGroupAttachment4 = itemDisplayInfo.AttachmentGeosetGroup4,
                    GeosetGroupAttachment5 = itemDisplayInfo.AttachmentGeosetGroup5,
                    HelmetGeosetVis0 = itemDisplayInfo.HelmetGeosetVis0,
                    HelmetGeosetVis1 = itemDisplayInfo.HelmetGeosetVis1,
                    ModelMaterialResourceId0 = itemDisplayInfo.ModelMaterialResourcesId0,
                    ModelMaterialResourceId1 = itemDisplayInfo.ModelMaterialResourcesId1,
                    ModelResourceId0 = itemDisplayInfo.ModelResourcesId0,
                    ModelResourceId1 = itemDisplayInfo.ModelResourcesId1,
                    ModelType0 = itemDisplayInfo.ModelType0,
                    ModelType1 = itemDisplayInfo.ModelType1,
                    Bonding = itemSparse.Bonding,
                    Flags0 = itemSparse.Flags0,
                    Flags1 = itemSparse.Flags1,
                    Flags2 = itemSparse.Flags2,
                    Flags3 = itemSparse.Flags3,
                    AllowableClasses = itemSparse.AllowableClass,
                    AllowableRaces = itemSparse.AllowableRace,
                    RequiredLevel = itemSparse.RequiredLevel,
                    ItemLevel = itemSparse.ItemLevel,
                    OverallQuality = itemSparse.OverallQualityId,
                    Material = itemSparse.Material,
                    ItemClass = item.ClassId,
                    Name = itemSparse.Display,
                    ItemSubClass = item.SubClassId,
                    ItemDisplayInfoFlags = itemDisplayInfo.Flags,
                    IconId = itemAppearance.DefaultIconFileDataId,
                    Description = itemSparse.Description,
                    InventoryType = item.InventoryType,
                    DisplayType = itemAppearance.DisplayType,
                    ItemGroupSoundsId = item.ItemGroupSoundsId,
                    StatModifierBonusStat0 = itemSparse.StatModifierBonusStat0,
                    StatModifierBonusStat1 = itemSparse.StatModifierBonusStat1,
                    StatModifierBonusStat2 = itemSparse.StatModifierBonusStat2,
                    StatModifierBonusStat3 = itemSparse.StatModifierBonusStat3,
                    StatModifierBonusStat4 = itemSparse.StatModifierBonusStat4,
                    StatModifierBonusStat5 = itemSparse.StatModifierBonusStat5,
                    StatModifierBonusStat6 = itemSparse.StatModifierBonusStat6,
                    StatModifierBonusStat7 = itemSparse.StatModifierBonusStat7,
                    StatModifierBonusStat8 = itemSparse.StatModifierBonusStat8,
                    StatModifierBonusStat9 = itemSparse.StatModifierBonusStat9,
                    StatPercentEditor0 = itemSparse.StatPercentEditor0,
                    StatPercentEditor1 = itemSparse.StatPercentEditor1,
                    StatPercentEditor2 = itemSparse.StatPercentEditor2,
                    StatPercentEditor3 = itemSparse.StatPercentEditor3,
                    StatPercentEditor4 = itemSparse.StatPercentEditor4,
                    StatPercentEditor5 = itemSparse.StatPercentEditor5,
                    StatPercentEditor6 = itemSparse.StatPercentEditor6,
                    StatPercentEditor7 = itemSparse.StatPercentEditor7,
                    StatPercentEditor8 = itemSparse.StatPercentEditor8,
                    StatPercentEditor9 = itemSparse.StatPercentEditor9,
                    Effects = effects,
                    SocketMatchEnchantmentId = itemSparse.SocketMatchEnchantmentId,
                    SocketType0 = itemSparse.SocketType0,
                    SocketType1 = itemSparse.SocketType1,
                    SocketType2 = itemSparse.SocketType2,
                    SheatheType = itemSparse.SheatheType,

                    IsUpdate = isUpdate,
                    SearchResultName = appearanceName
                };

                foreach (var materialRes in itemDisplayInfoMaterialResources)
                {
                    switch (materialRes.ComponentSection)
                    {
                        case ComponentSections.ARM_UPPER:
                            itemDto.ComponentArmUpper = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.ARM_LOWER:
                            itemDto.ComponentArmLower = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.HAND:
                            itemDto.ComponentHand = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.TORSO_UPPER:
                            itemDto.ComponentTorsoUpper = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.TORSO_LOWER:
                            itemDto.ComponentTorsoLower = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.LEG_UPPER:
                            itemDto.ComponentLegUpper = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.LEG_LOWER:
                            itemDto.ComponentLegLower = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.FOOT:
                            itemDto.ComponentFoot = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.ACCESSORY:
                            itemDto.ComponentAccessory = materialRes.MaterialResourcesId;
                            break;
                        default:
                            break;
                    }
                }
                result.Add(itemDto);
            }
            progressCallback($"Result", $"Returning {result.Count} {(result.Count == 1 ? "item" : "items")}", 100);
            return result;
        }



        async Task DeleteFromCharactersAsync(int id)
        {
            var itemInstances = await _mySql.GetAsync<ItemInstance>(i => i.ItemEntry == id);
            foreach (var itemInstance in itemInstances)
            {
                var characterInventory = await _mySql.GetSingleAsync<CharacterInventory>(c => c.Item == id);
                if (characterInventory != null)
                    await _mySql.DeleteAsync(characterInventory);
                await _mySql.DeleteAsync(itemInstance);
            }
        }

        async Task DeleteFromHotfixesAsync(int id)
        {
            var item = await _mySql.GetSingleAsync<Item>(c => c.Id == id);
            var itemSparse = await _mySql.GetSingleAsync<ItemSparse>(c => c.Id == id);
            var itemSearchName = await _mySql.GetSingleAsync<ItemSearchName>(c => c.Id == id);
            var itemAppearance = await _mySql.GetSingleAsync<ItemAppearance>(c => c.Id == id);
            var itemModifiedAppearance = await _mySql.GetSingleAsync<ItemModifiedAppearance>(c => c.Id == id);
            var itemDisplayInfo = await _mySql.GetSingleAsync<ItemDisplayInfo>(c => c.Id == id);
            var itemDisplayInfoMaterialResources = await _mySql.GetAsync<ItemDisplayInfoMaterialRes>(c => c.ItemDisplayInfoId == id);
            var itemXItemEffects = await _mySql.GetAsync<ItemXItemEffect>(c => c.ItemId == id);
            var hotfixModsData = await _mySql.GetSingleAsync<HotfixModsData>(h => h.Id == id && h.VerifiedBuild == VerifiedBuild);
            var itemEffects = new List<ItemEffect>();

            foreach (var itemXItemEffect in itemXItemEffects)
            {
                var itemEffect = await _mySql.GetSingleAsync<ItemEffect>(i => i.Id == itemXItemEffect.ItemEffectId);
                if(null != itemEffect)
                    itemEffects.Add(itemEffect);
            }
                
            if (null != item)
                await _mySql.DeleteAsync(item);

            if (null != itemSparse)
                await _mySql.DeleteAsync(itemSparse);

            if (null != itemSearchName)
                await _mySql.DeleteAsync(itemSearchName);

            if (null != itemAppearance)
                await _mySql.DeleteAsync(itemAppearance);

            if (null != itemModifiedAppearance)
                await _mySql.DeleteAsync(itemModifiedAppearance);

            if (null != itemDisplayInfo)
                await _mySql.DeleteAsync(itemDisplayInfo);

            if (itemDisplayInfoMaterialResources.Any())
                await _mySql.DeleteAsync(itemDisplayInfoMaterialResources.ToArray());

            if (itemEffects.Any())
                await _mySql.DeleteAsync(itemEffects.ToArray());

            if (itemXItemEffects.Any())
                await _mySql.DeleteAsync(itemXItemEffects.ToArray());

            var hotfixData = await _mySql.GetAsync<HotfixData>(h => h.UniqueId == id);
            if (hotfixData != null && hotfixData.Count() > 0)
            {
                foreach (var hotfix in hotfixData)
                {
                    hotfix.Status = HotfixStatuses.INVALID;
                }
                await _mySql.AddOrUpdateAsync(hotfixData.ToArray());
            }

            if (null != hotfixModsData)
                await _mySql.DeleteAsync(hotfixModsData);
        }
    }
}
