using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using HotfixMods.Infrastructure.DtoModels.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class SpellDto : Dto
    {
        public string? AuraDescription { get; set; }
        public int? CategoryRecoveryTime { get; set; }
        public int? RecoveryTime { get; set; }
        public int? StartRecoveryTime { get; set; }
        public int? CastingTimeIndex { get; set; }
        public int? DurationIndex { get; set; }
        public int? RangeIndex { get; set; }
        public DamageClass? SchoolMask { get; set; }
        public SpellAttributeFlags0? Attributes0 { get; set; }
        public SpellAttributeFlags1? Attributes1 { get; set; }
        public SpellAttributeFlags2? Attributes2 { get; set; }
        public SpellAttributeFlags3? Attributes3 { get; set; }
        public SpellAttributeFlags4? Attributes4 { get; set; }
        public SpellAttributeFlags5? Attributes5 { get; set; }
        public SpellAttributeFlags6? Attributes6 { get; set; }
        public SpellAttributeFlags7?  Attributes7 { get; set; }
        public SpellAttributeFlags8? Attributes8 { get; set; }
        public SpellAttributeFlags9? Attributes9 { get; set; }
        public SpellAttributeFlags10? Attributes10 { get; set; }
        public SpellAttributeFlags11? Attributes11 { get; set; }
        public SpellAttributeFlags12? Attributes12 { get; set; }
        public SpellAttributeFlags13? Attributes13 { get; set; }
        public SpellAttributeFlags14? Attributes14 { get; set; }
        public int? Speed { get; set; }
        public int? SpellIconFileDataId { get; set; }
        public int? PowerCost { get; set; } 
        public decimal? PowerCostPct { get; set; }
        public SpellPowerType? PowerType { get; set; }
        public int? RequiredAuraSpellId { get; set; }
        public int? CumulativeAura { get; set; } // stacks
        public int? ProcCategoryRecovery { get; set; }
        public int? ProcChance { get; set; }
        public int? ProcCharges { get; set; }
        public int? SpellProcsPerMinuteId { get; set; }
        public int? ProcTypeMask0 { get; set; }
        public int? ProcTypeMask1 { get; set; }
        public int? SpellVisualId { get; set; }
        public List<SpellEffectDto> SpellEffects { get; set; }

    }
}
