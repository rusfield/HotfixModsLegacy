using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Helpers;
using System.Text.Json;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellService : ServiceBase
    {
        public SpellService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public SpellDto GetNew(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            callback.Invoke(LoadingHelper.Loading, "Returning new template", 100);
            return new();
        }

        public async Task<SpellDto?> GetByIdAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var increaseProgress = LoadingHelper.GetLoaderFunc(9);

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(Spell)}", increaseProgress());
            var spell = await GetSingleAsync<Spell>(new DbParameter(nameof(Spell.Id), id));

            if (null == spell)
            {
                callback.Invoke(LoadingHelper.Loading, "Not found.", 100);
                return null;
            }

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(HotfixModsEntity)}", increaseProgress());
            var result = new SpellDto()
            {
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(id),
                Spell = spell,
                IsUpdate = true
            };

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(SpellMisc)}", increaseProgress());
            var spellMisc = await GetSingleAsync<SpellMisc>(new DbParameter(nameof(SpellMisc.SpellId), id));
            if (null == spellMisc)
            {
                callback.Invoke(LoadingHelper.Loading, $"{nameof(SpellMisc)} not found. Initializing new.", increaseProgress());
                spellMisc = new();
            }
            result.SpellMisc = spellMisc;

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(SpellName)}", increaseProgress());
            var spellName = await GetSingleAsync<SpellName>(new DbParameter(nameof(SpellName.Id), id));
            if (null == spellName)
            {
                callback.Invoke(LoadingHelper.Loading, $"{nameof(SpellName)} not found. Initializing new.", increaseProgress());
                spellName = new();
            }
            result.SpellName = spellName;

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(SpellAuraOptions)}", increaseProgress());
            result.SpellAuraOptions = await GetSingleAsync<SpellAuraOptions>(new DbParameter(nameof(SpellAuraOptions.SpellId), id));

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(SpellPower)}", increaseProgress());
            result.SpellPower = await GetSingleAsync<SpellPower>(new DbParameter(nameof(SpellPower.SpellId), id));

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(SpellCooldowns)}", increaseProgress());
            result.SpellCooldowns = await GetSingleAsync<SpellCooldowns>(new DbParameter(nameof(SpellCooldowns.SpellId), id));

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(SpellEffect)}", increaseProgress());
            var spellEffects = await GetAsync<SpellEffect>(new DbParameter(nameof(SpellEffect.SpellId), id));
            spellEffects.ForEach(s =>
            {
                result.EffectGroups.Add(new SpellDto.EffectGroup()
                {
                    SpellEffect = s
                });
            });

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(SpellXSpellVisual)}", increaseProgress());
            var spellXSpellVisuals = await GetAsync<SpellXSpellVisual>(new DbParameter(nameof(SpellXSpellVisual.SpellId), id));
            if(spellXSpellVisuals.Count > 0)
                callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(SpellVisual)} and {nameof(SpellVisualEvent)}", increaseProgress());

            await spellXSpellVisuals.ForEachAsync(async sxsv =>
            {
                var spellVisualEvents = await GetAsync<SpellVisualEvent>(new DbParameter(nameof(SpellVisualEvent.SpellVisualId), sxsv.SpellVisualId));
                await spellVisualEvents.ForEachAsync(async sve =>
                {
                    var spellVisuals = await GetAsync<SpellVisual>(new DbParameter(nameof(SpellVisual.Id), sve.SpellVisualId));
                    spellVisuals.ForEach(sv =>
                    {
                        var newSpellVisualEvent = JsonSerializer.Deserialize<SpellVisualEvent>(JsonSerializer.Serialize(sve))!;
                        var newSpellXSpellVisual = JsonSerializer.Deserialize<SpellXSpellVisual>(JsonSerializer.Serialize(sve))!;
                        result.VisualGroups.Add(new()
                        {
                            SpellVisual = sv,
                            SpellVisualEvent = newSpellVisualEvent,
                            SpellXSpellVisual = newSpellXSpellVisual
                        });
                    });
                });
            });

            callback.Invoke(LoadingHelper.Loading, $"Loading successful.", 100);
            return result;
        }

        public async Task<bool> SaveAsync(SpellDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            await SetIdAndVerifiedBuild(dto);

            await SaveAsync(dto.HotfixModsEntity);
            await SaveAsync(dto.Spell);
            await SaveAsync(dto.SpellAuraOptions);
            await SaveAsync(dto.SpellCooldowns);
            await SaveAsync(dto.SpellMisc);
            await SaveAsync(dto.SpellName);
            await SaveAsync(dto.SpellPower);
            await dto.EffectGroups.ForEachAsync(async e =>
            {
                await SaveAsync(e.SpellEffect);
            });
            await dto.VisualGroups.ForEachAsync(async v =>
            {
                await SaveAsync(v.SpellXSpellVisual);
                await SaveAsync(v.SpellVisual);
                await SaveAsync(v.SpellVisualEvent);
            });


            dto.IsUpdate = true;
            return true;
        }

        public async Task<bool> DeleteAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var dto = await GetByIdAsync(id);
            if (null == dto)
            {
                return false;
            }

            await dto.VisualGroups.ForEachAsync(async v =>
            {
                await DeleteAsync(v.SpellVisualEvent);
                await DeleteAsync(v.SpellVisualEvent);
                await DeleteAsync(v.SpellXSpellVisual);
            });
            await dto.EffectGroups.ForEachAsync(async e =>
            {
                await DeleteAsync(e.SpellEffect);
            });
            await DeleteAsync(dto.SpellPower);
            await DeleteAsync(dto.SpellName);
            await DeleteAsync(dto.SpellMisc);
            await DeleteAsync(dto.SpellCooldowns);
            await DeleteAsync(dto.SpellAuraOptions);
            await DeleteAsync(dto.Spell);
            await DeleteAsync(dto.HotfixModsEntity);

            return true;
        }
    }
}
