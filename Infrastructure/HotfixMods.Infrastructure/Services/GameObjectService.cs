using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public partial class GameobjectService : ServiceBase
    {
        public GameobjectService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public GameobjectDto GetNew(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            callback.Invoke(LoadingHelper.Loading, "Returning new template", 100);
            return new()
            {
                HotfixModsEntity = new()
                {
                    Name = "New Game Object"
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
                    Id = dto.RecordId,
                    Name = dto.Name,
                    AvatarUrl = null
                });
            }
            return results;
        }

        public async Task<GameobjectDto?> GetByIdAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(5);

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
                GameobjectDisplayInfo = await GetSingleAsync<GameobjectDisplayInfo>(callback, progress, new DbParameter(nameof(GameobjectTemplate.DisplayId), gameobjectTemplate.DisplayId)) ?? new(),
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(callback, progress, gameobjectTemplate.Entry),
                IsUpdate = true
            };

            callback.Invoke(LoadingHelper.Loading, $"Loading successful", 100);
            return result;
        }

        public async Task<bool> SaveAsync(GameobjectDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(1);

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

        public async Task<bool> DeleteAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(6);

            var dto = await GetByIdAsync(id);
            if (dto == null) 
            {
                callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                return false; 
            }

            // Delete gameobjects placed around
            var existingGameobjects = await GetAsync<Gameobject>(new DbParameter(nameof(Gameobject.Id), id));
            await DeleteAsync(callback, progress, existingGameobjects);

            await DeleteAsync(callback, progress, dto.GameobjectDisplayInfo);
            await DeleteAsync(callback, progress, dto.GameobjectTemplateAddon);
            await DeleteAsync(callback, progress, dto.GameobjectTemplate);
            await DeleteAsync(callback, progress, dto.HotfixModsEntity);

            callback.Invoke(LoadingHelper.Deleting, "Delete successful", 100);
            return true;
        }
    }
}
