using HotfixMods.Core.Models;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
namespace HotfixMods.Infrastructure.DefaultModels
{
    public partial class Default
    {
        public static readonly Spell Spell = new()
        {
            AuraDescription = "",
            NameSubtext = "New Spell",

            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly SpellCooldowns SpellCooldowns = new()
        {
            CategoryRecoveryTime = 0,
            RecoveryTime = 0,
            StartRecoveryTime = 0,

            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly SpellEffect SpellEFfect = new()
        {
            Effect = SpellEffects.NONE,
            EffectAura = SpellEffectAuras.NONE,
            EffectIndex = 0,
            ImplicitTarget0 = SpellTargets.NONE,
            ImplicitTarget1 = SpellTargets.NONE,
            SpellId = 0,
            EffectBasePointsF = 0,
            EffectAuraPeriod = 0,
            EffectAttributes = SpellEffectAttributeFlags.NONE,

            EffectChainAmplitude = 1,
            PvpMultiplier = 1,
            Variance = 1,

            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly SpellMisc SpellMisc = new()
        {
            Attributes0 = SpellAttributeFlags0.NONE,
            Attributes1 = SpellAttributeFlags1.NONE,
            Attributes2 = SpellAttributeFlags2.NONE,
            Attributes3 = SpellAttributeFlags3.NONE,
            Attributes4 = SpellAttributeFlags4.NONE,
            Attributes5 = SpellAttributeFlags5.NONE,
            Attributes6 = SpellAttributeFlags6.NONE,
            Attributes7 = SpellAttributeFlags7.NONE,
            Attributes8 = SpellAttributeFlags8.NONE,
            Attributes9 = SpellAttributeFlags9.NONE,
            Attributes10 = SpellAttributeFlags10.NONE,
            Attributes11 = SpellAttributeFlags11.NONE,
            Attributes12 = SpellAttributeFlags12.NONE,
            Attributes13 = SpellAttributeFlags13.NONE,
            Attributes14 = SpellAttributeFlags14.NONE,
            SpellId = 0,
            CastingTimeIndex = 0,
            DurationIndex = 0,
            RangeIndex = 0,
            SchoolMask = DamageClass.NONE,
            Speed = 0,
            SpellIconFileDataId = 0,

            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly SpellName SpellName = new()
        {
            Name = "New Spell",

            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly SpellPower SpellPower = new()
        {
            ManaCost = 0,
            PowerCostPct = 0,
            PowerType = SpellPowerType.MANA,
            RequiredAuraSpellId = 0,
            SpellId = 0,

            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly SpellXSpellVisual SpellXSpellVisual = new()
        {
            SpellId = 0,
            SpellVisualId = 0,

            Probability = 1,

            Id = -1,
            VerifiedBuild = -1
        };
    }
}
