using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public class CreatureService : Service
    {
        public CreatureService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public async Task<CreatureDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var result = new CreatureDto();
            result.Entity.RecordId = await GetNextIdAsync();

            return result;
        }

        public async Task<CreatureDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            return null;
        }

        public async Task SaveAsync(CreatureDto dto)
        {
            // TODO
        }

        public async Task DeleteAsync(int id)
        {
            // TODO
        }

        public async Task<int> GetNextIdAsync()
        {
            return await GetNextIdAsync<CreatureDto>();
        }
    }
}
