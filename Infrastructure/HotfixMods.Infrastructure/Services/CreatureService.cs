using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public class CreatureService : ServiceBase
    {
        public CreatureService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public async Task<CreatureDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var result = new CreatureDto();

            return result;
        }

        public async Task<CreatureDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var creatureTemplate = await GetSingleAsync<CreatureTemplate>(new DbParameter(nameof(CreatureTemplate.Entry), id));
            if(creatureTemplate == null)
            {
                return null;
            }

            return new CreatureDto()
            {
                CreatureTemplate = creatureTemplate,
                
                IsUpdate = true
            };
        }

        public async Task SaveAsync(CreatureDto dto, Action<string, string, int>? callback = null)
        {
            // TODO
        }

        public async Task DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            // TODO
        }
    }
}
