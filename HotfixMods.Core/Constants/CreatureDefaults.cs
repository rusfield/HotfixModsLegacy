using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Constants
{
    public abstract class CreatureDefaults
    {
        public const string Name = "New Creature";
        public const int Faction = 31;
        public const int MinLevel = 1;
        public const int MaxLevel = 1;
        public const CreatureRanks Rank = CreatureRanks.NORMAL;
        public const string SubName = "";
        public const CreatureTypes Type = CreatureTypes.HUMANOID;
        public const CreatureUnitClasses UnitClass = CreatureUnitClasses.WARRIOR;
        public const decimal Scale = 1;
        public const decimal HealthModifier = 1;
        public const decimal ArmorModifier = 1;
        public const decimal DamageModifier = 1;
        public const UnitFlags UnitFlag = UnitFlags.NONE;
        public const UnitFlags2 UnitFlag2 = UnitFlags2.NONE;
        public const UnitFlags3 UnitFlag3 = UnitFlags3.NONE;
        public const UnitFlagsExtra FlagsExtra = UnitFlagsExtra.NONE;
        public const bool RegenHealth = true;
        public const int MainHandItemId = 0;
        public const int OffHandItemId = 0;
        public const int RangedItemId = 0;
        public const int MainHandItemVisual = 0;
        public const int OffHandItemVisual = 0;
        public const int RangedItemVisual = 0;
        public const int MainHandItemAppearanceModifierId = 0;
        public const int OffHandItemAppearanceModifierId = 0;
        public const int RangedItemAppearanceModifierId = 0;
        public const Genders Gender = Genders.Male;
        public const Races Race = Races.HUMAN;
        public const int SoundId = 0;
        public const int HeadItemDisplayInfoId = 0;
        public const int ShouldersItemDisplayInfoId = 0;
        public const int ShirtItemDisplayInfoId = 0;
        public const int ChestItemDisplayInfoId = 0;
        public const int WaistItemDisplayInfoId = 0;
        public const int LegsItemDisplayInfoId = 0;
        public const int FeetItemDisplayInfoId = 0;
        public const int WristsItemDisplayInfoId = 0;
        public const int HandsItemDisplayInfoId = 0;
        public const int TabardItemDisplayInfoId = 0;
        public const int BackItemDisplayInfoId = 0;
        public const int QuiverItemDisplayInfoId = 0;
    }
}
