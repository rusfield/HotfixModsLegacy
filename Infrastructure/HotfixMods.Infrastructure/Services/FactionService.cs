using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.AggregateModels;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public partial class FactionService : ServiceBase
    {
        public FactionService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IServerEnumProvider serverEnumProvider, IListfileProvider listfileProvider, IExceptionHandler exceptionHandler, AppConfig appConfig)
            : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, serverEnumProvider, listfileProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.FactionSettings.FromId;
            ToId = appConfig.FactionSettings.ToId;
            VerifiedBuild = appConfig.FactionSettings.VerifiedBuild;
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            try
            {
                var dtos = await GetAsync<HotfixModsEntity>(DefaultCallback, DefaultProgress, true, false, new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
                return dtos
                    .Select(dto => new DashboardModel
                    {
                        ID = (int)dto.RecordID,
                        Name = dto.Name,
                        AvatarUrl = null
                    })
                    .OrderByDescending(d => d.ID)
                    .ToList();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new();
        }

        public async Task<FactionDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback ??= DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(4);

            try
            {
                var faction = await GetSingleAsync<Faction>(callback, progress, new DbParameter(nameof(Faction.ID), id));
                if (faction == null)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(Faction)} not found", 100);
                    return null;
                }

                var result = new FactionDto
                {
                    Faction = faction,
                    FactionTemplate = await GetSingleAsync<FactionTemplate>(callback, progress, new DbParameter(nameof(FactionTemplate.Faction), id)) ?? new(),
                    HotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(callback, progress, id),
                    IsUpdate = true
                };

                if (string.IsNullOrWhiteSpace(result.HotfixModsEntity.Name))
                    result.HotfixModsEntity.Name = string.IsNullOrWhiteSpace(result.Faction.Name) ? "New Faction" : result.Faction.Name;

                callback.Invoke(LoadingHelper.Loading, "Loading successful", 100);
                return result;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }

            return null;
        }

        public async Task<bool> SaveAsync(FactionDto dto, Action<string, string, int>? callback = null)
        {
            callback ??= DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(5);

            try
            {
                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                    await DeleteAsync(dto.Faction.ID);

                callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
                await SetIdAndVerifiedBuild(dto);

                await SaveAsync(callback, progress, dto.HotfixModsEntity);
                await SaveAsync(callback, progress, dto.Faction);
                await SaveAsync(callback, progress, dto.FactionTemplate);

                callback.Invoke(LoadingHelper.Saving, "Saving successful", 100);
                dto.IsUpdate = true;
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback ??= DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(4);

            try
            {
                var dto = await GetByIdAsync(id);
                if (dto == null)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }

                var ownsFaction = HasConfiguredVerifiedBuild(dto.Faction);
                var ownsFactionTemplate = HasConfiguredVerifiedBuild(dto.FactionTemplate);

                if (ownsFactionTemplate)
                    await DeleteAsync(callback, progress, dto.FactionTemplate);
                if (ownsFaction)
                    await DeleteAsync(callback, progress, dto.Faction);

                await DeleteAsync(callback, progress, dto.HotfixModsEntity);
                callback.Invoke(LoadingHelper.Deleting, "Delete successful", 100);
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }

            return false;
        }
    }
}
