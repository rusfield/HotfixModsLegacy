using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DefaultModels;
using HotfixMods.Infrastructure.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellService
    {
        Spell BuildSpell(SpellDto spellDto)
        {
            spellDto.AddHotfix(spellDto.Id, TableHashes.SPELL, HotfixStatuses.VALID);
            return new ()
            {
                Id = spellDto.Id,
                VerifiedBuild = VerifiedBuild,

                AuraDescription = spellDto.AuraDescription ?? Default.Spell.AuraDescription,
                NameSubtext = spellDto.HotfixModsName ?? Default.Spell.NameSubtext
            };
        }

        SpellAuraOptions BuildSpellAuraOptions(SpellDto spellDto)
        {
            spellDto.AddHotfix(spellDto.Id, TableHashes.SPELL_AURA_OPTIONS, HotfixStatuses.VALID);
            return new ()
            {
                Id = spellDto.Id,
                SpellId = spellDto.Id,
                VerifiedBuild = VerifiedBuild,

                CumulativeAura = spellDto.CumulativeAura ?? Default.SpellAuraOptions.CumulativeAura,
                SpellProcsPerMinuteId = spellDto.SpellProcsPerMinuteId ?? Default.SpellAuraOptions.SpellProcsPerMinuteId,
                ProcCategoryRecovery = spellDto.ProcCategoryRecovery ?? Default.SpellAuraOptions.ProcCategoryRecovery,
                ProcChance = spellDto.ProcChance ?? Default.SpellAuraOptions.ProcChance,
                ProcCharges = spellDto.ProcCharges ?? Default.SpellAuraOptions.ProcCharges,
                ProcTypeMask0 = spellDto.ProcTypeMask0 ?? Default.SpellAuraOptions.ProcTypeMask0,
                ProcTypeMask1 = spellDto.ProcTypeMask1 ?? Default.SpellAuraOptions.ProcTypeMask1
            };
        }

        SpellCooldowns BuildSpellCooldowns(SpellDto spellDto)
        {
            spellDto.AddHotfix(spellDto.Id, TableHashes.SPELL_COOLDOWNS, HotfixStatuses.VALID);
            return new ()
            {
                CategoryRecoveryTime = spellDto.CategoryRecoveryTime ?? Default.SpellCooldowns.CategoryRecoveryTime,
                RecoveryTime = spellDto.RecoveryTime ?? Default.SpellCooldowns.RecoveryTime,
                StartRecoveryTime = spellDto.StartRecoveryTime ?? Default.SpellCooldowns.StartRecoveryTime,

                Id = -1,
                VerifiedBuild = -1
            };
        }

        List<SpellEffect> BuildSpellEffects(SpellDto spellDto)
        {
            if(spellDto.SpellEffects.Count > 50)
            {
                throw new Exception("Spell effects should not exceed 50.");
            }
            var result = new List<SpellEffect>();
            foreach(var spellEffect in spellDto.SpellEffects)
            {
                spellDto.AddHotfix(spellDto.Id + spellEffect.EffectIndex, TableHashes.SPELL_EFFECT, HotfixStatuses.VALID);
                result.Add(new()
                {
                    Id = spellDto.Id + spellEffect.EffectIndex,
                    SpellId = spellDto.Id,
                    EffectIndex = spellEffect.EffectIndex,
                    VerifiedBuild = VerifiedBuild,

                    Effect = spellEffect.Effect ?? Default.SpellEffect.Effect,
                    EffectAttributes = spellEffect.EffectAttributes ?? Default.SpellEffect.EffectAttributes,
                    EffectAura = spellEffect.EffectAura ?? Default.SpellEffect.EffectAura,
                    EffectAuraPeriod = spellEffect.EffectAuraPeriod ?? Default.SpellEffect.EffectAuraPeriod,
                    EffectBasePointsF = spellEffect.EffectBasePointsF ?? Default.SpellEffect.EffectBasePointsF,
                    ImplicitTarget0 = spellEffect.ImplicitTarget0 ?? Default.SpellEffect.ImplicitTarget0,
                    ImplicitTarget1 = spellEffect.ImplicitTarget1 ?? Default.SpellEffect.ImplicitTarget1,
                    
                    PvpMultiplier = Default.SpellEffect.PvpMultiplier,
                    Variance = Default.SpellEffect.Variance,
                    EffectChainAmplitude = Default.SpellEffect.EffectChainAmplitude
                });
            }
            return result;
        }

        SpellMisc BuildSpellMisc(SpellDto spellDto)
        {
            spellDto.AddHotfix(spellDto.Id, TableHashes.SPELL_MISC, HotfixStatuses.VALID);
            return new()
            {
                Id = spellDto.Id,
                VerifiedBuild = VerifiedBuild,
                SpellId = spellDto.Id,

                Attributes0 = spellDto.Attributes0 ?? Default.SpellMisc.Attributes0,
                Attributes1 = spellDto.Attributes1 ?? Default.SpellMisc.Attributes1,
                Attributes2 = spellDto.Attributes2 ?? Default.SpellMisc.Attributes2,
                Attributes3 = spellDto.Attributes3 ?? Default.SpellMisc.Attributes3,
                Attributes4 = spellDto.Attributes4 ?? Default.SpellMisc.Attributes4,
                Attributes5 = spellDto.Attributes5 ?? Default.SpellMisc.Attributes5,
                Attributes6 = spellDto.Attributes6 ?? Default.SpellMisc.Attributes6,
                Attributes7 = spellDto.Attributes7 ?? Default.SpellMisc.Attributes7,
                Attributes8 = spellDto.Attributes8 ?? Default.SpellMisc.Attributes8,
                Attributes9 = spellDto.Attributes9 ?? Default.SpellMisc.Attributes9,
                Attributes10 = spellDto.Attributes10 ?? Default.SpellMisc.Attributes10,
                Attributes11 = spellDto.Attributes11 ?? Default.SpellMisc.Attributes11,
                Attributes12 = spellDto.Attributes12 ?? Default.SpellMisc.Attributes12,
                Attributes13 = spellDto.Attributes13 ?? Default.SpellMisc.Attributes13,
                Attributes14 = spellDto.Attributes14 ?? Default.SpellMisc.Attributes14,
                CastingTimeIndex = spellDto.CastingTimeIndex ?? Default.SpellMisc.CastingTimeIndex,
                DurationIndex = spellDto.DurationIndex ?? Default.SpellMisc.DurationIndex,
                RangeIndex = spellDto.RangeIndex ?? Default.SpellMisc.RangeIndex,
                SchoolMask = spellDto.SchoolMask ?? Default.SpellMisc.SchoolMask,
                Speed = spellDto.Speed ?? Default.SpellMisc.Speed,
                SpellIconFileDataId = spellDto.SpellIconFileDataId ?? Default.SpellMisc.SpellIconFileDataId
            };
        }

        SpellName BuildSpellName(SpellDto spellDto)
        {
            spellDto.AddHotfix(spellDto.Id, TableHashes.SPELL_NAME, HotfixStatuses.VALID);
            return new()
            {
                Id = spellDto.Id,
                VerifiedBuild = VerifiedBuild,

                Name = spellDto.HotfixModsName ?? Default.SpellName.Name
            };
        }

        SpellPower BuildSpellPower(SpellDto spellDto)
        {
            spellDto.AddHotfix(spellDto.Id, TableHashes.SPELL_POWER, HotfixStatuses.VALID);
            return new()
            {
                Id = spellDto.Id,
                SpellId = spellDto.Id,
                VerifiedBuild = VerifiedBuild,

                ManaCost = spellDto.ManaCost ?? Default.SpellPower.ManaCost,
                PowerCostPct = spellDto.PowerCostPct ?? Default.SpellPower.PowerCostPct,
                PowerType = spellDto.PowerType ?? Default.SpellPower.PowerType,
                RequiredAuraSpellId = spellDto.RequiredAuraSpellId ?? Default.SpellPower.RequiredAuraSpellId
            };
        }

        SpellXSpellVisual BuildSpellXSpellVisual(SpellDto spellDto)
        {
            spellDto.AddHotfix(spellDto.Id, TableHashes.SPELL_X_SPELL_VISUAL, HotfixStatuses.VALID);
            return new()
            {
                Id = spellDto.Id,
                SpellId = spellDto.Id,
                VerifiedBuild = VerifiedBuild,

                SpellVisualId = spellDto.SpellVisualId ?? Default.SpellXSpellVisual.SpellVisualId,

                Probability = Default.SpellXSpellVisual.Probability
            };
        }

        // TODO:
        SpellVisual BuildSpellVisual(SpellDto spellDto)
        {
            spellDto.AddHotfix(spellDto.Id, TableHashes.SPELL_VISUAL, HotfixStatuses.VALID);
            return new()
            {
                Id= spellDto.Id,
                VerifiedBuild = VerifiedBuild
            };
        }
    }
}
