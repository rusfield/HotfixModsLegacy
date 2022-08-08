using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class CreatureDto : Dto
    {
        public Genders? Gender { get; set; }
        public Races? Race { get; set; }
        public string? Name { get; set; }
        public string? SubName { get; set; }
        public int? Level { get; set; }
        public int? Faction { get; set; }
        public double? Scale { get; set; }
        public Ranks? Rank { get; set; }
        public CreatureTypes? CreatureType { get; set; }
        public CreatureUnitClasses? CreatureUnitClass { get; set; }
        public UnitFlags? UnitFlags { get; set; }
        public UnitFlags2? UnitFlags2 { get; set; }
        public UnitFlags3? UnitFlags3 { get; set; }
        public FlagsExtra? FlagsExtra { get; set; }
        public double? HealthModifier { get; set; }
        public double? DamageModifier { get; set; }
        public double? ArmorModifier { get; set; }
        public int? SoundId { get; set; }
        public bool? RegenHealth { get; set; }
        public List<int> Auras { get; set; }

        // Armor
        public int? HeadItemDisplayInfoId { get; set; }
        public int? ShouldersItemDisplayInfoId { get; set; }
        public int? BackItemDisplayInfoId { get; set; }
        public int? ShirtItemDisplayInfoId { get; set; }
        public int? TabardItemDisplayInfoId { get; set; }
        public int? ChestItemDisplayInfoId { get; set; }
        public int? HandsItemDisplayInfoId { get; set; }
        public int? WaistItemDisplayInfoId { get; set; }
        public int? WristsItemDisplayInfoId { get; set; }
        public int? LegsItemDisplayInfoId { get; set; }
        public int? FeetItemDisplayInfoId { get; set; }
        public int? QuiverItemDisplayInfoId { get; set; }

        // Weapons
        public int? MainHandItemId { get; set; }
        public int? MainHandItemVisual { get; set; }
        public int? MainHandItemAppearanceModifierId { get; set; }
        public int? OffHandItemId { get; set; }
        public int? OffHandItemVisual { get; set; }
        public int? OffHandItemAppearanceModifierId { get; set; }
        public int? RangedItemId { get; set; }
        public int? RangedItemVisual { get; set; }
        public int? RangedItemAppearanceModifierId { get; set; }

        public Dictionary<int, int> Customizations { get; set; }

    }
}
