using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
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
            var progress = LoadingHelper.GetLoaderFunc(11);

            var spell = await GetSingleAsync<Spell>(callback, progress, new DbParameter(nameof(Spell.Id), id));

            if (null == spell)
            {
                callback.Invoke(LoadingHelper.Loading, "Not found.", 100);
                return null;
            }

            var result = new SpellDto()
            {
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(callback, progress, id),
                SpellMisc = await GetSingleAsync<SpellMisc>(callback, progress, new DbParameter(nameof(SpellMisc.SpellId), id)) ?? new(),
                SpellName = await GetSingleAsync<SpellName>(callback, progress, new DbParameter(nameof(SpellName.Id), id)) ?? new(),
                SpellAuraOptions = await GetSingleAsync<SpellAuraOptions>(callback, progress, new DbParameter(nameof(SpellAuraOptions.SpellId), id)),
                SpellPower = await GetSingleAsync<SpellPower>(callback, progress, new DbParameter(nameof(SpellPower.SpellId), id)),
                SpellCooldowns = await GetSingleAsync<SpellCooldowns>(callback, progress, new DbParameter(nameof(SpellCooldowns.SpellId), id)),

                Spell = spell,
                IsUpdate = true
            };

            var spellEffects = await GetAsync<SpellEffect>(callback, progress, new DbParameter(nameof(SpellEffect.SpellId), id));
            spellEffects.ForEach(s =>
            {
                result.EffectGroups.Add(new SpellDto.EffectGroup()
                {
                    SpellEffect = s
                });
            });

            var spellXSpellVisuals = await GetAsync<SpellXSpellVisual>(callback, progress, new DbParameter(nameof(SpellXSpellVisual.SpellId), id));

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(SpellVisual)} and {nameof(SpellVisualEvent)}", progress());
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
            var progress = LoadingHelper.GetLoaderFunc(12);

            callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
            if (dto.IsUpdate)
            {
                await DeleteAsync(dto.Spell.Id);
            }

            callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
            await SetIdAndVerifiedBuild(dto);

            await SaveAsync(callback, progress, dto.HotfixModsEntity);
            await SaveAsync(callback, progress, dto.Spell);
            await SaveAsync(callback, progress, dto.SpellAuraOptions);
            await SaveAsync(callback, progress, dto.SpellCooldowns);
            await SaveAsync(callback, progress, dto.SpellMisc);
            await SaveAsync(callback, progress, dto.SpellName);
            await SaveAsync(callback, progress, dto.SpellPower);

            callback.Invoke(LoadingHelper.Saving, $"Saving {nameof(SpellEffect)}", progress());
            await dto.EffectGroups.ForEachAsync(async e =>
            {
                await SaveAsync(e.SpellEffect);
            });

            callback.Invoke(LoadingHelper.Saving, $"Saving {nameof(SpellXSpellVisual)}, {nameof(SpellVisual)} and {nameof(SpellVisualEvent)}", progress());
            await dto.VisualGroups.ForEachAsync(async v =>
            {
                await SaveAsync(v.SpellXSpellVisual);
                await SaveAsync(v.SpellVisual);
                await SaveAsync(v.SpellVisualEvent);
            });

            callback.Invoke(LoadingHelper.Saving, "Saving successful", 100);
            dto.IsUpdate = true;
            return true;
        }

        public async Task<bool> DeleteAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(10);

            var dto = await GetByIdAsync(id);
            if (null == dto)
            {
                callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                return false;
            }

            callback.Invoke(LoadingHelper.Deleting, $"Deleting {nameof(SpellXSpellVisual)}, {nameof(SpellVisual)} and {nameof(SpellVisualEvent)}", progress());
            await dto.VisualGroups.ForEachAsync(async v =>
            {
                await DeleteAsync(v.SpellVisualEvent);
                await DeleteAsync(v.SpellVisualEvent);
                await DeleteAsync(v.SpellXSpellVisual);
            });

            callback.Invoke(LoadingHelper.Deleting, $"Deleting {nameof(SpellEffect)}", progress());
            await dto.EffectGroups.ForEachAsync(async e =>
            {
                await DeleteAsync(e.SpellEffect);
            });
            await DeleteAsync(callback, progress, dto.SpellPower);
            await DeleteAsync(callback, progress, dto.SpellName);
            await DeleteAsync(callback, progress, dto.SpellMisc);
            await DeleteAsync(callback, progress, dto.SpellCooldowns);
            await DeleteAsync(callback, progress, dto.SpellAuraOptions);
            await DeleteAsync(callback, progress, dto.Spell);
            await DeleteAsync(callback, progress, dto.HotfixModsEntity);

            callback.Invoke(LoadingHelper.Deleting, "Delete successful", 100);
            return true;
        }
    }
}
