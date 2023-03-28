using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public partial class HotfixService : ServiceBase
    {
        public HotfixService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IServerEnumProvider serverEnumProvider, IListfileProvider listfileProvider, IExceptionHandler exceptionHandler, AppConfig appConfig)
            : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, serverEnumProvider, listfileProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.GenericHotfixSettings.FromId;
            ToId = appConfig.GenericHotfixSettings.ToId;
            VerifiedBuild = appConfig.GenericHotfixSettings.VerifiedBuild;
        }

        public async Task<bool> SaveAsync(HotfixDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(14);

            return true;
        }

        public async Task<List<HotfixDto>> GetAsync(string db2Name)
        {
            return null;
        }

        public async Task<HotfixDto?> GetByIdAsync(string db2Name, uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(3);

            try
            {
                var dbRow = await GetSingleAsync(callback, progress, _appConfig.HotfixesSchema, db2Name, false, new DbParameter("id", id));

                if (null == dbRow)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{db2Name} not found", 100);
                    return null;
                }
                var hotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(callback, progress, dbRow.GetIdValue());
                if (string.IsNullOrWhiteSpace(hotfixModsEntity.Name))
                {
                    hotfixModsEntity.Name = db2Name.ToDisplayName();
                }

                callback.Invoke(LoadingHelper.Loading, $"Loading successful", 100);

                return new HotfixDto()
                {
                    HotfixModsEntity = hotfixModsEntity,
                    DbRow = dbRow,
                };
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return null;
        }
    

        public async Task<int> GetNextIdAsync(string db2Name)
        {
            return await base.GetNextIdAsync(db2Name);
        }

        public async Task<List<string>> GetDefinitionNamesAsync()
        {
            return await GetClientDefinitionNamesAsync();
        }

        public async Task<bool> Db2Exists(string db2Name)
        {
            return await Db2ExistsAsync(_appConfig.Db2Path, _appConfig.HotfixesSchema, db2Name);
        }
    }
}
