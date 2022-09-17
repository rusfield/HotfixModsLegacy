using HotfixMods.Infrastructure.DefaultModels;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService
    {
        CreatureTemplate BuildCreatureTemplate(CreatureDto creatureDto)
        {
            return new CreatureTemplate()
            {
                Entry = creatureDto.Id,
                VerifiedBuild = VerifiedBuild,

                Name = string.IsNullOrWhiteSpace(creatureDto.Name) ? Default.CreatureTemplate.Name : creatureDto.Name,
                Faction = creatureDto.Faction ?? Default.CreatureTemplate.Faction,
                MaxLevel = creatureDto.Level ?? Default.CreatureTemplate.MaxLevel,
                MinLevel = creatureDto.Level ?? Default.CreatureTemplate.MinLevel,
                Rank = creatureDto.Rank ?? Default.CreatureTemplate.Rank,
                SubName = string.IsNullOrWhiteSpace(creatureDto.SubName) ? Default.CreatureTemplate.SubName : creatureDto.SubName,
                Type = creatureDto.CreatureType ?? Default.CreatureTemplate.Type,
                UnitClass = creatureDto.CreatureUnitClass ?? Default.CreatureTemplate.UnitClass,
                Scale = creatureDto.Scale ?? Default.CreatureTemplate.Scale,
                HealthModifier = creatureDto.HealthModifier ?? Default.CreatureTemplate.HealthModifier,
                DamageModifier = creatureDto.DamageModifier ?? Default.CreatureTemplate.DamageModifier,
                ArmorModifier = creatureDto.ArmorModifier ?? Default.CreatureTemplate.ArmorModifier,
                UnitFlags = creatureDto.UnitFlags ?? Default.CreatureTemplate.UnitFlags,
                UnitFlags2 = creatureDto.UnitFlags2 ?? Default.CreatureTemplate.UnitFlags2,
                UnitFlags3 = creatureDto.UnitFlags3 ?? Default.CreatureTemplate.UnitFlags3,
                FlagsExtra = creatureDto.FlagsExtra ?? Default.CreatureTemplate.FlagsExtra,
                RegenHealth = creatureDto.RegenHealth ?? Default.CreatureTemplate.RegenHealth,

                AiName = Default.CreatureTemplate.AiName,
                BaseAttackTime = Default.CreatureTemplate.BaseAttackTime,
                BaseVariance = Default.CreatureTemplate.BaseVariance,
                ExperienceModifier = Default.CreatureTemplate.ExperienceModifier,
                HealthModifierExtra = Default.CreatureTemplate.HealthModifier,
                HoverHeight = Default.CreatureTemplate.HoverHeight,
                ManaModifier = Default.CreatureTemplate.ManaModifier,
                ManaModifierExtra = Default.CreatureTemplate.ManaModifierExtra,
                RangeAttackTime = Default.CreatureTemplate.RangeAttackTime,
                RangeVariance = Default.CreatureTemplate.RangeVariance,
                SpeedRun = Default.CreatureTemplate.SpeedRun,
                SpeedWalk = Default.CreatureTemplate.SpeedWalk
            };
        }

        CreatureEquipTemplate BuildCreatureEquipTemplate(CreatureDto creatureDto)
        {
            return new CreatureEquipTemplate()
            {
                CreatureId = creatureDto.Id,
                VerifiedBuild = VerifiedBuild,

                ItemId1 = creatureDto.MainHandItemId ?? Default.CreatureEquipTemplate.ItemId1,
                AppearanceModId1 = creatureDto.MainHandItemAppearanceModifierId ?? Default.CreatureEquipTemplate.AppearanceModId1,
                ItemVisual1 = creatureDto.MainHandItemVisual ?? Default.CreatureEquipTemplate.ItemVisual1,
                ItemId2 = creatureDto.OffHandItemId ?? Default.CreatureEquipTemplate.ItemId2,
                AppearanceModId2 = creatureDto.OffHandItemAppearanceModifierId ?? Default.CreatureEquipTemplate.AppearanceModId2,
                ItemVisual2 = creatureDto.OffHandItemVisual ?? Default.CreatureEquipTemplate.ItemVisual2,
                ItemId3 = creatureDto.RangedItemId ?? Default.CreatureEquipTemplate.ItemId3,
                AppearanceModId3 = creatureDto.RangedItemAppearanceModifierId ?? Default.CreatureEquipTemplate.AppearanceModId3,
                ItemVisual3 = creatureDto.RangedItemVisual ?? Default.CreatureEquipTemplate.ItemVisual3,

                Id = Default.CreatureEquipTemplate.Id
            };
        }

        CreatureTemplateModel BuildCreatureTemplateModel(CreatureDto creatureDto)
        {
            return new CreatureTemplateModel()
            {
                CreatureId = creatureDto.Id,
                CreatureDisplayId = creatureDto.Id,
                VerifiedBuild = VerifiedBuild,

                Probability = Default.CreatureTemplateModel.Probability,
                DisplayScale = Default.CreatureTemplateModel.DisplayScale
            };
        }

        CreatureModelInfo BuildCreatureModelInfo(CreatureDto creatureDto)
        {
            return new CreatureModelInfo()
            {
                DisplayId = creatureDto.Id,
                VerifiedBuild = VerifiedBuild
            };
        }

        CreatureTemplateAddon BuildCreatureTemplateAddon(CreatureDto creatureDto)
        {
            return new CreatureTemplateAddon()
            {
                Entry = creatureDto.Id,
                Auras = string.Join(" ", creatureDto.Auras)
            };
        }

        CreatureDisplayInfo BuildCreatureDisplayInfo(CreatureDto creatureDto)
        {
            creatureDto.AddHotfix(creatureDto.Id, TableHashes.CREATURE_DISPLAY_INFO, HotfixStatuses.VALID);

            var gender = creatureDto.Gender ?? Default.CreatureDisplayInfo.Gender;
            var race = creatureDto.Race ?? Races.HUMAN;
            return new CreatureDisplayInfo()
            {
                Id = creatureDto.Id,
                ExtendedDisplayInfoId = creatureDto.Id,
                VerifiedBuild = VerifiedBuild,

                ModelId = GetModelIdByRaceAndGenders(race, gender, creatureDto.Customizations),
                Gender = gender,
                SoundId = creatureDto.SoundId ?? Default.CreatureDisplayInfo.SoundId,

                UnarmedWeaponType = Default.CreatureDisplayInfo.UnarmedWeaponType,
                CreatureModelAlpha = Default.CreatureDisplayInfo.CreatureModelAlpha,
                CreatureModelScale = Default.CreatureDisplayInfo.CreatureModelScale,
                PetInstanceScale = Default.CreatureDisplayInfo.PetInstanceScale,
                SizeClass = Default.CreatureDisplayInfo.SizeClass
            };
        }

        CreatureDisplayInfoExtra BuildCreatureDisplayInfoExtra(CreatureDto creatureDto)
        {
            creatureDto.AddHotfix(creatureDto.Id, TableHashes.CREATURE_DISPLAY_INFO_EXTRA, HotfixStatuses.VALID);

            return new CreatureDisplayInfoExtra()
            {
                Id = creatureDto.Id,
                VerifiedBuild = VerifiedBuild,

                DisplayRaceId = creatureDto.Race ?? Default.CreatureDisplayInfoExtra.DisplayRaceId,
                DisplaySexId = creatureDto.Gender ?? Default.CreatureDisplayInfoExtra.DisplaySexId
            };
        }

        CreatureDisplayInfoOption[] BuildCreatureDisplayInfoOption(CreatureDto creature)
        {
            var result = new List<CreatureDisplayInfoOption>();
            foreach (var customization in creature.Customizations)
            {
                if (customization.Value == null)
                    continue;

                creature.AddHotfix(creature.Id + customization.Key, TableHashes.CREATURE_DISPLAY_INFO_OPTION, HotfixStatuses.VALID);
                result.Add(new CreatureDisplayInfoOption()
                {
                    Id = creature.Id + customization.Key,
                    ChrCustomizationOptionId = customization.Key,
                    ChrCustomizationChoiceId = (int)customization.Value,
                    CreatureDisplayInfoExtraId = creature.Id,
                    VerifiedBuild = VerifiedBuild
                });
            }
            return result.ToArray();
        }

        NpcModelItemSlotDisplayInfo[] BuildNpcModelItemSlotDisplayInfo(CreatureDto creatureDto)
        {
            int npcModelId = creatureDto.Id;

            var result = new List<NpcModelItemSlotDisplayInfo>();
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.HEAD,
                ItemSlot = ArmorSlots.HEAD,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild,

                ItemDisplayInfoId = creatureDto.HeadItemDisplayInfoId ?? Default.NpcModelItemSlotDisplayInfo.ItemDisplayInfoId
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.SHOULDERS,
                ItemSlot = ArmorSlots.SHOULDERS,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild,

                ItemDisplayInfoId = creatureDto.ShouldersItemDisplayInfoId ?? Default.NpcModelItemSlotDisplayInfo.ItemDisplayInfoId
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.SHIRT,
                ItemSlot = ArmorSlots.SHIRT,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild,

                ItemDisplayInfoId = creatureDto.ShirtItemDisplayInfoId ?? Default.NpcModelItemSlotDisplayInfo.ItemDisplayInfoId
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.CHEST,
                ItemSlot = ArmorSlots.CHEST,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild,

                ItemDisplayInfoId = creatureDto.ChestItemDisplayInfoId ?? Default.NpcModelItemSlotDisplayInfo.ItemDisplayInfoId
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.WAIST,
                ItemSlot = ArmorSlots.WAIST,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild,

                ItemDisplayInfoId = creatureDto.WaistItemDisplayInfoId ?? Default.NpcModelItemSlotDisplayInfo.ItemDisplayInfoId
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.LEGS,
                ItemSlot = ArmorSlots.LEGS,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild,

                ItemDisplayInfoId = creatureDto.LegsItemDisplayInfoId ?? Default.NpcModelItemSlotDisplayInfo.ItemDisplayInfoId
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.FEET,
                ItemSlot = ArmorSlots.FEET,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild,

                ItemDisplayInfoId = creatureDto.FeetItemDisplayInfoId ?? Default.NpcModelItemSlotDisplayInfo.ItemDisplayInfoId
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.WRISTS,
                ItemSlot = ArmorSlots.WRISTS,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild,

                ItemDisplayInfoId = creatureDto.WristsItemDisplayInfoId ?? Default.NpcModelItemSlotDisplayInfo.ItemDisplayInfoId
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.HANDS,
                ItemSlot = ArmorSlots.HANDS,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild,

                ItemDisplayInfoId = creatureDto.HandsItemDisplayInfoId ?? Default.NpcModelItemSlotDisplayInfo.ItemDisplayInfoId
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.TABARD,
                ItemSlot = ArmorSlots.TABARD,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild,

                ItemDisplayInfoId = creatureDto.TabardItemDisplayInfoId ?? Default.NpcModelItemSlotDisplayInfo.ItemDisplayInfoId
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.BACK,
                ItemSlot = ArmorSlots.BACK,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild,

                ItemDisplayInfoId = creatureDto.BackItemDisplayInfoId ?? Default.NpcModelItemSlotDisplayInfo.ItemDisplayInfoId
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.QUIVER,
                ItemSlot = ArmorSlots.QUIVER,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild,

                ItemDisplayInfoId = creatureDto.QuiverItemDisplayInfoId ?? Default.NpcModelItemSlotDisplayInfo.ItemDisplayInfoId
            });

            foreach (var item in result)
            {
                creatureDto.AddHotfix(item.Id, TableHashes.NPC_MODEL_ITEM_SLOT_DISPLAY_INFO, HotfixStatuses.VALID);
            }

            return result.ToArray();
        }
    }
}
