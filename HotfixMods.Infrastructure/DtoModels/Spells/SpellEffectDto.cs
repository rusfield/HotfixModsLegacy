using HotfixMods.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels.Spells
{
    public class SpellEffectDto
    {
        public SpellEffectAuras EffectAura { get; set; }
        public int EffectIndex { get; set; }
        public SpellEffects Effect { get; set; }
        public decimal Variance { get; set; }
        public decimal PvpMultiplier { get; set; }
        public decimal EffectChainAmplitude { get; set; }
        public SpellTargets ImplicitTarget0 { get; set; }
        public SpellTargets ImplicitTarget1 { get; set; }
    }
}
