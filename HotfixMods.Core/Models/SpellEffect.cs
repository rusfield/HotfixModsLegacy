using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SpellEffect : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public SpellEffectAuras EffectAura { get; set; }
        public int EffectIndex { get; set; }
        public SpellEffects Effect { get; set; }
        public int SpellId { get; set; }
        public decimal Variance { get; set; }
        public decimal PvpMultiplier { get; set; }
        public decimal EffectChainAmplitude { get; set; }
        public int EffectAuraPeriod { get; set; }
        [Column("EffectBasePoints")]
        public decimal EffectBasePointsF { get; set; }
        [Column("ImplicitTarget1")]
        public SpellTargets ImplicitTarget0 { get; set; }
        [Column("ImplicitTarget2")]
        public SpellTargets ImplicitTarget1 { get; set; }
        public SpellEffectAttributeFlags EffectAttributes { get; set; }
        [Column("EffectMiscValue1")]
        public int EffectMiscValue0 { get; set; }
        [Column("EffectMiscValue2")]
        public int EffectMiscValue1 { get; set; }

        public int VerifiedBuild { get; set; }
    }
}
