using HotfixMods.Core.Constants;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService
    {
        public CreatureTemplate BuildCreatureTemplate(CreatureDto creatureDto)
        {
            return new CreatureTemplate()
            {
                Entry = creatureDto.Id,
                Name = string.IsNullOrWhiteSpace(creatureDto.Name) ? CreatureDefaults.Name : creatureDto.Name,
                Faction = creatureDto.Faction ?? CreatureDefaults.Faction,
                MaxLevel = creatureDto.Level ?? CreatureDefaults.MaxLevel,
                MinLevel = creatureDto.Level ?? CreatureDefaults.MinLevel,
                Rank = creatureDto.Rank ?? CreatureDefaults.Rank,
                SubName = string.IsNullOrWhiteSpace(creatureDto.SubName) ? CreatureDefaults.SubName : creatureDto.SubName,
                Type = creatureDto.CreatureType ?? CreatureDefaults.Type,
                UnitClass = creatureDto.CreatureUnitClass ?? CreatureDefaults.UnitClass,
                Scale = creatureDto.Scale ?? CreatureDefaults.Scale,
                HealthModifier = creatureDto.HealthModifier ?? CreatureDefaults.HealthModifier,
                DamageModifier = creatureDto.DamageModifier ?? CreatureDefaults.DamageModifier,
                ArmorModifier = creatureDto.ArmorModifier ?? CreatureDefaults.ArmorModifier,
                UnitFlags = (long)(creatureDto.UnitFlags ?? CreatureDefaults.UnitFlag),
                UnitFlags2 = (long)(creatureDto.UnitFlags2 ?? CreatureDefaults.UnitFlag2),
                UnitFlags3 = (long)(creatureDto.UnitFlags3 ?? CreatureDefaults.UnitFlag3),
                FlagsExtra = (long)(creatureDto.FlagsExtra ?? CreatureDefaults.FlagsExtra),
                RegenHealth = creatureDto.RegenHealth ?? CreatureDefaults.RegenHealth,

                VerifiedBuild = VerifiedBuild,

                // Custom default values
                AiName = "SmartAI",
                BaseAttackTime = 2000,
                BaseVariance = 1,
                ExperienceModifier = 1,
                HealthModifierExtra = 1,
                HoverHeight = 1,
                ManaModifier = 1,
                ManaModifierExtra = 1,
                RangeAttackTime = 2000,
                RangeVariance = 1,
                
                SpeedRun = 1.14286,
                SpeedWalk = 1
            };
        }

        public CreatureEquipTemplate BuildCreatureEquipTemplate(CreatureDto creatureDto)
        {
            return new CreatureEquipTemplate()
            {
                CreatureId = creatureDto.Id,
                VerifiedBuild = VerifiedBuild,

                ItemId1 = creatureDto.MainHandItemId ?? CreatureDefaults.MainHandItemId,
                AppearanceModId1 = creatureDto.MainHandItemAppearanceModifierId ?? CreatureDefaults.MainHandItemAppearanceModifierId,
                ItemVisual1 = creatureDto.MainHandItemVisual ?? CreatureDefaults.MainHandItemVisual,
                ItemId2 = creatureDto.OffHandItemId ?? CreatureDefaults.OffHandItemId,
                AppearanceModId2 = creatureDto.OffHandItemAppearanceModifierId ?? CreatureDefaults.OffHandItemAppearanceModifierId,
                ItemVisual2 = creatureDto.OffHandItemVisual ?? CreatureDefaults.OffHandItemVisual,
                ItemId3 = creatureDto.RangedItemId ?? CreatureDefaults.RangedItemId,
                AppearanceModId3 = creatureDto.RangedItemAppearanceModifierId ?? CreatureDefaults.RangedItemAppearanceModifierId,
                ItemVisual3 = creatureDto.RangedItemVisual ?? CreatureDefaults.RangedItemVisual,
                
                Id = 1
            };
        }

        public CreatureTemplateModel BuildCreatureTemplateModel(CreatureDto creatureDto)
        {
            return new CreatureTemplateModel()
            {
                CreatureId = creatureDto.Id,
                CreatureDisplayId = creatureDto.Id,
                VerifiedBuild = VerifiedBuild,
                
                Probability = 1,
                DisplayScale = 1
            };
        }

        public CreatureModelInfo BuildCreatureModelInfo(CreatureDto creatureDto)
        {
            return new CreatureModelInfo()
            {
                DisplayId = creatureDto.Id,
                VerifiedBuild = VerifiedBuild
            };
        }

        public CreatureTemplateAddon BuildCreatureTemplateAddon(CreatureDto creatureDto)
        {
            return new CreatureTemplateAddon()
            {
                Entry = creatureDto.Id,
                Auras = string.Join(" ", creatureDto.Auras)
            };
        }

        public CreatureDisplayInfo BuildCreatureDisplayInfo(CreatureDto creatureDto)
        {
            creatureDto.AddHotfix(creatureDto.Id, TableHashes.CreatureDisplayInfo, HotfixStatuses.VALID);

            var gender = creatureDto.Gender ?? CreatureDefaults.Gender;
            var race = creatureDto.Race ?? CreatureDefaults.Race;
            return new CreatureDisplayInfo()
            {
                Id = creatureDto.Id,
                ExtendedDisplayInfoId = creatureDto.Id,
                VerifiedBuild = VerifiedBuild,

                ModelId = GetModelIdByRaceAndGenders(race, gender, creatureDto.Customizations),
                Gender = gender,
                SoundId = creatureDto.SoundId ?? CreatureDefaults.SoundId,

                UnarmedWeaponType = -1,
                CreatureModelAlpha = 255,
                CreatureModelScale = 1,
                PetInstanceScale = 1,
                SizeClass = 1
            };
        }

        public CreatureDisplayInfoExtra BuildCreatureDisplayInfoExtra(CreatureDto creatureDto)
        {
            creatureDto.AddHotfix(creatureDto.Id, TableHashes.CreatureDisplayInfoExtra, HotfixStatuses.VALID);

            return new CreatureDisplayInfoExtra()
            {
                Id = creatureDto.Id,
                VerifiedBuild = VerifiedBuild,

                DisplayRaceId = creatureDto.Race ?? CreatureDefaults.Race,
                DisplaySexId = creatureDto.Gender ?? CreatureDefaults.Gender
            };
        }

        public List<CreatureDisplayInfoOption> BuildCreatureDisplayInfoOption(CreatureDto creature)
        {
            var result = new List<CreatureDisplayInfoOption>();
            foreach (var customization in creature.Customizations)
            {
                if (customization.Value == null)
                    continue;

                creature.AddHotfix(creature.Id + customization.Key, TableHashes.CreatureDisplayInfoOption, HotfixStatuses.VALID);
                result.Add(new CreatureDisplayInfoOption()
                {
                    Id = creature.Id + customization.Key,
                    ChrCustomizationOptionId = customization.Key,
                    ChrCustomizationChoiceId = (int)customization.Value,
                    CreatureDisplayInfoExtraId = creature.Id,
                    VerifiedBuild = VerifiedBuild
                });
            }
            return result;
        }

        public List<NpcModelItemSlotDisplayInfo> BuildNpcModelItemSlotDisplayInfo(CreatureDto creatureDto)
        {
            int npcModelId = creatureDto.Id;

            var result = new List<NpcModelItemSlotDisplayInfo>();
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.HEAD,
                ItemDisplayInfoId = creatureDto.HeadItemDisplayInfoId ?? CreatureDefaults.HeadItemDisplayInfoId,
                ItemSlot = ArmorSlots.HEAD,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.SHOULDERS,
                ItemDisplayInfoId = creatureDto.ShouldersItemDisplayInfoId ?? CreatureDefaults.ShouldersItemDisplayInfoId,
                ItemSlot = ArmorSlots.SHOULDERS,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.SHIRT,
                ItemDisplayInfoId = creatureDto.ShirtItemDisplayInfoId ?? CreatureDefaults.ShirtItemDisplayInfoId,
                ItemSlot = ArmorSlots.SHIRT,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.CHEST,
                ItemDisplayInfoId = creatureDto.ChestItemDisplayInfoId ?? CreatureDefaults.ChestItemDisplayInfoId,
                ItemSlot = ArmorSlots.CHEST,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.WAIST,
                ItemDisplayInfoId = creatureDto.WaistItemDisplayInfoId ?? CreatureDefaults.WaistItemDisplayInfoId,
                ItemSlot = ArmorSlots.WAIST,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.LEGS,
                ItemDisplayInfoId = creatureDto.LegsItemDisplayInfoId ?? CreatureDefaults.LegsItemDisplayInfoId,
                ItemSlot = ArmorSlots.LEGS,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.FEET,
                ItemDisplayInfoId = creatureDto.FeetItemDisplayInfoId ?? CreatureDefaults.FeetItemDisplayInfoId,
                ItemSlot = ArmorSlots.FEET,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.WRISTS,
                ItemDisplayInfoId = creatureDto.WristsItemDisplayInfoId ?? CreatureDefaults.WristsItemDisplayInfoId,
                ItemSlot = ArmorSlots.WRISTS,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.HANDS,
                ItemDisplayInfoId = creatureDto.HandsItemDisplayInfoId ?? CreatureDefaults.HandsItemDisplayInfoId,
                ItemSlot = ArmorSlots.HANDS,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.TABARD,
                ItemDisplayInfoId = creatureDto.TabardItemDisplayInfoId ?? CreatureDefaults.TabardItemDisplayInfoId,
                ItemSlot = ArmorSlots.TABARD,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.BACK,
                ItemDisplayInfoId = creatureDto.BackItemDisplayInfoId ?? CreatureDefaults.BackItemDisplayInfoId,
                ItemSlot = ArmorSlots.BACK,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild
            });
            result.Add(new NpcModelItemSlotDisplayInfo()
            {
                Id = creatureDto.Id + (int)ArmorSlots.QUIVER,
                ItemDisplayInfoId = creatureDto.QuiverItemDisplayInfoId ?? CreatureDefaults.QuiverItemDisplayInfoId,
                ItemSlot = ArmorSlots.QUIVER,
                NpcModelId = npcModelId,
                VerifiedBuild = VerifiedBuild
            });

            foreach (var item in result)
            {
                creatureDto.AddHotfix(item.Id, TableHashes.NpcModelItemSlotDisplayInfo, HotfixStatuses.VALID);
            }

            return result;
        }
    }
}
