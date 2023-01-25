﻿using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellEffect
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public short EffectAura { get; set; } = 0;
        public int DifficultyId { get; set; } = 0;
        public int EffectIndex { get; set; } = 0;
        public uint Effect { get; set; } = 0;
        public decimal EffectAmplitude { get; set; } = 0;
        public int EffectAttributes { get; set; } = 0;
        public int EffectAuraPeriod { get; set; } = 0;
        public decimal EffectBonusCoefficient { get; set; } = 0;
        public decimal EffectChainAmplitude { get; set; } = 1;
        public int EffectChainTargets { get; set; } = 0;
        public int EffectItemType { get; set; } = 0;
        public int EffectMechanic { get; set; } = 0;
        public decimal EffectPointsPerResource { get; set; } = 0;
        public decimal EffectPos_facing { get; set; } = 0;
        public decimal EffectRealPointsPerLevel { get; set; } = 0;
        public int EffectTriggerSpell { get; set; } = 0;
        public decimal BonusCoefficientFromAP { get; set; } = 0;
        public decimal PvpMultiplier { get; set; } = 1;
        public decimal Coefficient { get; set; } = 0; 
        public decimal Variance { get; set; } = 0;
        public decimal ResourceCoefficient { get; set; } = 0;
        public decimal GroupSizeBasePointsCoefficient { get; set; } = 0;
        public decimal EffectBasePointsF { get; set; } = 0;
        public int ScalingClass { get; set; } = 0;
        public int EffectMiscValue1 { get; set; } = 0;
        public int EffectMiscValue2 { get; set; } = 0;
        public uint EffectRadiusIndex1 { get; set; } = 0;
        public uint EffectRadiusIndex2 { get; set; } = 0;
        public int EffectSpellClassMask1 { get; set; } = 0;
        public int EffectSpellClassMask2 { get; set; } = 0;
        public int EffectSpellClassMask3 { get; set; } = 0;
        public int EffectSpellClassMask4 { get; set; } = 0;
        public short ImplicitTarget1 { get; set; } = 0;
        public short ImplicitTarget2 { get; set; } = 0;
        [ParentIndexField]
        public int SpellId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
