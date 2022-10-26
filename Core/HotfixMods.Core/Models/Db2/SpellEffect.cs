using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellEffect
    {
        public int Id { get; set; }
        public short EffectAura { get; set; }
        public int DifficultyId { get; set; }
        public int EffectIndex { get; set; }
        public uint Effect { get; set; }
        public decimal EffectAmplitude { get; set; }
        public int EffectAttributes { get; set; }
        public int EffectAuraPeriod { get; set; }
        public decimal EffectBonusCoefficient { get; set; }
        public decimal EffectChainAmplitude { get; set; }
        public int EffectChainTargets { get; set; }
        public int EffectItemType { get; set; }
        public int EffectMechanic { get; set; }
        public decimal EffectPointsPerResource { get; set; }
        public decimal EffectPos_facing { get; set; }
        public decimal EffectRealPointsPerLevel { get; set; }
        public int EffectTriggerSpell { get; set; }
        public decimal BonusCoefficientFromAP { get; set; }
        public decimal PvpMultiplier { get; set; }
        public decimal Coefficient { get; set; }
        public decimal Variance { get; set; }
        public decimal ResourceCoefficient { get; set; }
        public decimal GroupSizeBasePointsCoefficient { get; set; }
        public decimal EffectBasePointsF { get; set; }
        public int ScalingClass { get; set; }
        public int EffectMiscValue1 { get; set; }
        public int EffectMiscValue2 { get; set; }
        public uint EffectRadiusIndex1 { get; set; }
        public uint EffectRadiusIndex2 { get; set; }
        public int EffectSpellClassMask1 { get; set; }
        public int EffectSpellClassMask2 { get; set; }
        public int EffectSpellClassMask3 { get; set; }
        public int EffectSpellClassMask4 { get; set; }
        public short ImplicitTarget1 { get; set; }
        public short ImplicitTarget2 { get; set; }
        public int SpellId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
