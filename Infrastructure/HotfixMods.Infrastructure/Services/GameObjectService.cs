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
                GameobjectTemplateAddon = await GetSingleAsync<GameobjectTemplateAddon>(new DbParameter(nameof(GameobjectTemplateAddon.Entry), id)) ?? new(),
                GameobjectDisplayInfo = await GetSingleAsync<GameobjectDisplayInfo>(new DbParameter(nameof(GameobjectTemplate.DisplayId), id)) ?? new(),
                Entity = await GetHotfixModsEntity((int)gameobjectTemplate.Entry)
            };
        }

        public async Task SaveAsync(GameobjectDto gameobjectDto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            
            await SetIdAndVerifiedBuild(gameobjectDto);

            await SaveAsync(gameobjectDto.GameobjectTemplate);
            await SaveAsync(gameobjectDto.GameobjectTemplateAddon);
            await SaveAsync(gameobjectDto.GameobjectDisplayInfo);
            await SaveAsync(gameobjectDto.Entity);
        }

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var gameobjectDto = await GetByIdAsync(id);
            if (gameobjectDto == null) {  
                return false; 
            }

            // Delete gameobjects placed around
            var existingGameobjects = await GetAsync<Gameobject>(new DbParameter(nameof(Gameobject.Id), id));
            foreach(var existingGameobject in  existingGameobjects)
            {
                await DeleteAsync(existingGameobject);
            }

            await DeleteAsync(gameobjectDto.GameobjectDisplayInfo);
            await DeleteAsync(gameobjectDto.GameobjectTemplateAddon);
            await DeleteAsync(gameobjectDto.GameobjectTemplate);
            await DeleteAsync(gameobjectDto.Entity);
            return true;
        }

        public async Task<int> GetNextIdAsync()
        {
            return await GetNextIdAsync<GameobjectTemplate>();
        }
    }
}
