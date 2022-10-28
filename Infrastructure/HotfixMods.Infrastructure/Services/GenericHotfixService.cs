using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.Config;

namespace HotfixMods.Infrastructure.Services
{
    public class GenericHotfixService : Service
    {
        public GenericHotfixService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }
    
        public async Task<DbRow?> GetByIdAsync(string db2Name, int id)
        {
            return await GetSingleAsync(db2Name, new DbParameter("id", id));
        }
    }
}
