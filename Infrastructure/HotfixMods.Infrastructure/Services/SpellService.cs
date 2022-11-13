using HotfixMods.Core.Interfaces;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public class SpellService : Service
    {
        public SpellService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public async Task<SpellDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var result = new SpellDto();
            result.Entity.RecordId = await GetNextIdAsync();

            return result;
        }

        public async Task<SpellDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            return null;
        }

        public async Task SaveAsync(SpellDto dto)
        {
            // TODO
        }

        public async Task DeleteAsync(int id)
        {
            // TODO
        }

        public async Task<int> GetNextIdAsync()
        {
            return await GetNextIdAsync<SpellDto>();
        }
    }
}
