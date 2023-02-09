using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;
using System.Text.Json;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellService : ServiceBase
    {
        public SpellService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IExceptionHandler exceptionHandler, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.SpellSettings.FromId;
            ToId = appConfig.SpellSettings.ToId;
            VerifiedBuild = appConfig.SpellSettings.VerifiedBuild;
        }

        public SpellDto GetNew(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            callback.Invoke(LoadingHelper.Loading, "Returning new template", 100);
            return new()
            {
                HotfixModsEntity = new()
                {
                    Name = "New Spell"
                }
            };
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            var dtos = await GetAsync<HotfixModsEntity>(new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
            var results = new List<DashboardModel>();
            foreach (var dto in dtos)
            {
                results.Add(new()
                {
                    ID = dto.RecordID,
                    Name = dto.Name,
                    AvatarUrl = null
                });
            }
            return results;
        }

        public async Task<SpellDto?> GetByIdAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(12);

            var spell = await GetSingleAsync<Spell>(callback, progress, new DbParameter(nameof(Spell.ID), id));

            if (null == spell)
            {
                callback.Invoke(LoadingHelper.Loading, "Not found.", 100);
                return null;
            }

            var result = new SpellDto()
            {
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(callback, progress, id),
                SpellMisc = await GetSingleAsync<SpellMisc>(callback, progress, new DbParameter(nameof(SpellMisc.SpellID), id)) ?? new(),
                SpellName = await GetSingleAsync<SpellName>(callback, progress, new DbParameter(nameof(SpellName.ID), id)) ?? new(),
                SpellAuraOptions = await GetSingleAsync<SpellAuraOptions>(callback, progress, new DbParameter(nameof(SpellAuraOptions.SpellID), id)),
                SpellPower = await GetSingleAsync<SpellPower>(callback, progress, new DbParameter(nameof(SpellPower.SpellID), id)),
                SpellCooldowns = await GetSingleAsync<SpellCooldowns>(callback, progress, new DbParameter(nameof(SpellCooldowns.SpellID), id)),
                SpellXSpellVisual = await GetSingleAsync<SpellXSpellVisual>(callback, progress, new DbParameter(nameof(SpellXSpellVisual.SpellID), id)),

                Spell = spell,
                IsUpdate = true
            };

            if (result.SpellXSpellVisual != null)
            {
                result.SpellVisual = await GetSingleAsync<SpellVisual>(callback, progress, new DbParameter(nameof(SpellVisual.ID), result.SpellXSpellVisual.SpellVisualID));
                var spellVisualEvents = await GetAsync<SpellVisualEvent>(callback, progress, new DbParameter(nameof(SpellVisualEvent.SpellVisualID), result.SpellXSpellVisual.SpellVisualID));
                spellVisualEvents.ForEach(s =>
                {
                    result.EventGroups.Add(new()
                    {
                        SpellVisualEvent = s
                    });
                });
            }

            var spellEffects = await GetAsync<SpellEffect>(callback, progress, new DbParameter(nameof(SpellEffect.SpellID), id));
            spellEffects.ForEach(s =>
            {
                result.EffectGroups.Add(new SpellDto.EffectGroup()
                {
                    SpellEffect = s
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
                await DeleteAsync(dto.Spell.ID);
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
            await SaveAsync(callback, progress, dto.SpellXSpellVisual);
            await SaveAsync(callback, progress, dto.SpellVisual);

            callback.Invoke(LoadingHelper.Saving, $"Saving {nameof(SpellEffect)}", progress());
            await dto.EffectGroups.ForEachAsync(async e =>
            {
                await SaveAsync(e.SpellEffect);
            });

            callback.Invoke(LoadingHelper.Saving, $"Saving {nameof(SpellVisualEvent)}", progress());
            await dto.EventGroups.ForEachAsync(async v =>
            {
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
            await dto.EventGroups.ForEachAsync(async v =>
            {
                await DeleteAsync(v.SpellVisualEvent);
            });

            callback.Invoke(LoadingHelper.Deleting, $"Deleting {nameof(SpellEffect)}", progress());
            await dto.EffectGroups.ForEachAsync(async e =>
            {
                await DeleteAsync(e.SpellEffect);
            });
            await DeleteAsync(callback, progress, dto.SpellVisual);
            await DeleteAsync(callback, progress, dto.SpellXSpellVisual);
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
