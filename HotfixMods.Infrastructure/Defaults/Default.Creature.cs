using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using HotfixMods.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Defaults
{
    public static partial class Default
    {
        public static readonly CreatureTemplate CreatureTemplate = new()
        {
            ArmorModifier = 1,
            HealthModifier = 1,
            DamageModifier = 1,
            Faction = 0,
            FlagsExtra = UnitFlagsExtra.NONE,
            MaxLevel = 1,
            MinLevel = 1,
            Name = "New Creature",
            Rank = CreatureRanks.NORMAL,
            Scale = 1,
            SubName = "",
            Type = CreatureTypes.HUMANOID,
            UnitClass = CreatureUnitClasses.WARRIOR,
            UnitFlags = UnitFlags.NONE,
            UnitFlags2 = UnitFlags2.NONE,
            UnitFlags3 = UnitFlags3.NONE,
            RegenHealth = true,
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
            SpeedRun = 1.14286M,
            SpeedWalk = 1,

            Entry = -1,
            VerifiedBuild = -1
        };

        public static readonly CreatureEquipTemplate CreatureEquipTemplate = new()
        {
            Id = 1, // Id is more like order/index for CreatureEquipTemplate (for multiple equipment)
            AppearanceModId1 = 0,
            AppearanceModId2 = 0,
            AppearanceModId3 = 0,
            ItemId1 = 0,
            ItemId2 = 0,
            ItemId3 = 0,
            ItemVisual1 = 0,
            ItemVisual2 = 0,
            ItemVisual3 = 0,

            VerifiedBuild = -1,
            CreatureId = -1
        };

        public static readonly CreatureTemplateModel CreatureTemplateModel = new()
        {
            DisplayScale = 1,
            Probability = 1,

            CreatureDisplayId = -1,
            CreatureId = -1,
            VerifiedBuild = -1
        };

        public static readonly CreatureDisplayInfo CreatureDisplayInfo = new()
        {
            UnarmedWeaponType = -1,
            CreatureModelAlpha = 255,
            CreatureModelScale = 1,
            PetInstanceScale = 1,
            SizeClass = 1,
            SoundId = 0,
            Gender = Genders.MALE,

            Id = -1,
            VerifiedBuild = -1,
            ModelId = -1,
            ExtendedDisplayInfoId = -1
        };

        public static readonly CreatureDisplayInfoExtra CreatureDisplayInfoExtra = new()
        {
            DisplaySexId = Genders.MALE,
            DisplayRaceId = Races.HUMAN,

            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly NpcModelItemSlotDisplayInfo NpcModelItemSlotDisplayInfo = new()
        {
            ItemDisplayInfoId = 0,

            Id = -1,
            VerifiedBuild = -1,
            NpcModelId = -1,
            ItemSlot = (ArmorSlots)(-1)
        };

        public static readonly CreatureModelInfo CreatureModelInfo = new()
        {
            // Unused
        };

        public static readonly CreatureTemplateAddon CreatureTemplateAddon = new()
        {
            // Unused
        };

        public static readonly CreatureDisplayInfoOption CreatureDisplayInfoOption = new()
        {
            // Unused
        };
    }
}
