using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.Config;

namespace HotfixMods.Infrastructure.Services
{
    public class GenericHotfixService : Service
    {
        public GenericHotfixService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        // Cache
        static List<string> definitionNames;

        public async Task<DbRow?> GetByIdAsync(string db2Name, int id)
        {
            return await GetSingleAsync(db2Name, new DbParameter("id", id));
        }

        public async Task<int> GetNextIdAsync(string db2Name)
        {
            return await base.GetNextIdAsync(db2Name);
        }

        public async Task<IEnumerable<string>> GetDefinitionNamesAsync()
        {
            if (null == definitionNames || !_appConfig.CacheResults)
                definitionNames = (await GetClientDefinitionNamesAsync()).ToList();
            return definitionNames;
        }

        public async Task<bool> Db2Exists(string db2Name)
        {
            return await Db2Exists(_appConfig.Location, _appConfig.HotfixesSchema, db2Name);
        }
    }
}
