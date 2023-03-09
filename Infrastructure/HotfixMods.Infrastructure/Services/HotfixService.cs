using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.Handlers;

namespace HotfixMods.Infrastructure.Services
{
    public partial class HotfixService : ServiceBase
    {
        public HotfixService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IExceptionHandler exceptionHandler, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, exceptionHandler, appConfig) 
        {
            FromId = appConfig.GenericHotfixSettings.FromId;
            ToId = appConfig.GenericHotfixSettings.ToId;
            VerifiedBuild = appConfig.GenericHotfixSettings.VerifiedBuild;
        }

        public async Task<IEnumerable<DbRow>> GetAsync(string db2Name)
        {
            return await GetAsync(db2Name);
        }
        public async Task<DbRow?> GetByIdAsync(string db2Name, int id)
        {
            return await GetSingleAsync(_appConfig.HotfixesSchema, db2Name, false, new DbParameter("id", id));
        }

        public async Task<uint> GetNextIdAsync(string db2Name)
        {
            return await base.GetNextIdAsync(db2Name);
        }

        public async Task<IEnumerable<string>> GetDefinitionNamesAsync()
        {
            return await GetClientDefinitionNamesAsync();
        }

        public async Task<bool> Db2Exists(string db2Name)
        {
            return await Db2ExistsAsync(_appConfig.Db2Path, _appConfig.HotfixesSchema, db2Name);
        }
    }
}
