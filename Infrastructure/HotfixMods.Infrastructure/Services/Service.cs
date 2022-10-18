using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;

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
        public string Db2Path { get; } = @"C:\hotfixMods";

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

        public async Task<IEnumerable<DbRow>> GetHotfixEntitiesAsync(string tableName, IDictionary<string, object> parameters)
        {
            var result = await _serverDbProvider.GetAsync(HotfixesSchema, tableName, parameters);
            if(!result.Any())
                result = await _clientDbProvider.GetAsync(Db2Path, tableName, parameters);

            return result;
        }

        public async Task<DbRow> GetHotfixEntityAsync(string tableName, IDictionary<string, object> parameters)
        {
            return await _serverDbProvider.GetSingleAsync(HotfixesSchema, tableName, parameters) ?? await _clientDbProvider.GetSingleAsync(Db2Path, tableName, parameters);
        }

        public async Task<int> GetNextHotfixEntityIdAsync<T>()
            where T : new()
        {
            return 0;
        }


    }
}
