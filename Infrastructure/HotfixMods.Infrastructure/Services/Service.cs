using HotfixMods.Core.Interfaces;

namespace HotfixMods.Infrastructure.Services
{
    public partial class Service
    {
        public int VerifiedBuild { get; set; }
        public int FromId { get; set; }
        public string HotfixesSchema { get; } = "hotfixes";
        public string CharactersSchema { get; } = "characters";
        public string WorldSchema { get; } = "world";
        public string HotfixModsSchema { get; } = "hotfixmods";

        IServerDbDefinitionProvider _serverDbDefinitionProvider;
        IClientDbDefinitionProvider _clieIClientDbDefinitionProvider;
        IServerDbProvider _serverDbProvider;
        IClientDbProvider _clientDbProvider;
        
        public Service(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider)
        {
            _serverDbDefinitionProvider = serverDbDefinitionProvider;
            _clieIClientDbDefinitionProvider = clientDbDefinitionProvider;
            _serverDbProvider = serverDbProvider;
            _clientDbProvider = clientDbProvider;
        }

        public async Task GetHotfixEntityAsync(string tableName, string? whereClause = null)
        {
            return await _serverDbProvider.GetAsync(HotfixesSchema, tableName, whereClause) ?? await _clientDbProvider.GetAsync(tableName, whereClause);
        }

        public async Task<int> GetNextHotfixEntityIdAsync<T>()
            where T : new()
        {
            return 0;
        }


    }
}
