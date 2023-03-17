using HotfixMods.Core.Enums.TrinityCore;
using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.AggregateModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService : ServiceBase
    {
        public CreatureService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IServerEnumProvider serverEnumProvider, IExceptionHandler exceptionHandler, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, serverEnumProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.CreatureSettings.FromId;
            ToId = appConfig.CreatureSettings.ToId;
            VerifiedBuild = appConfig.CreatureSettings.VerifiedBuild;
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            try
            {

                var dtos = await GetAsync<HotfixModsEntity>(DefaultCallback, DefaultProgress, true, false, new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
                var results = new List<DashboardModel>();
                foreach (var dto in dtos)
                {
                    results.Add(new()
                    {
                        ID = (int)dto.RecordID,
                        Name = dto.Name,
                        AvatarUrl = null
                    });
                }
                return results.OrderByDescending(d => d.ID).ToList();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return new();
        }

        public async Task<CreatureDto?> GetByCharacterNameAsync(string characterName, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(11);

            try
            {


                var character = await GetSingleAsync<Characters>(callback, progress, new DbParameter(nameof(Characters.Name), characterName)); // TODO: Check if Deleted chars must be excluded
                if (character == null)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(Characters)} not found", 100);
                    return null;
                }
                var result = new CreatureDto()
                {
                    CreatureTemplate = new()
                    {
                        Name = character.Name,
                        Minlevel = character.Level,
                        Maxlevel = character.Level,
                        Type = 7 // Humanoid
                    },
                    HotfixModsEntity = new()
                    {
                        Name = character.Name,
                    },
                    CreatureDisplayInfoOption = new(),
                    CreatureDisplayInfo = new()
                    {
                        Gender = (sbyte)character.Gender,
                        ModelID = 1
                    },
                    CreatureTemplateModel = new(),
                    CreatureEquipTemplate = new(),
                    CreatureDisplayInfoExtra = new()
                    {
                        DisplayRaceID = (sbyte)character.Race,
                        DisplaySexID = (sbyte)character.Gender,
                        DisplayClassID = (sbyte)character.Class
                    },
                    CreatureModelInfo = new(),
                    NpcModelItemSlotDisplayInfo = new(),

                    IsUpdate = false
                };

                var characterCustomization = await GetAsync<CharacterCustomizations>(callback, progress, new DbParameter(nameof(CharacterCustomizations.Guid), character.Guid));
                foreach (var customization in characterCustomization)
                {
                    result.CreatureDisplayInfoOption.Add(new()
                    {
                        ChrCustomizationChoiceID = (int)customization.ChrCustomizationChoiceID,
                        ChrCustomizationOptionID = (int)customization.ChrCustomizationOptionID,
                    });
                }
                result.CreatureDisplayInfo.ModelID = GetModelIdByRaceAndGenders(result.CreatureDisplayInfoExtra.DisplayRaceID, result.CreatureDisplayInfoExtra.DisplaySexID, result.CreatureDisplayInfoOption);

                var characterItems = await GetAsync<ItemInstance>(callback, progress, new DbParameter(nameof(ItemInstance.Owner_Guid), character.Guid));
                var characterEquipMap = await GetAsync<CharacterInventory>(callback, progress, new DbParameter(nameof(CharacterInventory.Guid), character.Guid));

                foreach (var equippedItem in characterEquipMap.OrderBy(c => c.Slot))
                {
                    if (!Enum.IsDefined(typeof(CharacterInventorySlot), (int)equippedItem.Slot))
                        continue;

                    var item = characterItems.Where(i => i.Guid == equippedItem.Item).FirstOrDefault();
                    if (item == null)
                        continue;

                    var itemAppearanceId = 0;
                    var itemAppearanceModifierId = 0;
                    var itemId = item.ItemEntry;
                    var itemVisual = 0;
                    var transmogItem = await GetSingleAsync<ItemInstanceTransmog>(new DbParameter(nameof(ItemInstanceTransmog.ItemGuid), equippedItem.Item));

                    // Try get ItemAppearanceId by transmog.
                    // Note: Dont mix up ItemModifiedAppearance.Id with ItemAppearanceModifierId.
                    if (transmogItem != null)
                    {
                        var itemModifiedAppearance = await GetSingleAsync<ItemModifiedAppearance>(new DbParameter(nameof(ItemModifiedAppearance.ID), transmogItem.ItemModifiedAppearanceAllSpecs));
                        if (itemModifiedAppearance != null)
                        {
                            itemAppearanceId = itemModifiedAppearance.ItemAppearanceID;
                            itemAppearanceModifierId = itemModifiedAppearance.ItemAppearanceModifierID;
                            itemId = (uint)itemModifiedAppearance.ItemID;
                        }
                        if (transmogItem.SpellItemEnchantmentAllSpecs > 0 && IsWeaponSlot(equippedItem.Slot))
                        {
                            var spellItemEnchantment = await GetSingleAsync<SpellItemEnchantment>(new DbParameter(nameof(SpellItemEnchantment.ID), transmogItem.SpellItemEnchantmentAllSpecs));
                            if (spellItemEnchantment != null)
                                itemVisual = spellItemEnchantment.ItemVisual;
                        }
                    }

                    // Get ItemAppearanceId the default way, either because transmog does not exist or transmog failed.
                    if (itemAppearanceId == 0)
                    {
                        if (!string.IsNullOrWhiteSpace(item.BonusListIds))
                        {
                            var bonusListIds = item.BonusListIds.Trim().Split(' ').Select(int.Parse).ToList();
                            if (bonusListIds != null && bonusListIds.Any())
                            {
                                var itemBonuses = await GetAsync<ItemBonus>(new DbParameter(nameof(ItemBonus.Type), 7));
                                var itemBonus = itemBonuses.Where(b => bonusListIds.Contains(b.ParentItemBonusListID)).FirstOrDefault();
                                if (itemBonus != null)
                                {
                                    itemAppearanceModifierId = itemBonus.Value0; // TODO: Change to 0 after refactor
                                }
                            }
                        }

                        var itemModifiedAppearance = await GetSingleAsync<ItemModifiedAppearance>(new DbParameter(nameof(ItemModifiedAppearance.ItemID), item.ItemEntry), new DbParameter(nameof(ItemModifiedAppearance.ItemAppearanceModifierID), itemAppearanceModifierId));
                        if (itemModifiedAppearance == null)
                            continue;

                        itemAppearanceId = itemModifiedAppearance.ItemAppearanceID;
                    }

                    if (itemVisual == 0 && IsWeaponSlot(equippedItem.Slot))
                    {
                        foreach (var enchantment in item.Enchantments.Split(' '))
                        {
                            if (int.TryParse(enchantment.Trim(), out var enchantmentId))
                            {
                                if (enchantmentId > 0)
                                {
                                    var spellItemEnchantment = await GetSingleAsync<SpellItemEnchantment>(new DbParameter(nameof(SpellItemEnchantment.ID), enchantmentId));
                                    if (spellItemEnchantment != null && spellItemEnchantment.ItemVisual > 0)
                                    {
                                        itemVisual = spellItemEnchantment.ItemVisual;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    var itemAppearance = await GetSingleAsync<ItemAppearance>(new DbParameter(nameof(ItemAppearance.ID), itemAppearanceId));
                    if (itemAppearance == null)
                        continue;

                    if (equippedItem.Slot == (int)CharacterInventorySlot.MAIN_HAND)
                    {
                        result.CreatureEquipTemplate.ItemId1 = itemId;
                        result.CreatureEquipTemplate.AppearanceModId1 = (ushort)itemAppearanceModifierId;
                        result.CreatureEquipTemplate.ItemVisual1 = (ushort)itemVisual;

                    }
                    else if (equippedItem.Slot == (int)CharacterInventorySlot.OFF_HAND)
                    {
                        result.CreatureEquipTemplate.ItemId2 = itemId;
                        result.CreatureEquipTemplate.AppearanceModId2 = (ushort)itemAppearanceModifierId;
                        result.CreatureEquipTemplate.ItemVisual2 = (ushort)itemVisual;
                    }
                    else if (equippedItem.Slot == (int)CharacterInventorySlot.RANGED)
                    {
                        result.CreatureEquipTemplate.ItemId3 = itemId;
                        result.CreatureEquipTemplate.AppearanceModId3 = (ushort)itemAppearanceModifierId;
                        result.CreatureEquipTemplate.ItemVisual3 = (ushort)itemVisual;
                    }
                    else
                    {
                        result.NpcModelItemSlotDisplayInfo.Add(new()
                        {
                            ItemDisplayInfoID = itemAppearance.ItemDisplayInfoID,
                            ItemSlot = CharacterInventorySlotToNpcModelItemSlot(equippedItem.Slot)
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return null;
        }

        public async Task<CreatureDto?> GetByIdAsync(int id, int idx = 0, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(11);

            try
            {
                var creatureTemplate = await GetSingleAsync<CreatureTemplate>(callback, progress, new DbParameter(nameof(CreatureTemplate.Entry), id));
                if (creatureTemplate == null)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(CreatureTemplate)} not found", 100);
                    return null;
                }

                var result = new CreatureDto()
                {
                    HotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(callback, progress, id),
                    CreatureTemplateAddon = await GetSingleAsync<CreatureTemplateAddon>(callback, progress, new DbParameter(nameof(CreatureTemplateAddon.Entry), id)),
                    CreatureEquipTemplate = await GetSingleAsync<CreatureEquipTemplate>(callback, progress, new DbParameter(nameof(CreatureEquipTemplate.CreatureID), id)),
                    CreatureTemplate = creatureTemplate,
                    IsUpdate = true
                };

                var creatureTemplateModel = await GetSingleAsync<CreatureTemplateModel>(callback, progress, new DbParameter(nameof(CreatureTemplateModel.CreatureID), id), new DbParameter(nameof(CreatureTemplateModel.Idx), idx));
                if (creatureTemplateModel != null)
                {
                    result.CreatureTemplateModel = creatureTemplateModel;
                    result.CreatureModelInfo = await GetSingleAsync<CreatureModelInfo>(callback, progress, new DbParameter(nameof(CreatureModelInfo.DisplayID), (int)result.CreatureTemplateModel.CreatureDisplayID)) ?? new();
                    result.CreatureDisplayInfo = await GetSingleAsync<CreatureDisplayInfo>(callback, progress, new DbParameter(nameof(CreatureDisplayInfo.ID), (int)result.CreatureTemplateModel.CreatureDisplayID)) ?? new();
                }
                else
                {
                    result.CreatureTemplateModel = new();
                    result.CreatureModelInfo = new();
                    result.CreatureDisplayInfo = new();
                }


                result.CreatureDisplayInfoExtra = await GetSingleAsync<CreatureDisplayInfoExtra>(callback, progress, new DbParameter(nameof(CreatureDisplayInfoExtra.ID), result.CreatureDisplayInfo.ExtendedDisplayInfoID));

                if (result.CreatureDisplayInfoExtra != null)
                {
                    result.CreatureDisplayInfoOption = await GetAsync<CreatureDisplayInfoOption>(callback, progress, new DbParameter(nameof(CreatureDisplayInfoOption.CreatureDisplayInfoExtraID), result.CreatureDisplayInfoExtra.ID));
                    result.NpcModelItemSlotDisplayInfo = await GetAsync<NpcModelItemSlotDisplayInfo>(callback, progress, new DbParameter(nameof(NpcModelItemSlotDisplayInfo.NpcModelID), result.CreatureDisplayInfoExtra.ID));
                }

                if(string.IsNullOrWhiteSpace(result.HotfixModsEntity.Name))
                {
                    result.HotfixModsEntity.Name = string.IsNullOrWhiteSpace(result.CreatureTemplate.Name) ? "New Creature" : result.CreatureTemplate.Name;
                }

                callback.Invoke(LoadingHelper.Loading, $"Loading successful", 100);
                return result;

            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return null;
        }

        public async Task<CreatureDto?> GetByCreatureDisplayInfoIdAsync(int creatureDisplayInfoId, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(11);

            try
            {
                var creatureDisplayInfo = await GetSingleAsync<CreatureDisplayInfo>(callback, progress, new DbParameter(nameof(CreatureDisplayInfo.ID), creatureDisplayInfoId));
                if (creatureDisplayInfo == null)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(CreatureDisplayInfo)} not found", 100);
                    return null;
                }

                var result = new CreatureDto()
                {
                    CreatureDisplayInfo = creatureDisplayInfo,
                    CreatureModelInfo = await GetSingleAsync<CreatureModelInfo>(callback, progress, new DbParameter(nameof(CreatureModelInfo.DisplayID), creatureDisplayInfoId)) ?? new(),
                    IsUpdate = false
                };

                var creatureTemplateModel = await GetSingleAsync<CreatureTemplateModel>(callback, progress, new DbParameter(nameof(CreatureTemplateModel.CreatureDisplayID), creatureDisplayInfoId));
                if (creatureTemplateModel != null)
                {
                    var creatureTemplate = await GetSingleAsync<CreatureTemplate>(callback, progress, new DbParameter(nameof(CreatureTemplate.Entry), creatureTemplateModel.CreatureID));
                    result.CreatureTemplateModel = creatureTemplateModel;
                    result.HotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(callback, progress, (int?)creatureTemplate?.Entry ?? 0);
                    if (creatureTemplate != null)
                    {
                        result.CreatureTemplate = creatureTemplate;
                        result.CreatureTemplateAddon = await GetSingleAsync<CreatureTemplateAddon>(callback, progress, new DbParameter(nameof(CreatureTemplateAddon.Entry), creatureTemplate.Entry));
                        result.CreatureEquipTemplate = await GetSingleAsync<CreatureEquipTemplate>(callback, progress, new DbParameter(nameof(CreatureEquipTemplate.CreatureID), creatureTemplate.Entry));
                        result.IsUpdate = true;
                    }
                    else
                    {
                        result.CreatureTemplate = new();
                    }
                }
                else
                {
                    result.CreatureTemplateModel = new();
                    result.CreatureTemplate = new();
                }

                result.CreatureDisplayInfoExtra = await GetSingleAsync<CreatureDisplayInfoExtra>(callback, progress, new DbParameter(nameof(CreatureDisplayInfoExtra.ID), result.CreatureDisplayInfo.ExtendedDisplayInfoID));

                if (result.CreatureDisplayInfoExtra != null)
                {
                    result.CreatureDisplayInfoOption = await GetAsync<CreatureDisplayInfoOption>(callback, progress, new DbParameter(nameof(CreatureDisplayInfoOption.CreatureDisplayInfoExtraID), result.CreatureDisplayInfoExtra.ID));
                    result.NpcModelItemSlotDisplayInfo = await GetAsync<NpcModelItemSlotDisplayInfo>(callback, progress, new DbParameter(nameof(NpcModelItemSlotDisplayInfo.NpcModelID), result.CreatureDisplayInfoExtra.ID));
                }

                if (string.IsNullOrWhiteSpace(result.HotfixModsEntity.Name))
                {
                    result.HotfixModsEntity.Name = string.IsNullOrWhiteSpace(result.CreatureTemplate.Name) ? "New Creature" : result.CreatureTemplate.Name;
                }

                result.IsUpdate = true;
                callback.Invoke(LoadingHelper.Loading, $"Loading successful", 100);
                return result;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return null;
        }

        public async Task<bool> SaveAsync(CreatureDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(14);

            try
            {
                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync((int)dto.CreatureTemplate.Entry);
                }

                callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
                await SetIdAndVerifiedBuild(dto);

                await SaveAsync(callback, progress, dto.HotfixModsEntity);
                await SaveAsync(callback, progress, dto.CreatureTemplate);
                await SaveAsync(callback, progress, dto.CreatureTemplateAddon);
                await SaveAsync(callback, progress, dto.CreatureEquipTemplate);
                await SaveAsync(callback, progress, dto.CreatureTemplateModel);
                await SaveAsync(callback, progress, dto.CreatureDisplayInfo);
                await SaveAsync(callback, progress, dto.CreatureModelInfo);

                if (dto.CreatureDisplayInfoExtra != null)
                {
                    await SaveAsync(callback, progress, dto.CreatureDisplayInfoExtra);
                    await SaveAsync(callback, progress, dto.NpcModelItemSlotDisplayInfo?.ToList() ?? new());
                    await SaveAsync(callback, progress, dto.CreatureDisplayInfoOption?.ToList() ?? new());
                }

                callback.Invoke("Saving", "Saving successful", 100);
                dto.IsUpdate = true;
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(10);

            try
            {
                var dto = await GetByIdAsync(id);
                if (null == dto)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }

                await DeleteAsync(callback, progress, dto.CreatureDisplayInfoExtra);
                await DeleteAsync(callback, progress, dto.CreatureDisplayInfo);
                await DeleteAsync(callback, progress, dto.CreatureModelInfo);
                await DeleteAsync(callback, progress, dto.CreatureTemplateModel);
                await DeleteAsync(callback, progress, dto.CreatureEquipTemplate);
                await DeleteAsync(callback, progress, dto.CreatureTemplateAddon);
                await DeleteAsync(callback, progress, dto.NpcModelItemSlotDisplayInfo ?? new());
                await DeleteAsync(callback, progress, dto.CreatureDisplayInfoOption ?? new());
                await DeleteAsync(callback, progress, dto.CreatureTemplate);
                await DeleteAsync(callback, progress, dto.HotfixModsEntity);

                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return false;
        }
    }
}
