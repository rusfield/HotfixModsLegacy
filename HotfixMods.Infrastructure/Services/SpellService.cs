using HotfixMods.Core.Models;
using HotfixMods.Core.Providers;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.DtoModels.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellService : Service
    {
        public SpellService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider) { }

        public async Task SaveAsync(SpellDto spellDto)
        {
            if (spellDto.SpellEffects.Count > 50)
                throw new Exception("Spell Effects should not exceed 50.");

            var hotfixId = await GetNextHotfixIdAsync();
            spellDto.InitHotfixes(hotfixId, VerifiedBuild);

            if (spellDto.IsUpdate)
            {
                var spellEffects = await _mySql.GetAsync<SpellEffect>(c => c.SpellId == spellDto.Id);
                if (spellEffects.Any())
                    await _mySql.DeleteAsync(spellEffects.ToArray());
            }

            await _mySql.AddOrUpdateAsync(BuildHotfixModsData(spellDto));
            await _mySql.AddOrUpdateAsync(BuildSpell(spellDto));
            await _mySql.AddOrUpdateAsync(BuildSpellAuraOptions(spellDto));
            await _mySql.AddOrUpdateAsync(BuildSpellCooldowns(spellDto));
            await _mySql.AddOrUpdateAsync(BuildSpellEffects(spellDto).ToArray());
            await _mySql.AddOrUpdateAsync(BuildSpellMisc(spellDto));
            await _mySql.AddOrUpdateAsync(BuildSpellName(spellDto));
            await _mySql.AddOrUpdateAsync(BuildSpellPower(spellDto));
            await _mySql.AddOrUpdateAsync(BuildSpellXSpellVisual(spellDto));
            await _mySql.AddOrUpdateAsync(BuildSpellVisual(spellDto));

            await AddHotfixes(spellDto.GetHotfixes());

        }

        public async Task<SpellDto> GetNewSpellAsync(Action<string, string, int>? progressCallback = null)
        {
            return new SpellDto()
            {
                Id = await GetNextIdAsync(),
                SpellEffects = new()
            };
        }

        public async Task<SpellDto?> GetSpellByIdAsync(int spellId, Action<string, string, int>? progressCallback = null)
        {
            var hmData = await _mySql.GetSingleAsync<HotfixModsData>(h => h.RecordId == spellId && h.VerifiedBuild == VerifiedBuild);
            var spell = await _mySql.GetSingleAsync<Spell>(s => s.Id == spellId) ?? await _db2.GetSingleAsync<Spell>(s => s.Id == spellId);
            var spellAuraOptions = await _mySql.GetSingleAsync<SpellAuraOptions>(s => s.SpellId == spellId) ?? await _db2.GetSingleAsync<SpellAuraOptions>(s => s.SpellId == spellId);
            var spellCooldowns = await _mySql.GetSingleAsync<SpellCooldowns>(s => s.SpellId == spellId) ?? await _db2.GetSingleAsync<SpellCooldowns>(s => s.SpellId == spellId);
            var spellMisc = await _mySql.GetSingleAsync<SpellMisc>(s => s.SpellId == spellId) ?? await _db2.GetSingleAsync<SpellMisc>(s => s.SpellId == spellId);
            var spellName = await _mySql.GetSingleAsync<SpellName>(s => s.Id == spellId) ?? await _db2.GetSingleAsync<SpellName>(s => s.Id == spellId);
            var spellPower = await _mySql.GetSingleAsync<SpellPower>(s => s.SpellId == spellId) ?? await _db2.GetSingleAsync<SpellPower>(s => s.SpellId == spellId);
            var spellXSpellVisual = await _mySql.GetSingleAsync<SpellXSpellVisual>(s => s.SpellId == spellId) ?? await _db2.GetSingleAsync<SpellXSpellVisual>(s => s.SpellId == spellId);
            var spellVisual = await _mySql.GetSingleAsync<SpellVisual>(s => s.Id == spellXSpellVisual.SpellVisualId) ?? await _db2.GetSingleAsync<SpellVisual>(s => s.Id == spellXSpellVisual.SpellVisualId);

            var spellEffects = await _mySql.GetAsync<SpellEffect>(s => s.SpellId == spellId);
            if(!spellEffects.Any())
                spellEffects = await _db2.GetAsync<SpellEffect>(s => s.SpellId == spellId);

            var spellEffectDtos = new List<SpellEffectDto>();
            foreach(var spellEffect in spellEffects)
            {
                spellEffectDtos.Add(new SpellEffectDto()
                {
                    Effect = spellEffect.Effect,
                    EffectAttributes = spellEffect.EffectAttributes,
                    EffectAura = spellEffect.EffectAura,
                    EffectAuraPeriod = spellEffect.EffectAuraPeriod,
                    EffectBasePointsF = spellEffect.EffectBasePointsF,
                    EffectIndex = spellEffect.EffectIndex,
                    ImplicitTarget0 = spellEffect.ImplicitTarget0,
                    ImplicitTarget1 = spellEffect.ImplicitTarget1
                });
            }

            var result = new SpellDto()
            {
                Id = hmData != null ? spellId : await GetNextIdAsync(),
                Attributes0 = spellMisc.Attributes0,
                Attributes1 = spellMisc.Attributes1,
                Attributes2 = spellMisc.Attributes2,
                Attributes3 = spellMisc.Attributes3,
                Attributes4 = spellMisc.Attributes4,
                Attributes5 = spellMisc.Attributes5,
                Attributes6 = spellMisc.Attributes6,
                Attributes7 = spellMisc.Attributes7,
                Attributes8 = spellMisc.Attributes8,
                Attributes9 = spellMisc.Attributes9,
                Attributes10 = spellMisc.Attributes10,
                Attributes11 = spellMisc.Attributes11,
                Attributes12 = spellMisc.Attributes12,
                Attributes13 = spellMisc.Attributes13,
                Attributes14 = spellMisc.Attributes14,
                AuraDescription = spell.AuraDescription,
                SpellEffects = spellEffectDtos,
                CategoryRecoveryTime = spellCooldowns.CategoryRecoveryTime,
                RecoveryTime = spellCooldowns.RecoveryTime,
                StartRecoveryTime = spellCooldowns.StartRecoveryTime,
                ManaCost = spellPower.ManaCost,
                PowerCostPct = spellPower.PowerCostPct,
                PowerType = spellPower.PowerType,
                ProcTypeMask0 = spellAuraOptions.ProcTypeMask0,
                ProcTypeMask1 = spellAuraOptions.ProcTypeMask1,
                CumulativeAura = spellAuraOptions.CumulativeAura,
                ProcCategoryRecovery = spellAuraOptions.ProcCategoryRecovery,
                ProcChance = spellAuraOptions.ProcChance,
                ProcCharges = spellAuraOptions.ProcCharges,
                SpellProcsPerMinuteId = spellAuraOptions.SpellProcsPerMinuteId,
                RequiredAuraSpellId = spellPower.RequiredAuraSpellId,
                SpellVisualId = spellXSpellVisual.SpellVisualId,
                CastingTimeIndex = spellMisc.CastingTimeIndex,
                DurationIndex= spellMisc.DurationIndex,
                RangeIndex = spellMisc.RangeIndex,
                SchoolMask = spellMisc.SchoolMask,
                Speed = spellMisc.Speed,
                SpellIconFileDataId = spellMisc.SpellIconFileDataId,

                IsUpdate = hmData != null,
                HotfixModsName = hmData?.Name,
                HotfixModsComment = hmData?.Comment
            };

            return result;
        }

        public async Task<List<DashboardModel>> GetDashboardAsync()
        {
            var hotfixModsData = await _mySql.GetAsync<HotfixModsData>(c => c.VerifiedBuild == VerifiedBuild);
            var result = new List<DashboardModel>();
            foreach (var data in hotfixModsData)
            {
                result.Add(new DashboardModel()
                {
                    Id = data.RecordId,
                    Name = data.Name,
                    Comment = data.Comment,
                    AvatarUrl = "TODO"
                });
            }
            // Newest on top
            result.Reverse();
            return result;
        }

        public async Task DeleteAsync(int spellId)
        {

        }
    }
}
