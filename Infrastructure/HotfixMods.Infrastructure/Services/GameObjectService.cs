using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.AggregateModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public partial class GameobjectService : ServiceBase
    {
        public GameobjectService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IServerEnumProvider serverEnumProvider, IExceptionHandler exceptionHandler, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, serverEnumProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.GameobjectSettings.FromId;
            ToId = appConfig.GameobjectSettings.ToId;
            VerifiedBuild = appConfig.GameobjectSettings.VerifiedBuild;
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            try
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
                return results.OrderByDescending(d => d.ID).ToList();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return new();
        }

        public async Task<GameobjectDto?> GetByIdAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(5);

            try
            {
                var gameobjectTemplate = await GetSingleAsync<GameobjectTemplate>(callback, progress, new DbParameter(nameof(GameobjectTemplate.Entry), id));
                if (null == gameobjectTemplate)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(GameobjectTemplate)} not found", 100);
                    return null;
                }
                var result = new GameobjectDto()
                {
                    GameobjectTemplate = gameobjectTemplate,
                    GameobjectTemplateAddon = await GetSingleAsync<GameobjectTemplateAddon>(callback, progress, new DbParameter(nameof(GameobjectTemplateAddon.Entry), id)),
                    GameobjectDisplayInfo = await GetSingleAsync<GameobjectDisplayInfo>(callback, progress, new DbParameter(nameof(GameobjectTemplate.DisplayID), gameobjectTemplate.DisplayID)) ?? new(),
                    HotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(callback, progress, gameobjectTemplate.Entry),
                    IsUpdate = true
                };

                callback.Invoke(LoadingHelper.Loading, $"Loading successful", 100);
                return result;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return null;
        }

        public async Task<bool> SaveAsync(GameobjectDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(1);

            try
            {

                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync(dto.GameobjectTemplate.Entry);
                }

                await SetIdAndVerifiedBuild(dto);

                await SaveAsync(callback, progress, dto.GameobjectTemplate);
                await SaveAsync(callback, progress, dto.GameobjectTemplateAddon);
                await SaveAsync(callback, progress, dto.GameobjectDisplayInfo);
                await SaveAsync(callback, progress, dto.HotfixModsEntity);

                callback.Invoke(LoadingHelper.Saving, $"Saving successful", 100);
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

        public async Task<bool> DeleteAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(6);

            try
            {
                var dto = await GetByIdAsync(id);
                if (dto == null)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }

                // Delete gameobjects placed around
                var existingGameobjects = await GetAsync<Gameobject>(new DbParameter(nameof(Gameobject.ID), id));
                await DeleteAsync(callback, progress, existingGameobjects);

                await DeleteAsync(callback, progress, dto.GameobjectDisplayInfo);
                await DeleteAsync(callback, progress, dto.GameobjectTemplateAddon);
                await DeleteAsync(callback, progress, dto.GameobjectTemplate);
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
