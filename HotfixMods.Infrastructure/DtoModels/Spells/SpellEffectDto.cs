using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;

namespace HotfixMods.Infrastructure.DtoModels.Spells
{
    public class SpellEffectDto
    {
        public SpellEffectAuras? EffectAura { get; set; }
        public int? EffectIndex { get; set; }
        public SpellEffects? Effect { get; set; }
        public SpellEffectAttributeFlags? EffectAttributes { get; set; }
        public decimal? EffectBasePointsF { get; set; } // mod value
        public int? EffectAuraPeriod { get; set; } // How often a dot ticks, in ms
        public SpellTargets? ImplicitTarget0 { get; set; }
        public SpellTargets? ImplicitTarget1 { get; set; }
        public int? EffectMiscValue0 { get; set; }
        public int? EffectMiscValue1 { get; set; }
    }
}
