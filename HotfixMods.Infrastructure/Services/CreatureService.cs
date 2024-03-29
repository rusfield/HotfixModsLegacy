﻿using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using HotfixMods.Core.Models;
using HotfixMods.Core.Providers;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels;


namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService : Service
    {
        public CreatureService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider) { }

        public async Task SaveAsync(CreatureDto dto)
        {
            var hotfixId = await GetNextHotfixIdAsync();
            dto.InitHotfixes(hotfixId, VerifiedBuild);

            await _mySql.AddOrUpdateAsync(BuildCreatureTemplate(dto));
            await _mySql.AddOrUpdateAsync(BuildCreatureTemplateAddon(dto));
            await _mySql.AddOrUpdateAsync(BuildCreatureTemplateModel(dto));
            await _mySql.AddOrUpdateAsync(BuildCreatureDisplayInfo(dto));
            await _mySql.AddOrUpdateAsync(BuildCreatureDisplayInfoExtra(dto));
            await _mySql.AddOrUpdateAsync(BuildCreatureEquipTemplate(dto));
            await _mySql.AddOrUpdateAsync(BuildCreatureModelInfo(dto));
            await _mySql.AddOrUpdateAsync(BuildCreatureDisplayInfoOption(dto));
            await _mySql.AddOrUpdateAsync(BuildNpcModelItemSlotDisplayInfo(dto));
            await _mySql.AddOrUpdateAsync(BuildHotfixModsData(dto));

            await AddHotfixes(dto.GetHotfixes());

        }

        public async Task DeleteAsync(int id)
        {
            await DeleteFromHotfixesAsync(id);
            await DeleteFromWorldAsync(id);
        }

        public async Task<CreatureDto> GetNewAsync(Action<string, string, int>? progressCallback = null)
        {
            return new CreatureDto()
            {
                Id = await GetNextIdAsync(),
                Auras = new(),
                Gender = Genders.MALE,
                Race = Races.HUMAN,
                Customizations = new()
            };
        }

        public async Task<CreatureDto?> GetByCharacterNameAsync(string characterName, Action<string, string, int>? progressCallback = null)
        {
            if (progressCallback == null)
                progressCallback = ConsoleProgressCallback;

            progressCallback("Character", $"Retrieving {characterName}", 10);
            var character = await _mySql.GetSingleAsync<Characters>(c => c.Name == characterName); // TODO: Check if Deleted chars must be excluded
            if (character == null)
            {
                progressCallback("Failed", $"{characterName} not found", 100);
                return null;
            }

            progressCallback("Customizations", "Retrieving customizations", 30);
            var characterCustomization = await _mySql.GetAsync<CharacterCustomizations>(c => c.Guid == character.Guid);
            progressCallback("Customizations", $"Found {characterCustomization.Count()} customizations", 40);

            var customizations = new Dictionary<int, int?>();
            foreach (var customization in characterCustomization)
            {
                customizations.Add(customization.ChrCustomizationOptionId, customization.ChrCustomizationChoiceId);
            }
            var result = new CreatureDto()
            {
                Id = await GetNextIdAsync(),
                Race = character.Race,
                Gender = character.Gender,
                Customizations = customizations,
                Level = character.Level,
                Name = character.Name,
                Auras = new(),
                SoundId = GetDefaultSoundId(character.Race, character.Gender)
            };

            progressCallback("Equipment", "Retrieving equipment", 50);
            var characterItems = await _mySql.GetAsync<ItemInstance>(c => c.OwnerGuid == character.Guid);
            var characterEquipMap = await _mySql.GetAsync<CharacterInventory>(c => c.Guid == character.Guid);
            foreach (var equippedItem in characterEquipMap.OrderBy(c => c.Slot))
            {
                progressCallback("Equipment", $"Handling {equippedItem.Slot.ToString().ToLower().Replace("_", "")} slot", Math.Min(50 + 2 * (int)equippedItem.Slot, 99));

                var item = characterItems.Where(i => i.Guid == equippedItem.Item).FirstOrDefault();
                if (item == null || (int)equippedItem.Slot > Enum.GetValues(typeof(CharacterInventorySlots)).Cast<int>().Max())
                    continue;

                var itemAppearanceId = 0;
                var itemAppearanceModifierId = 0;
                var itemId = item.ItemEntry;
                var itemVisual = 0;
                var transmogItem = await _mySql.GetSingleAsync<ItemInstanceTransmog>(c => c.ItemGuid == equippedItem.Item);

                // Try get ItemAppearanceId by transmog.
                // Note: Dont mix up ItemModifiedAppearance.Id with ItemAppearanceModifierId.
                if (transmogItem != null)
                {
                    var itemModifiedAppearance = await _db2.GetSingleAsync<ItemModifiedAppearance>(c => c.Id == transmogItem.ItemModifiedAppearanceAllSpecs);
                    if (itemModifiedAppearance != null)
                    {
                        itemAppearanceId = itemModifiedAppearance.ItemAppearanceId;
                        itemAppearanceModifierId = itemModifiedAppearance.ItemAppearanceModifierId;
                        itemId = itemModifiedAppearance.ItemId;
                    }
                    if (transmogItem.SpellItemEnchantmentAllSpecs > 0 && IsWeaponSlot(equippedItem.Slot))
                    {
                        var spellItemEnchantment = await _db2.GetSingleAsync<SpellItemEnchantment>(s => s.Id == transmogItem.SpellItemEnchantmentAllSpecs);
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
                            var itemBonus = await _db2.GetSingleAsync<ItemBonus>(bonus => bonus.Type == 7 && bonusListIds.Any(bonusId => bonusId == bonus.ParentItemBonusListId));
                            if (itemBonus != null)
                            {
                                itemAppearanceModifierId = itemBonus.Value0;
                            }
                        }
                    }

                    var itemModifiedAppearance = await _db2.GetSingleAsync<ItemModifiedAppearance>(c => c.ItemId == item.ItemEntry && c.ItemAppearanceModifierId == itemAppearanceModifierId);
                    if (itemModifiedAppearance == null)
                        continue;

                    itemAppearanceId = itemModifiedAppearance.ItemAppearanceId;
                }

                if (itemVisual == 0 && IsWeaponSlot(equippedItem.Slot))
                {
                    foreach (var enchantment in item.Enchantments.Split(' '))
                    {
                        if (int.TryParse(enchantment.Trim(), out var enchantmentId))
                        {
                            if (enchantmentId > 0)
                            {
                                var spellItemEnchantment = await _db2.GetSingleAsync<SpellItemEnchantment>(s => s.Id == enchantmentId);
                                if (spellItemEnchantment != null && spellItemEnchantment.ItemVisual > 0)
                                {
                                    itemVisual = spellItemEnchantment.ItemVisual;
                                    break;
                                }
                            }
                        }
                    }
                }

                var itemAppearance = await _db2.GetSingleAsync<ItemAppearance>(c => c.Id == itemAppearanceId);
                if (itemAppearance == null)
                    continue;

                switch (equippedItem.Slot)
                {
                    case CharacterInventorySlots.HEAD:
                        result.HeadItemDisplayInfoId = itemAppearance.ItemDisplayInfoId;
                        break;

                    case CharacterInventorySlots.BACK:
                        result.BackItemDisplayInfoId = itemAppearance.ItemDisplayInfoId;
                        break;

                    case CharacterInventorySlots.SHOULDERS:
                        result.ShouldersItemDisplayInfoId = itemAppearance.ItemDisplayInfoId;
                        break;

                    case CharacterInventorySlots.CHEST:
                        result.ChestItemDisplayInfoId = itemAppearance.ItemDisplayInfoId;
                        break;

                    case CharacterInventorySlots.WAIST:
                        result.WaistItemDisplayInfoId = itemAppearance.ItemDisplayInfoId;
                        break;

                    case CharacterInventorySlots.TABARD:
                        result.TabardItemDisplayInfoId = itemAppearance.ItemDisplayInfoId;
                        break;

                    case CharacterInventorySlots.SHIRT:
                        result.ShirtItemDisplayInfoId = itemAppearance.ItemDisplayInfoId;
                        break;

                    case CharacterInventorySlots.LEGS:
                        result.LegsItemDisplayInfoId = itemAppearance.ItemDisplayInfoId;
                        break;

                    case CharacterInventorySlots.FEET:
                        result.FeetItemDisplayInfoId = itemAppearance.ItemDisplayInfoId;
                        break;

                    case CharacterInventorySlots.WRISTS:
                        result.WristsItemDisplayInfoId = itemAppearance.ItemDisplayInfoId;
                        break;

                    case CharacterInventorySlots.HANDS:
                        result.HandsItemDisplayInfoId = itemAppearance.ItemDisplayInfoId;
                        break;

                    case CharacterInventorySlots.MAIN_HAND:
                        result.MainHandItemId = itemId;
                        result.MainHandItemAppearanceModifierId = itemAppearanceModifierId;
                        result.MainHandItemVisual = itemVisual;
                        break;

                    case CharacterInventorySlots.OFF_HAND:
                        result.OffHandItemId = itemId;
                        result.OffHandItemAppearanceModifierId = itemAppearanceModifierId;
                        result.OffHandItemVisual = itemVisual;
                        break;

                    case CharacterInventorySlots.RANGED:
                        result.RangedItemId = itemId;
                        result.RangedItemAppearanceModifierId = itemAppearanceModifierId;
                        result.RangedItemVisual = itemVisual;
                        break;
                }
            }
            progressCallback("Done", "Creature returned", 100);
            return result;
        }

        public async Task<List<CreatureDto>> GetByIdAsync(int id, Action<string, string, int>? progressCallback = null)
        {
            if (progressCallback == null)
                progressCallback = ConsoleProgressCallback;

            progressCallback("Creature", $"Retrieving DisplayInfos for Creature ID {id}", 0);
            // This will only return the first model.
            // If the desired result has multiple displays, use the desired one directly instead.
            var creatureTemplateModels = await _mySql.GetAsync<CreatureTemplateModel>(c => c.CreatureId == id);
            if (creatureTemplateModels.Any())
            {
                var creatures = await GetByDisplayIdsAsync(creatureTemplateModels.Select(c => c.CreatureId).ToList(), progressCallback);
                if (creatures.Any())
                {
                    foreach (var creature in creatures)
                    {
                        if (id < IdRangeTo && id >= IdRangeFrom)
                        {
                            // Override the automatically generated Id, as this is most likely an update.
                            creature.Id = id;
                            creature.IsUpdate = true;
                        }
                    }
                    progressCallback("Done", "Returning creature", 100);
                    return creatures;
                }
            }
            progressCallback("Creature", "No creatures found", 100);
            return new List<CreatureDto>();
        }

        public async Task<CreatureDto?> GetByDisplayIdAsync(int displayId, Action<string, string, int>? progressCallback = null)
        {
            if (progressCallback == null)
                progressCallback = ConsoleProgressCallback;

            progressCallback("Creature", $"Retrieving DisplayInfo for ID {displayId}", 0);
            var result = await GetByDisplayIdsAsync(new List<int>() { displayId }, progressCallback);

            progressCallback("Done", "Returning creature", 100);
            return result.FirstOrDefault();
        }

        async Task<List<CreatureDto>> GetByDisplayIdsAsync(List<int> displayIds, Action<string, string, int>? progressCallback)
        {
            if (progressCallback == null)
                progressCallback = ConsoleProgressCallback;

            int index = 0;
            var result = new List<CreatureDto>();
            foreach (var creatureDisplayId in displayIds)
            {
                index++;
                double iterationDivider = index / displayIds.Count();

                progressCallback("Creature", $"Retrieving Creature Template Model", (int)(10 / iterationDivider));
                CreatureTemplate? creatureTemplate = null;
                var creatureTemplateModel = await _mySql.GetSingleAsync<CreatureTemplateModel>(c => c.CreatureDisplayId == creatureDisplayId);
                if (creatureTemplateModel != null)
                {
                    progressCallback("Creature", $"Retrieving Creature Template", (int)(15 / iterationDivider));
                    creatureTemplate = await _mySql.GetSingleAsync<CreatureTemplate>(c => c.Entry == creatureTemplateModel.CreatureId);
                    if (creatureTemplate != null)
                        progressCallback("Creature", $"Found Creature Template ({creatureTemplate.Entry})", (int)(15 / iterationDivider));
                }

                progressCallback("Creature", $"Retrieving Display Info", (int)(20 / iterationDivider));
                var displayInfo = await _mySql.GetSingleAsync<CreatureDisplayInfo>(c => c.Id == creatureDisplayId) ?? await _db2.GetSingleAsync<CreatureDisplayInfo>(c => c.Id == creatureDisplayId);
                if (displayInfo == null)
                {
                    progressCallback("Failed", $"Display Info for Display Id {creatureDisplayId} not found", (int)(20 / iterationDivider));
                    continue;
                }

                var auras = new List<int>();
                if (creatureTemplate != null)
                {
                    progressCallback("Creature", $"Retrieving Auras", (int)(25 / iterationDivider));
                    var creatureTemplateAddon = await _mySql.GetSingleAsync<CreatureTemplateAddon>(c => c.Entry == creatureTemplate.Entry);
                    if (creatureTemplateAddon != null && !string.IsNullOrWhiteSpace(creatureTemplateAddon.Auras))
                    {
                        var auraStrings = creatureTemplateAddon.Auras.Split(' ');
                        foreach (var auraString in auraStrings)
                        {
                            if (int.TryParse(auraString.Trim(), out var aura) && aura != 0)
                                auras.Add(aura);
                        }
                    }

                }

                progressCallback("Creature", $"Retrieving Display Info Extra", (int)(30 / iterationDivider));
                var displayInfoExtra = await _mySql.GetSingleAsync<CreatureDisplayInfoExtra>(c => c.Id == displayInfo.ExtendedDisplayInfoId) ?? await _db2.GetSingleAsync<CreatureDisplayInfoExtra>(c => c.Id == displayInfo.ExtendedDisplayInfoId);
                if (displayInfoExtra == null)
                {
                    progressCallback("Creature", $"Display Info Extra not found", (int)(30 / iterationDivider));
                    continue;
                }

                var creature = new CreatureDto()
                {
                    Id = await GetNextIdAsync(),
                    Gender = displayInfo.Gender,
                    Race = displayInfoExtra.DisplayRaceId,
                    CreatureType = creatureTemplate != null ? creatureTemplate.Type : CreatureTypes.HUMANOID,
                    Faction = creatureTemplate != null ? creatureTemplate.Faction : 17,
                    CreatureUnitClass = creatureTemplate != null ? creatureTemplate.UnitClass : CreatureUnitClasses.WARRIOR,
                    Level = creatureTemplate != null && creatureTemplate.MaxLevel > 0 ? creatureTemplate.MaxLevel : 30,
                    Name = creatureTemplate?.Name,
                    SubName = creatureTemplate?.SubName,
                    Scale = creatureTemplate?.Scale ?? 1.0M,
                    Rank = creatureTemplate?.Rank ?? CreatureRanks.NORMAL,
                    HealthModifier = creatureTemplate?.HealthModifier ?? 1,
                    DamageModifier = creatureTemplate?.DamageModifier ?? 1,
                    FlagsExtra = (UnitFlagsExtra)(creatureTemplate?.FlagsExtra ?? 0),
                    UnitFlags = (UnitFlags)(creatureTemplate?.UnitFlags ?? 0),
                    UnitFlags2 = (UnitFlags2)(creatureTemplate?.UnitFlags2 ?? 0),
                    UnitFlags3 = (UnitFlags3)(creatureTemplate?.UnitFlags3 ?? 0),
                    ArmorModifier = creatureTemplate?.ArmorModifier ?? 1,
                    SoundId = displayInfo.SoundId,
                    RegenHealth = creatureTemplate?.RegenHealth ?? true,
                    Auras = auras,

                    IsUpdate = false,
                    SearchResultName = creatureDisplayId.ToString()
                };

                progressCallback("Customizations", $"Retrieving available customizations", (int)(40 / iterationDivider));
                var availableCustomizations = await GetAvailableCustomizations(displayInfoExtra.DisplayRaceId, displayInfo.Gender);
                progressCallback("Customizations", $"Retrieving creature customizations", (int)(60 / iterationDivider));
                var customizations = await _mySql.GetAsync<CreatureDisplayInfoOption>(h => h.CreatureDisplayInfoExtraId == displayInfoExtra.Id) ?? await _db2.GetAsync<CreatureDisplayInfoOption>(h => h.CreatureDisplayInfoExtraId == displayInfoExtra.Id);
                var creatureCustomizations = new Dictionary<int, int?>();

                foreach (var availableCustomization in availableCustomizations)
                {
                    progressCallback("Customizations", $"Preparing {availableCustomization.Key.Name}", (int)(70 / iterationDivider));

                    var customization = customizations.Where(hc => hc.ChrCustomizationOptionId == availableCustomization.Key.Id).FirstOrDefault();

                    if (customization != null)
                    {
                        creatureCustomizations.Add(customization.ChrCustomizationOptionId, customization.ChrCustomizationChoiceId);
                    }
                    else
                    {
                        creatureCustomizations.Add(availableCustomization.Key.Id, availableCustomization.Value.First().Id);
                    }
                }
                creature.Customizations = creatureCustomizations;

                progressCallback("Equipment", $"Retrieving equipment", (int)(80 / iterationDivider));
                var hotfixEquipment = await _mySql.GetAsync<NpcModelItemSlotDisplayInfo>(npc => npc.NpcModelId == displayInfoExtra.Id);
                var db2Equipment = await _db2.GetAsync<NpcModelItemSlotDisplayInfo>(npc => npc.NpcModelId == displayInfoExtra.Id);

                for (int i = 0; i <= Enum.GetValues(typeof(ArmorSlots)).Cast<int>().Max(); i++)
                {
                    var itemSlot = (ArmorSlots)i;
                    progressCallback("Equipment", $"Handling {itemSlot.ToString().ToLower().Replace("_", "")} slot", (int)(90 / iterationDivider));

                    var hotfixItem = hotfixEquipment.FirstOrDefault(h => (int)h.ItemSlot == i);
                    var db2Item = db2Equipment.FirstOrDefault(d => (int)d.ItemSlot == i);
                    var item = hotfixItem != null ? hotfixItem : (db2Item != null ? db2Item : null);
                    if (item == null || item.ItemDisplayInfoId == 0)
                        continue;

                    switch (itemSlot)
                    {
                        case ArmorSlots.HEAD:
                            creature.HeadItemDisplayInfoId = item.ItemDisplayInfoId;
                            break;
                        case ArmorSlots.SHOULDERS:
                            creature.ShouldersItemDisplayInfoId = item.ItemDisplayInfoId;
                            break;
                        case ArmorSlots.CHEST:
                            creature.ChestItemDisplayInfoId = item.ItemDisplayInfoId;
                            break;
                        case ArmorSlots.WAIST:
                            creature.WaistItemDisplayInfoId = item.ItemDisplayInfoId;
                            break;
                        case ArmorSlots.HANDS:
                            creature.HandsItemDisplayInfoId = item.ItemDisplayInfoId;
                            break;
                        case ArmorSlots.TABARD:
                            creature.TabardItemDisplayInfoId = item.ItemDisplayInfoId;
                            break;
                        case ArmorSlots.SHIRT:
                            creature.ShirtItemDisplayInfoId = item.ItemDisplayInfoId;
                            break;
                        case ArmorSlots.BACK:
                            creature.BackItemDisplayInfoId = item.ItemDisplayInfoId;
                            break;
                        case ArmorSlots.WRISTS:
                            creature.WristsItemDisplayInfoId = item.ItemDisplayInfoId;
                            break;
                        case ArmorSlots.LEGS:
                            creature.LegsItemDisplayInfoId = item.ItemDisplayInfoId;
                            break;
                        case ArmorSlots.FEET:
                            creature.FeetItemDisplayInfoId = item.ItemDisplayInfoId;
                            break;
                        case ArmorSlots.QUIVER:
                            creature.QuiverItemDisplayInfoId = item.ItemDisplayInfoId;
                            break;
                    }
                }

                progressCallback("Equipment", $"Handling weapons", (int)(95 / iterationDivider));
                if (creatureTemplate != null)
                {
                    var creatureEquipTemplate = await _mySql.GetSingleAsync<CreatureEquipTemplate>(c => c.CreatureId == creatureTemplate.Entry);
                    if (creatureEquipTemplate != null)
                    {
                        if (creatureEquipTemplate.ItemId1 > 0)
                            creature.MainHandItemId = creatureEquipTemplate.ItemId1;
                        if (creatureEquipTemplate.AppearanceModId1 > 0)
                            creature.MainHandItemAppearanceModifierId = creatureEquipTemplate.AppearanceModId1;
                        if (creatureEquipTemplate.ItemVisual1 > 0)
                            creature.MainHandItemVisual = creatureEquipTemplate.ItemVisual1;

                        if (creatureEquipTemplate.ItemId2 > 0)
                            creature.OffHandItemId = creatureEquipTemplate.ItemId2;
                        if (creatureEquipTemplate.AppearanceModId2 > 0)
                            creature.OffHandItemAppearanceModifierId = creatureEquipTemplate.AppearanceModId2;
                        if (creatureEquipTemplate.ItemVisual2 > 0)
                            creature.OffHandItemVisual = creatureEquipTemplate.ItemVisual2;

                        if (creatureEquipTemplate.ItemId3 > 0)
                            creature.RangedItemId = creatureEquipTemplate.ItemId3;
                        if (creatureEquipTemplate.AppearanceModId3 > 0)
                            creature.RangedItemAppearanceModifierId = creatureEquipTemplate.AppearanceModId3;
                        if (creatureEquipTemplate.ItemVisual3 > 0)
                            creature.RangedItemVisual = creatureEquipTemplate.ItemVisual3;
                    }
                }
                result.Add(creature);
            }

            return result;
        }

        public async Task<List<DashboardModel>> GetDashboardAsync()
        {
            var creatures = await _mySql.GetAsync<CreatureTemplate>(c => c.VerifiedBuild == VerifiedBuild);
            var result = new List<DashboardModel>();
            foreach (var creature in creatures)
            {
                var displayInfo = await _mySql.GetSingleAsync<CreatureDisplayInfoExtra>(c => c.Id == creature.Entry);
                if (displayInfo == null)
                    continue;
                result.Add(new DashboardModel()
                {
                    Id = creature.Entry,
                    Name = creature.Name,
                    Comment = "TODO",
                    AvatarUrl = $"/images/creatures/avatars/{displayInfo.DisplaySexId.ToString().ToLower()}/{displayInfo.DisplayRaceId.ToString().ToLower().Replace("_", "")}.jpg"
                });
            }
            // Newest on top
            result.Reverse();
            return result;
        }

        async Task DeleteFromHotfixesAsync(int id)
        {
            var creatureDisplayInfo = await _mySql.GetSingleAsync<CreatureDisplayInfo>(c => c.Id == id);
            var creatureDisplayInfoExtra = await _mySql.GetSingleAsync<CreatureDisplayInfoExtra>(c => c.Id == id);
            var creatureDisplayInfoOptions = await _mySql.GetAsync<CreatureDisplayInfoOption>(c => c.CreatureDisplayInfoExtraId == id);
            var npcModelItemSlotDisplayInfos = await _mySql.GetAsync<NpcModelItemSlotDisplayInfo>(c => c.NpcModelId == id);
            var hotfixModsData = await _mySql.GetSingleAsync<HotfixModsData>(h => h.Id == id && h.VerifiedBuild == VerifiedBuild);

            if (null != creatureDisplayInfo)
                await _mySql.DeleteAsync(creatureDisplayInfo);

            if (null != creatureDisplayInfoExtra)
                await _mySql.DeleteAsync(creatureDisplayInfoExtra);

            if (creatureDisplayInfoOptions.Any())
                await _mySql.DeleteAsync(creatureDisplayInfoOptions.ToArray());

            if (npcModelItemSlotDisplayInfos.Any())
                await _mySql.DeleteAsync(npcModelItemSlotDisplayInfos.ToArray());

            var hotfixData = await _mySql.GetAsync<HotfixData>(h => h.UniqueId == id && h.VerifiedBuild == VerifiedBuild);
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

        async Task DeleteFromWorldAsync(int id)
        {
            var creatures = await _mySql.GetAsync<Creature>(c => c.Guid == id);
            var creatureTemplate = await _mySql.GetSingleAsync<CreatureTemplate>(c => c.Entry == id);
            var creatureTemplateAddon = await _mySql.GetSingleAsync<CreatureTemplateAddon>(c => c.Entry == id);
            var creatureTemplateModel = await _mySql.GetSingleAsync<CreatureTemplateModel>(c => c.CreatureId == id);
            var creatureEquipTemplate = await _mySql.GetSingleAsync<CreatureEquipTemplate>(c => c.CreatureId == id);
            var creatureModelInfo = await _mySql.GetSingleAsync<CreatureModelInfo>(c => c.DisplayId == id);

            if (null != creatureTemplate)
                await _mySql.DeleteAsync(creatureTemplate);

            if (null != creatureTemplateAddon)
                await _mySql.DeleteAsync(creatureTemplateAddon);

            if (null != creatureTemplateModel)
                await _mySql.DeleteAsync(creatureTemplateModel);

            if (null != creatureEquipTemplate)
                await _mySql.DeleteAsync(creatureEquipTemplate);

            if (null != creatureModelInfo)
                await _mySql.DeleteAsync(creatureModelInfo);

            if (creatures.Count() > 0)
                await _mySql.DeleteAsync(creatures.ToArray());
        }
    }
}
