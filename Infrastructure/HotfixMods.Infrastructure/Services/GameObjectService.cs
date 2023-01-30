using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
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
            return new();
        }

        public async Task<GameobjectDto?> GetByIdAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var gameobjectTemplate = await GetSingleAsync<GameobjectTemplate>(new DbParameter(nameof(GameobjectTemplate.Entry), id));
            if (null == gameobjectTemplate)
            {
                return null;
            }
            return new GameobjectDto()
            {
                GameobjectTemplate = gameobjectTemplate,
                GameobjectTemplateAddon = await GetSingleAsync<GameobjectTemplateAddon>(new DbParameter(nameof(GameobjectTemplateAddon.Entry), id)),
                GameobjectDisplayInfo = await GetSingleAsync<GameobjectDisplayInfo>(new DbParameter(nameof(GameobjectTemplate.DisplayId), id)),
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(gameobjectTemplate.Entry),
                IsUpdate = true
            };
        }

        public async Task<bool> SaveAsync(GameobjectDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            
            await SetIdAndVerifiedBuild(dto);

            await SaveAsync(dto.GameobjectTemplate);
            await SaveAsync(dto.GameobjectTemplateAddon);
            await SaveAsync(dto.GameobjectDisplayInfo);
            await SaveAsync(dto.HotfixModsEntity);

            dto.IsUpdate = true;
            return true;
        }

        public async Task<bool> DeleteAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var dto = await GetByIdAsync(id);
            if (dto == null) {  
                return false; 
            }

            // Delete gameobjects placed around
            var existingGameobjects = await GetAsync<Gameobject>(new DbParameter(nameof(Gameobject.Id), id));
            await existingGameobjects.ForEachAsync(async g =>
            {
                await DeleteAsync(g);
            });

            await DeleteAsync(dto.GameobjectDisplayInfo);
            await DeleteAsync(dto.GameobjectTemplateAddon);
            await DeleteAsync(dto.GameobjectTemplate);
            await DeleteAsync(dto.HotfixModsEntity);
            return true;
        }
    }
}
