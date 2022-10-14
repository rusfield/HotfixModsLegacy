using HotfixMods.Core.Enums;
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

        public async Task SaveAsync(SpellDto dto)
        {
            if (dto.SpellEffects.Count > 50)
                throw new Exception("Spell Effects should not exceed 50.");

            var hotfixId = await GetNextHotfixIdAsync();
            dto.InitHotfixes(hotfixId, VerifiedBuild);

            if (dto.IsUpdate)
            {
                var spellEffects = await _mySql.GetAsync<SpellEffect>(c => c.SpellId == dto.Id);
                if (spellEffects.Any())
                    await _mySql.DeleteAsync(spellEffects.ToArray());
            }

            await _mySql.AddOrUpdateAsync(BuildHotfixModsData(dto));
            await _mySql.AddOrUpdateAsync(BuildSpell(dto));
            await _mySql.AddOrUpdateAsync(BuildSpellAuraOptions(dto));
            await _mySql.AddOrUpdateAsync(BuildSpellCooldowns(dto));
            await _mySql.AddOrUpdateAsync(BuildSpellEffects(dto).ToArray());
            await _mySql.AddOrUpdateAsync(BuildSpellMisc(dto));
            await _mySql.AddOrUpdateAsync(BuildSpellName(dto));
            await _mySql.AddOrUpdateAsync(BuildSpellPower(dto));
            await _mySql.AddOrUpdateAsync(BuildSpellXSpellVisual(dto));
            await _mySql.AddOrUpdateAsync(BuildSpellVisual(dto));
            await _mySql.AddOrUpdateAsync(BuildSpellVisualEvent(dto));

            await AddHotfixes(dto.GetHotfixes());

        }

        public async Task<SpellDto> GetNewAsync(Action<string, string, int>? progressCallback = null)
        {
            return new SpellDto()
            {
                Id = await GetNextIdAsync(),
                SpellEffects = new()
            };
        }

        public async Task<SpellDto?> GetByIdAsync(int id, Action<string, string, int>? progressCallback = null)
        {
            var hmData = await _mySql.GetSingleAsync<HotfixModsData>(h => h.RecordId == id && h.VerifiedBuild == VerifiedBuild);
            var spell = await _mySql.GetSingleAsync<Spell>(s => s.Id == id) ?? await _db2.GetSingleAsync<Spell>(s => s.Id == id);
            var spellAuraOptions = await _mySql.GetSingleAsync<SpellAuraOptions>(s => s.SpellId == id) ?? await _db2.GetSingleAsync<SpellAuraOptions>(s => s.SpellId == id);
            var spellCooldowns = await _mySql.GetSingleAsync<SpellCooldowns>(s => s.SpellId == id) ?? await _db2.GetSingleAsync<SpellCooldowns>(s => s.SpellId == id);
            var spellMisc = await _mySql.GetSingleAsync<SpellMisc>(s => s.SpellId == id) ?? await _db2.GetSingleAsync<SpellMisc>(s => s.SpellId == id);
            var spellName = await _mySql.GetSingleAsync<SpellName>(s => s.Id == id) ?? await _db2.GetSingleAsync<SpellName>(s => s.Id == id);
            var spellPower = await _mySql.GetSingleAsync<SpellPower>(s => s.SpellId == id) ?? await _db2.GetSingleAsync<SpellPower>(s => s.SpellId == id);
            var spellXSpellVisual = await _mySql.GetSingleAsync<SpellXSpellVisual>(s => s.SpellId == id) ?? await _db2.GetSingleAsync<SpellXSpellVisual>(s => s.SpellId == id);
            SpellVisual? spellVisual = null;
            SpellVisualEvent? spellVisualEvent = null;

            if(spellXSpellVisual != null)
                spellVisual = await _mySql.GetSingleAsync<SpellVisual>(s => s.Id == spellXSpellVisual.SpellVisualId) ?? await _db2.GetSingleAsync<SpellVisual>(s => s.Id == spellXSpellVisual.SpellVisualId);
            
            if(spellVisual != null)
                spellVisualEvent = await _mySql.GetSingleAsync<SpellVisualEvent>(s => s.SpellVisualId == spellVisual.Id) ?? await _db2.GetSingleAsync<SpellVisualEvent>(s => s.SpellVisualId == spellVisual.Id);

            var spellEffects = await _mySql.GetAsync<SpellEffect>(s => s.SpellId == id);
            if(!spellEffects.Any())
                spellEffects = await _db2.GetAsync<SpellEffect>(s => s.SpellId == id);

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
                    ImplicitTarget1 = spellEffect.ImplicitTarget1,
                    EffectMiscValue0 = spellEffect.EffectMiscValue0,
                    EffectMiscValue1 = spellEffect.EffectMiscValue1
                });
            }

            var result = new SpellDto()
            {
                Id = hmData != null ? id : await GetNextIdAsync(),
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
                CategoryRecoveryTime = spellCooldowns?.CategoryRecoveryTime,
                RecoveryTime = spellCooldowns?.RecoveryTime,
                StartRecoveryTime = spellCooldowns?.StartRecoveryTime,
                ManaCost = spellPower?.ManaCost,
                PowerCostPct = spellPower?.PowerCostPct,
                PowerType = spellPower?.PowerType,
                ProcTypeMask0 = spellAuraOptions?.ProcTypeMask0,
                ProcTypeMask1 = spellAuraOptions?.ProcTypeMask1,
                CumulativeAura = spellAuraOptions?.CumulativeAura,
                ProcCategoryRecovery = spellAuraOptions?.ProcCategoryRecovery,
                ProcChance = spellAuraOptions?.ProcChance,
                ProcCharges = spellAuraOptions?.ProcCharges,
                SpellProcsPerMinuteId = spellAuraOptions?.SpellProcsPerMinuteId,
                RequiredAuraSpellId = spellPower?.RequiredAuraSpellId,
                SpellVisualKitId = spellVisualEvent?.SpellVisualKitId,
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

        public async Task DeleteAsync(int id)
        {
            var hotfixModsData = await _mySql.GetSingleAsync<HotfixModsData>(QueryBuilder(id));
            var spell = await _mySql.GetSingleAsync<Spell>(QueryBuilder(id));
            var spellAuraOptions = await _mySql.GetSingleAsync<SpellAuraOptions>(QueryBuilder(id));
            var spellCooldowns = await _mySql.GetSingleAsync<SpellCooldowns>(QueryBuilder(id));
            var spellMisc = await _mySql.GetSingleAsync<SpellMisc>(QueryBuilder(id));
            var spellName = await _mySql.GetSingleAsync<SpellName>(QueryBuilder(id));
            var spellPower = await _mySql.GetSingleAsync<SpellPower>(QueryBuilder(id));
            var spellVisual = await _mySql.GetSingleAsync<SpellVisual>(QueryBuilder(id));
            var spellVisualEvent = await _mySql.GetSingleAsync<SpellVisualEvent>(QueryBuilder(id));
            var spellXSpellVisual = await _mySql.GetAsync<SpellXSpellVisual>(QueryBuilder(id));
            var spellEffects = await _mySql.GetAsync<SpellEffect>(QueryBuilder(id));


            if (null != spell)
                await _mySql.DeleteAsync(spell);

            if (null != spellAuraOptions)
                await _mySql.DeleteAsync(spellAuraOptions);

            if (null != spellCooldowns)
                await _mySql.DeleteAsync(spellCooldowns);

            if (null != spellMisc)
                await _mySql.DeleteAsync(spellMisc);

            if (null != spellName)
                await _mySql.DeleteAsync(spellName);

            if (null != spellPower)
                await _mySql.DeleteAsync(spellPower);

            if (null != spellVisual)
                await _mySql.DeleteAsync(spellVisual);

            if (null != spellVisualEvent)
                await _mySql.DeleteAsync(spellVisualEvent);

            if (spellXSpellVisual.Any())
                await _mySql.DeleteAsync(spellXSpellVisual.ToArray());

            if (spellEffects.Any())
                await _mySql.DeleteAsync(spellEffects.ToArray());

            var hotfixData = await _mySql.GetAsync<HotfixData>(h => h.UniqueId == id && h.VerifiedBuild == VerifiedBuild);
            if (hotfixData != null && hotfixData.Count() > 0)
            {
                foreach (var hotfix in hotfixData)
                {
                    hotfix.Status = HotfixStatuses.INVALID;
                }
                await _mySql.AddOrUpdateAsync(hotfixData.ToArray());
            }

            if (null != hotfixModsData)
                await _mySql.DeleteAsync(hotfixModsData);
        }
    }
}
