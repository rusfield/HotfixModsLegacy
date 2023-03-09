using HotfixMods.Core.Enums;
using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ServiceBase
    {
        public int VerifiedBuild { get; set; }
        public uint FromId { get; set; }
        public uint ToId { get; set; }


        IServerDbDefinitionProvider _serverDbDefinitionProvider;
        IClientDbDefinitionProvider _clientDbDefinitionProvider;
        IServerDbProvider _serverDbProvider;
        IClientDbProvider _clientDbProvider;
        IExceptionHandler _exceptionHandler;
        protected AppConfig _appConfig;

        public ServiceBase(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IExceptionHandler exceptionHandler, AppConfig appConfig)
        {
            _serverDbDefinitionProvider = serverDbDefinitionProvider;
            _clientDbDefinitionProvider = clientDbDefinitionProvider;
            _serverDbProvider = serverDbProvider;
            _clientDbProvider = clientDbProvider;
            _exceptionHandler = exceptionHandler;
            _appConfig = appConfig;
        }

        #region GET (single)
        protected async Task<T?> GetSingleAsync<T>(Action<string, string, int> callback, Func<int> progress, params DbParameter[] parameters)
        where T : new()
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(T).Name}", progress());
            return await GetSingleAsync<T>(parameters);
        }

        protected async Task<T?> GetSingleAsync<T>(params DbParameter[] parameters)
        where T : new()
        {
            var result = await GetSingleAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), parameters);
            return result.DbRowToEntity<T>();
        }

        protected async Task<DbRow?> GetSingleAsync(string schemaName, string tableName, params DbParameter[] parameters)
        {
            DbRow? result = null;
            var serverDbDefinition = await _serverDbDefinitionProvider.GetDefinitionAsync(_appConfig.HotfixesSchema, tableName);
            if (serverDbDefinition != null)
                result = await _serverDbProvider.GetSingleAsync(schemaName, tableName, serverDbDefinition, parameters);

            if (null == result)
            {
                var clientDbDefinition = await _clientDbDefinitionProvider.GetDefinitionAsync(_appConfig.Db2Path, tableName);
                if (clientDbDefinition != null)
                    result = await _clientDbProvider.GetSingleAsync(_appConfig.Db2Path, tableName, clientDbDefinition, parameters);
            }

            return result;
        }

        #endregion


        #region GET (many)
        protected async Task<List<T>> GetAsync<T>(bool includeClientIfServerResult, params DbParameter[] parameters)
            where T : new()
        {
            return await GetAsync<T>(DefaultProgressCallback, DefaultProgress, includeClientIfServerResult, parameters);
        }

        protected async Task<List<T>> GetAsync<T>(Action<string, string, int> callback, Func<int> progress, bool includeClientIfServerResult, params DbParameter[] parameters)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(T).Name}", progress());
            var result = await GetAsync(GetTableNameOfEntity<T>(), GetTableNameOfEntity<T>(), includeClientIfServerResult, parameters);
            return result.DbRowsToEntities<T>().ToList();
        }

        protected async Task<List<DbRow>> GetAsync(string schemaName, string db2Name, bool includeClientIfServerResult, params DbParameter[] parameters)
        {
            string serverEx = "";
            string clientEx = "";
            var results = new List<DbRow>();

            DbRowDefinition? definition;
            if (schemaName == _appConfig.HotfixesSchema)
            {
                definition = await GetDefinitionFromClientAsync(db2Name);
            }
            else
            {
                definition = await GetDefinitionFromServerAsync(schemaName, db2Name);
            }

            if (definition == null)
                return results;

            try
            {
                var serverResults = await _serverDbProvider.GetAsync(schemaName, db2Name, definition, parameters);
                results.AddRange(serverResults);
            }
            catch (Exception ex)
            {
                serverEx = ex.Message;
            }
            try
            {
                if(includeClientIfServerResult || !results.Any())
                {
                    var clientResults = await _clientDbProvider.GetAsync(_appConfig.Db2Path, db2Name, definition, parameters);
                    results.AddRange(clientResults);
                }
            }
            catch (Exception ex)
            {
                clientEx = ex.Message;
            }

            if (!string.IsNullOrWhiteSpace(serverEx) && !string.IsNullOrWhiteSpace(clientEx))
            {
                throw new Exception($"Failed to get data from both server and client.\nServer error: {serverEx}\nClient error: {clientEx}");
            }
            return results;
        }
        #endregion


        #region SAVE
        protected async Task SaveAsync<T>(Action<string, string, int> callback, Func<int> progress, params T[] entities)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(T).Name}", progress());
            await SaveAsync(entities);
        }
        protected async Task SaveAsync<T>(Action<string, string, int> callback, Func<int> progress, List<T> entities)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(T).Name}", progress());
            await SaveAsync(entities.ToArray());
        }

        protected async Task SaveAsync<T>(List<T> entities)
            where T : new()
        {
            await SaveAsync(entities.ToArray());
        }

        protected async Task SaveAsync<T>(params T[] entities)
            where T : new()
        {
            if (entities.Any())
                await SaveAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), entities.Where(e => e != null).EntitiesToDbRows().ToArray());
        }

        protected async Task SaveAsync(string schemaName, string tableName, params DbRow[] dbRows)
        {
            var hotfixDataTableDefinition = await _serverDbDefinitionProvider.GetDefinitionAsync(_appConfig.HotfixesSchema, _appConfig.HotfixDataTableName);
            if (null == hotfixDataTableDefinition)
            {
                throw new Exception("Unable to load Hotfix Data table.");
            }

            var hotfixDbRows = new List<DbRow>();
            var newHotfixDataId = await GetNextIdAsync(_appConfig.HotfixesSchema, _appConfig.HotfixDataTableName, _appConfig.HotfixDataTableFromId, _appConfig.HotfixDataTableToId, "id");

            foreach (var dbRow in dbRows)
            {
                if (!Enum.TryParse<TableHashes>(tableName, true, out var tableHash))
                {
                    continue;
                }

                var dbParameters = new DbParameter[3];
                dbParameters[0] = new DbParameter(_appConfig.HotfixDataRecordIDColumnName, dbRow.GetIdValue());
                dbParameters[1] = new DbParameter(_appConfig.HotfixDataTableStatusColumnName, (byte)HotfixStatuses.VALID);
                dbParameters[2] = new DbParameter(_appConfig.HotfixDataTableHashColumnName, (uint)tableHash);

                var existingHotfix = await _serverDbProvider.GetSingleAsync(_appConfig.HotfixesSchema, _appConfig.HotfixDataTableName, hotfixDataTableDefinition, dbParameters);
                if (existingHotfix != null)
                {
                    existingHotfix.SetColumnValue(_appConfig.HotfixDataTableStatusColumnName, (byte)HotfixStatuses.INVALID);
                    hotfixDbRows.Add(existingHotfix);
                }

                var hotfixDbRow = new DbRow(tableName);
                foreach (var definition in hotfixDataTableDefinition.ColumnDefinitions)
                {
                    var dbColumn = new DbColumn()
                    {
                        Name = definition.Name,
                        Type = definition.Type,
                        Value = Activator.CreateInstance(definition.Type)!
                    };

                    if (dbColumn.Name.Equals(_appConfig.HotfixDataRecordIDColumnName, StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = dbRow.GetIdValue();
                    else if (dbColumn.Name.Equals(_appConfig.HotfixDataTableStatusColumnName, StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = (byte)HotfixStatuses.VALID;
                    else if (dbColumn.Name.Equals(_appConfig.HotfixDataTableHashColumnName, StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = (uint)tableHash;
                    else if (dbColumn.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = newHotfixDataId;
                    else if (dbColumn.Name.Equals("verifiedbuild", StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = VerifiedBuild;

                    hotfixDbRow.Columns.Add(dbColumn);
                };
                hotfixDbRows.Add(hotfixDbRow);
                newHotfixDataId++;
            }

            if (dbRows.Length > 0)
                await _serverDbProvider.AddOrUpdateAsync(schemaName, tableName, dbRows);

            if (hotfixDbRows.Count > 0)
                await _serverDbProvider.AddOrUpdateAsync(_appConfig.HotfixesSchema, _appConfig.HotfixDataTableName, hotfixDbRows.ToArray());
        }

        #endregion

        #region Delete
        protected async Task<bool> DeleteAsync<T>(List<T> entities)
            where T : new()
        {
            foreach (var entity in entities)
                await DeleteAsync(entity);
            return true;
        }

        protected async Task DeleteAsync(string schemaName, string tableName, params DbParameter[] parameters)
        {
            await _serverDbProvider.DeleteAsync(schemaName, tableName, parameters);
        }

        protected async Task<bool> DeleteAsync<T>(Action<string, string, int> callback, Func<int> progress, List<T> entities)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Deleting, $"Deleting {typeof(T).Name}", progress());
            foreach (var entity in entities)
                await DeleteAsync(entity);
            return true;
        }

        protected async Task<bool> DeleteAsync<T>(Action<string, string, int> callback, Func<int> progress, T entity)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Deleting, $"Deleting {typeof(T).Name}", progress());
            return await DeleteAsync(entity);
        }

        protected async Task<bool> DeleteAsync<T>(T entity)
            where T : new()
        {
            if (null == entity)
                return false;

            var idName = entity.GetIdName();
            var idValue = entity.GetIdValue();
            return await DeleteAsync<T>(new DbParameter(idName, idValue));
        }

        protected async Task<bool> DeleteAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            var schemaName = GetSchemaNameOfEntity<T>();
            if (schemaName == _appConfig.HotfixesSchema)
            {
                var entities = await GetAsync<T>(false, parameters);

                foreach (var entity in entities)
                {
                    var dbParameters = new DbParameter[] { new DbParameter(nameof(HotfixData.RecordID), entity.GetIdValue()), new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild) };

                    var hotfixData = await GetSingleAsync<HotfixData>(dbParameters);
                    if (hotfixData != null)
                    {
                        hotfixData.Status = (byte)HotfixStatuses.RECORD_REMOVED;
                        await SaveAsync(hotfixData);
                    }
                }
            }
            await _serverDbProvider.DeleteAsync(schemaName, GetTableNameOfEntity<T>(), parameters);
            return true;
        }
        #endregion
    }
}
