using HotfixMods.Core.Enums;
using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public class GameobjectService : Service
    {
        public GameobjectService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public async Task<GameobjectDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var result = new GameobjectDto();
            result.GameobjectTemplate.Entry = (uint)await GetNextIdAsync();

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

        public async Task SaveAsync(GameobjectDto gameobjectDto)
        {
            await SaveAsync(gameobjectDto.GameobjectTemplate);
            await SaveAsync(gameobjectDto.GameobjectTemplateAddon);
            await SaveAsync(gameobjectDto.GameobjectDisplayInfo);
            await SaveAsync(gameobjectDto.Entity);
        }

        public async Task DeleteAsync(int id)
        {
            // TODO
        }

        public async Task<int> GetNextIdAsync()
        {
            return await GetNextIdAsync<GameobjectTemplate>();
        }
    }
}
