using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotfixMods.Core.Models
{
    public class CreatureTemplate : IWorldSchema
    {
        [Key]
        public int Entry { get; set; }
        public string? Name { get; set; }
        public string? SubName { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int Faction { get; set; }
        public double Scale { get; set; }
        [Column("unit_class")]
        public CreatureUnitClasses UnitClass { get; set; }
        public CreatureTypes Type { get; set; }
        public CreatureRanks Rank { get; set; }
        [Column("speed_walk")]
        public double SpeedWalk { get; set; }
        [Column("speed_run")]
        public double SpeedRun { get; set; }
        public string? AiName { get; set; }
        public int BaseAttackTime { get; set; }
        public int RangeAttackTime { get; set; }
        public double BaseVariance { get; set; }
        public double RangeVariance { get; set; }
        public double HoverHeight { get; set; }
        public bool RegenHealth { get; set; }

        public double HealthModifier { get; set; }
        public double HealthModifierExtra { get; set; }
        public double ManaModifier { get; set; }
        public double ManaModifierExtra { get; set; }
        public double ArmorModifier { get; set; }
        public double DamageModifier { get; set; }
        public double ExperienceModifier { get; set; }

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
