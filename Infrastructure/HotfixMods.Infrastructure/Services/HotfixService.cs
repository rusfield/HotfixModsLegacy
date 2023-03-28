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

            try { 
            callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
            if (dto.IsUpdate)
            {
                await DeleteAsync(dto.HotfixModsEntity.RecordID);
            }

            callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
            await SetIdAndVerifiedBuild(dto);

            await SaveAsync(callback, progress, dto.HotfixModsEntity);
            await SaveAsync(callback, progress, dto.DbRow);

                callback.Invoke("Saving", "Saving successful", 100);
                dto.IsUpdate = true;
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
                return false;
            }
        }

        public async Task<HotfixDto?> GetByIdAsync(string db2Name, int id, Action<string, string, int>? callback = null)
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

        public async Task<bool> DeleteAsync(string db2, int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(10);

            try
            {
                var dto = await GetByIdAsync(db2, id);
                if (null == dto)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }

                await DeleteAsync(callback, progress, _appConfig.HotfixesSchema, db2.ToTableName(), new DbParameter(dto.DbRow.GetIdName(), dto.DbRow.GetIdValue()));
                await DeleteAsync(callback, progress, dto.HotfixModsEntity);

                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return false;
        }

        // Return next ID, and a bool for whether the previous ID was created by HotfixMods
        // If it was not, it is potentially in range of being overwritten in later patches, and the next ID should be a bit higher than just +1.
        public async Task<(int, bool)> GetNextIdAsync(string db2Name)
        {
            var nextId = await base.GetNextIdAsync(db2Name);
            var hotfixMods = GetAsync<HotfixModsEntity>(new DbParameter(nameof(HotfixModsEntity.RecordID), nextId - 1));

            return (nextId, hotfixMods != null);
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
