using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotfixMods.Core.Models
{
    public class CreatureTemplate : IWorldSchema
    {
        public int Entry { get; set; }
        public string? Name { get; set; }
        public string? SubName { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int Faction { get; set; }
        public decimal Scale { get; set; }
        [Column("unit_class")]
        public CreatureUnitClasses UnitClass { get; set; }
        public CreatureTypes Type { get; set; }
        public CreatureRanks Rank { get; set; }
        [Column("speed_walk")]
        public decimal SpeedWalk { get; set; }
        [Column("speed_run")]
        public decimal SpeedRun { get; set; }
        public string? AiName { get; set; }
        public int BaseAttackTime { get; set; }
        public int RangeAttackTime { get; set; }
        public decimal BaseVariance { get; set; }
        public decimal RangeVariance { get; set; }
        public decimal HoverHeight { get; set; }
        public bool RegenHealth { get; set; }

        public decimal HealthModifier { get; set; }
        public decimal HealthModifierExtra { get; set; }
        public decimal ManaModifier { get; set; }
        public decimal ManaModifierExtra { get; set; }
        public decimal ArmorModifier { get; set; }
        public decimal DamageModifier { get; set; }
        public decimal ExperienceModifier { get; set; }

        [Column("unit_flags")]
        public long UnitFlags { get; set; }
        [Column("unit_flags2")]
        public long UnitFlags2 { get; set; }
        [Column("unit_flags3")]
        public long UnitFlags3 { get; set; }
        [Column("flags_extra")]
        public long FlagsExtra { get; set; }


        public int VerifiedBuild { get; set; }



    }
}
