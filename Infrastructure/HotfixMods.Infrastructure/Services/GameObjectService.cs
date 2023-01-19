using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class GameobjectService : ServiceBase
    {
        public GameobjectService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public async Task<GameobjectDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var result = new GameobjectDto();

            return result;
        }

        public async Task<GameobjectDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var gameobjectTemplate = await GetSingleByIdAsync<GameobjectTemplate>(id);
            if (null == gameobjectTemplate)
            {
                return null;
            }
            return new GameobjectDto()
            {
                GameobjectTemplate = gameobjectTemplate,
                GameobjectTemplateAddon = await GetSingleAsync<GameobjectTemplateAddon>(new DbParameter(nameof(GameobjectTemplateAddon.Entry), id)),
                GameobjectDisplayInfo = await GetSingleAsync<GameobjectDisplayInfo>(new DbParameter(nameof(GameobjectTemplate.DisplayId), id)),
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity((int)gameobjectTemplate.Entry),
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

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var dto = await GetByIdAsync(id);
            if (dto == null) {  
                return false; 
            }

            // Delete gameobjects placed around
            var existingGameobjects = await GetAsync<Gameobject>(new DbParameter(nameof(Gameobject.Id), id));
            existingGameobjects.ForEach(async g =>
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
