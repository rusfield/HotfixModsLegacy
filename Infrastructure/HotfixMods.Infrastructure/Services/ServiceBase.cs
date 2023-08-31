using HotfixMods.Core.Attributes;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;
using HotfixMods.Providers.Interfaces;
using HotfixMods.Providers.Models;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ServiceBase
    {
        public int VerifiedBuild { get; set; }

        IServerDbDefinitionProvider _serverDbDefinitionProvider;
        IClientDbDefinitionProvider _clientDbDefinitionProvider;
        IServerDbProvider _serverDbProvider;
        IClientDbProvider _clientDbProvider;
        IServerValuesProvider _serverValuesProvider;
        IExceptionHandler _exceptionHandler;
        IListfileProvider _listfileProvider;

        protected AppConfig _appConfig;

        public ServiceBase(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IServerValuesProvider serverValuesProvider, IListfileProvider listfileProvider, IExceptionHandler exceptionHandler, AppConfig appConfig)
        {
            _serverDbDefinitionProvider = serverDbDefinitionProvider;
            _clientDbDefinitionProvider = clientDbDefinitionProvider;
            _serverDbProvider = serverDbProvider;
            _clientDbProvider = clientDbProvider;
            _serverValuesProvider = serverValuesProvider;
            _listfileProvider = listfileProvider;
            _exceptionHandler = exceptionHandler;
            _appConfig = appConfig;
        }

        #region GET (single)
        protected async Task<DbRow?> GetSingleAsync(Action<string, string, int> callback, Func<int> progress, string schemaName, string db2Name, bool serverOnly, params DbParameter[] parameters)
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {db2Name}", progress());
            return await GetSingleAsync(schemaName, db2Name, serverOnly, parameters);
        }

        protected async Task<T?> GetSingleAsync<T>(Action<string, string, int> callback, Func<int> progress, bool serverOnly, params DbParameter[] parameters)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(T).Name}", progress());
            var result = await GetSingleAsync(GetSchemaNameOfEntity<T>()!, typeof(T).Name, serverOnly, parameters);
            return result.DbRowToEntity<T>();
        }

        protected async Task<T?> GetSingleAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            return await GetSingleAsync<T>(DefaultCallback, DefaultProgress, parameters);
        }

        protected async Task<T?> GetSingleAsync<T>(Action<string, string, int> callback, Func<int> progress, params DbParameter[] parameters)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(T).Name}", progress());
            var schema = GetSchemaNameOfEntity<T>()!;
            var result = await GetSingleAsync(schema, typeof(T).Name, !schema.Equals(_appConfig.HotfixesSchema), parameters);
            return result.DbRowToEntity<T>();
        }

        protected async Task<DbRow?> GetSingleAsync(string schemaName, string db2Name, bool serverOnly, params DbParameter[] parameters)
        {
            DbRow? result;
            DbRowDefinition? definition;
            string tableName = db2Name.ToTableName();

            // TODO: Check if serverOnly parameter is needed anymore now that HotfixData and Entity is hardcoded here.
            var useClientDefinition = !serverOnly
                && _appConfig.HotfixesSchema.Equals(schemaName, StringComparison.InvariantCultureIgnoreCase)
                && !db2Name.Equals(nameof(HotfixData), StringComparison.InvariantCultureIgnoreCase)
                && !db2Name.Equals(nameof(HotfixModsEntity), StringComparison.InvariantCultureIgnoreCase);

            if (useClientDefinition)
                definition = await _clientDbDefinitionProvider.GetDefinitionAsync(db2Name);
            else
                definition = await _serverDbDefinitionProvider.GetDefinitionAsync(schemaName, tableName);

            if (null == definition)
            {
                _exceptionHandler.Handle(new Exception($"Unable to get definition for {db2Name}."));
                return null;
            }
                

            result = await _serverDbProvider.GetSingleAsync(schemaName, tableName, definition, parameters);

            if (null == result && !serverOnly)
            {
                result = await _clientDbProvider.GetSingleAsync(db2Name, definition, parameters);
            }

            return result;
        }

        #endregion


        #region GET (many)
        protected async Task<PagedDbResult<T>> GetPagedAsync<T>(Action<string, string, int> callback, Func<int> progress, bool serverOnly, bool includeClientIfServerResult, int pageIndex, int pageSize, params DbParameter[] parameters)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(T).Name}", progress());
            var result = await GetPagedAsync(GetSchemaNameOfEntity<T>(), typeof(T).Name, serverOnly, includeClientIfServerResult, pageIndex, pageSize, parameters);
            return result.DbRowsToPagedEntities<T>();
        }

        protected async Task<List<T>> GetAsync<T>(Action<string, string, int> callback, Func<int> progress, params DbParameter[] parameters)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(T).Name}", progress());
            var result = await GetPagedAsync(GetSchemaNameOfEntity<T>(), typeof(T).Name, false, false, -1, -1, parameters);
            return result.DbRowsToEntities<T>();
        }

        protected async Task<List<T>> GetAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            var result = await GetPagedAsync(GetSchemaNameOfEntity<T>(), typeof(T).Name, false, false, -1, -1, parameters);
            return result.DbRowsToEntities<T>();
        }

        protected async Task<PagedDbResult> GetAsync(string schemaName, string db2Name, params DbParameter[] parameters)
        {
            return await GetPagedAsync(schemaName, db2Name, false, true, -1, -1, parameters);
        }
        protected async Task<PagedDbResult> GetPagedAsync(string schemaName, string db2Name, bool serverOnly, bool includeClientIfServerResult, int pageIndex, int pageSize, params DbParameter[] parameters)
        {
            DbRowDefinition? definition;
            string serverEx = "";
            string clientEx = "";
            string tableName = db2Name.ToTableName();
            var results = new PagedDbResult(pageIndex, pageSize, 0);

            // TODO: Check if serverOnly parameter is needed anymore now that HotfixData and Entity is hardcoded here.
            var useClientDefinition = !serverOnly
                && _appConfig.HotfixesSchema.Equals(schemaName, StringComparison.InvariantCultureIgnoreCase)
                && !db2Name.Equals(nameof(HotfixData), StringComparison.InvariantCultureIgnoreCase)
                && !db2Name.Equals(nameof(HotfixModsEntity), StringComparison.InvariantCultureIgnoreCase);

            if (useClientDefinition)
                definition = await _clientDbDefinitionProvider.GetDefinitionAsync(db2Name);
            else
                definition = await _serverDbDefinitionProvider.GetDefinitionAsync(schemaName, tableName);

            if (null == definition)
                throw new Exception($"Unable to get definition for {db2Name}.");

            try
            {
                var serverResults = await _serverDbProvider.GetAsync(schemaName, tableName, definition, -1, -1, parameters);
                results.Rows.AddRange(serverResults.Rows);
                results.TotalRowCount += serverResults.TotalRowCount;
            }
            catch (Exception ex)
            {
                serverEx = ex.Message;
            }
            try
            {
                if (!serverOnly && (includeClientIfServerResult || !results.Rows.Any()))
                {
                    var clientResults = await _clientDbProvider.GetAsync(db2Name, definition, pageIndex, pageSize, parameters);

                    var resultIds = new HashSet<string>(results.Rows.Select(r => r.GetIdColumnValue().ToString()));
                    foreach (var clientResult in clientResults.Rows)
                    {
                        var idValue = clientResult.GetIdColumnValue().ToString();

                        if (!resultIds.Contains(idValue))
                        {
                            results.Rows.Add(clientResult);
                            results.TotalRowCount += clientResults.TotalRowCount;
                            resultIds.Add(idValue);
                        }
                    }
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
            callback.Invoke(LoadingHelper.Saving, $"Saving {typeof(T).Name}", progress());
            await SaveAsync(entities);
        }
        protected async Task SaveAsync<T>(Action<string, string, int> callback, Func<int> progress, List<T> entities)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Saving, $"Saving {typeof(T).Name}", progress());
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
            {
                var definition = await GetDefinitionOfEntity<T>();
                await SaveAsync(GetSchemaNameOfEntity<T>(), typeof(T).Name, entities.Where(e => e != null).EntitiesToDbRows(definition).ToArray());
            }
        }

        protected async Task SaveAsync(Action<string, string, int> callback, Func<int> progress, string schemaName, string db2Name, params DbRow[] dbRows)
        {
            callback.Invoke(LoadingHelper.Saving, $"Saving {db2Name}", progress());
            await SaveAsync(schemaName, db2Name, dbRows);
        }

        protected async Task SaveAsync(string schemaName, string db2Name, params DbRow[] dbRows)
        {
            var tableName = db2Name.ToTableName();
            var hotfixDataDefinition = await _serverDbDefinitionProvider.GetDefinitionAsync(_appConfig.HotfixesSchema, nameof(HotfixData).ToTableName());

            var hotfixDbRows = new List<DbRow>();
            var newHotfixDataIdString = await GetNextIdAsync(_appConfig.HotfixesSchema, nameof(HotfixData).ToTableName());
            var newHotfixDataId = ulong.Parse(newHotfixDataIdString);

            foreach (var dbRow in dbRows)
            {
                if (!Enum.TryParse<TableHashes>(tableName, true, out var tableHash))
                {
                    continue;
                }

                var dbParameters = new DbParameter[3];
                dbParameters[0] = new DbParameter(nameof(HotfixData.RecordID), dbRow.GetIdColumnValue());
                dbParameters[1] = new DbParameter(nameof(HotfixData.Status), (byte)HotfixStatuses.VALID);
                dbParameters[2] = new DbParameter(nameof(HotfixData.TableHash), (uint)tableHash);

                var existingHotfix = await _serverDbProvider.GetSingleAsync(_appConfig.HotfixesSchema, nameof(HotfixData).ToTableName(), hotfixDataDefinition, dbParameters);
                if (existingHotfix != null)
                {
                    existingHotfix.SetColumnValue(nameof(HotfixData.Status), (byte)HotfixStatuses.INVALID);
                    hotfixDbRows.Add(existingHotfix);
                }

                var hotfixDbRow = new DbRow(tableName);
                foreach (var def in hotfixDataDefinition.ColumnDefinitions)
                {
                    var dbColumn = new DbColumn()
                    {
                        Value = Activator.CreateInstance(def.Type)!,
                        Definition = def
                    };

                    if (dbColumn.Definition.Name.Equals(nameof(HotfixData.RecordID), StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = dbRow.GetIdColumnValue();
                    else if (dbColumn.Definition.Name.Equals(nameof(HotfixData.Status), StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = (byte)HotfixStatuses.VALID;
                    else if (dbColumn.Definition.Name.Equals(nameof(HotfixData.TableHash), StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = (uint)tableHash;
                    else if (dbColumn.Definition.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = newHotfixDataId;
                    else if (dbColumn.Definition.Name.Equals("verifiedbuild", StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = VerifiedBuild;

                    hotfixDbRow.Columns.Add(dbColumn);
                };
                hotfixDbRows.Add(hotfixDbRow);
                newHotfixDataId++;
            }

            if (dbRows.Length > 0)
                await _serverDbProvider.AddOrUpdateAsync(schemaName, tableName, dbRows);

            if (hotfixDbRows.Count > 0)
                await _serverDbProvider.AddOrUpdateAsync(_appConfig.HotfixesSchema, nameof(HotfixData).ToTableName(), hotfixDbRows.ToArray());
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

        protected async Task DeleteAsync(Action<string, string, int> callback, Func<int> progress, string schemaName, string tableName, params DbParameter[] parameters)
        {
            callback.Invoke(LoadingHelper.Deleting, $"Deleting {tableName.ToDisplayString()}", progress());
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

            var definition = await GetDefinitionOfEntity<T>();
            var idColumn = entity.EntityToDbRow(definition).GetIdColumn();
            return await DeleteAsync<T>(new DbParameter(idColumn.Definition.Name, idColumn.Value));
        }

        protected async Task<bool> DeleteAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            var schemaName = GetSchemaNameOfEntity<T>();
            if (schemaName == _appConfig.HotfixesSchema)
            {
                var entities = await GetAsync<T>(parameters);
                var definition = await GetDefinitionOfEntity<T>();

                var dbRows = entities.EntitiesToDbRows(definition);

                foreach (var dbRow in dbRows)
                {
                    var dbParameters = new DbParameter[] { new DbParameter(nameof(HotfixData.RecordID), dbRow.GetIdColumnValue()), new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild) };

                    var hotfixData = await GetSingleAsync<HotfixData>(DefaultCallback, DefaultProgress, true, dbParameters);
                    if (hotfixData != null)
                    {
                        hotfixData.Status = (byte)HotfixStatuses.RECORD_REMOVED;
                        await SaveAsync(hotfixData);
                    }
                }
            }
            await _serverDbProvider.DeleteAsync(schemaName, typeof(T).Name, parameters);
            return true;
        }
        #endregion
    }
}
