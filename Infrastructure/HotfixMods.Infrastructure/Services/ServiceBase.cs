using HotfixMods.Core.Enums;
using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.Extensions;
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
        protected AppConfig _appConfig;

        public ServiceBase(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig)
        {
            _serverDbDefinitionProvider = serverDbDefinitionProvider;
            _clientDbDefinitionProvider = clientDbDefinitionProvider;
            _serverDbProvider = serverDbProvider;
            _clientDbProvider = clientDbProvider;
            _appConfig = appConfig;
        }

        protected async Task<T?> GetSingleAsync<T>(Action<string, string, int> callback, Func<int> progress, params DbParameter[] parameters)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(T).Name}", progress());
            return await GetSingleAsync<T>(parameters);
        }

        protected async Task<T?> GetSingleAsync<T>(params DbParameter[] parameters)
        where T : new()
        {
            try
            {
                var serverResult = await _serverDbProvider.GetSingleAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), GetDbRowDefinitionOfEntity<T>(), parameters);
                if (serverResult != null)
                    return serverResult.DbRowToEntity<T?>();
            }
            catch (Exception ex)
            {

            }
            try
            {
                var clientResult = await _clientDbProvider.GetSingleAsync(_appConfig.Location, typeof(T).Name, GetDbRowDefinitionOfEntity<T>(), parameters);
                if (clientResult != null)
                    return clientResult.DbRowToEntity<T?>();
            }
            catch (Exception ex)
            {

            }
            return default;
        }


        protected async Task<DbRow?> GetSingleAsync(string tableName, params DbParameter[] parameters)
        {
            DbRow? result = null;
            var serverDbDefinition = await _serverDbDefinitionProvider.GetDefinitionAsync(_appConfig.HotfixesSchema, tableName);
            if (serverDbDefinition != null)
                result = await _serverDbProvider.GetSingleAsync(_appConfig.HotfixesSchema, tableName, serverDbDefinition, parameters);

            if (null == result)
            {
                var clientDbDefinition = await _clientDbDefinitionProvider.GetDefinitionAsync(_appConfig.Location, tableName);
                if (clientDbDefinition != null)
                    result = await _clientDbProvider.GetSingleAsync(_appConfig.Location, tableName, clientDbDefinition, parameters);
            }

            return result;
        }


        protected async Task<List<T>> GetAsync<T>(Action<string, string, int> callback, Func<int> progress, params DbParameter[] parameters)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(T).Name}", progress());
            return await GetAsync<T>(parameters);
        }

            protected async Task<List<T>> GetAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            try
            {
                var serverResults = await _serverDbProvider.GetAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), GetDbRowDefinitionOfEntity<T>(), parameters);
                if (serverResults.Any())
                    return serverResults.DbRowsToEntities<T>().ToList();
            }
            catch (Exception ex)
            {

            }
            try
            {
                var clientResults = await _clientDbProvider.GetAsync(_appConfig.Location, typeof(T).Name, GetDbRowDefinitionOfEntity<T>(), parameters);
                if (clientResults.Any())
                    return clientResults.DbRowsToEntities<T>().ToList();
            }
            catch (Exception ex)
            {

            }

            return new();
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


        protected async Task SaveAsync<T>(Action<string, string, int> callback, Func<int> progress, params T[] entities)
            where T : new()
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(T).Name}", progress());
            await SaveAsync(entities);
        }

        protected async Task SaveAsync<T>(params T[] entities)
            where T : new()
        {
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
                dbParameters[0] = new DbParameter(_appConfig.HotfixDataRecordIdColumnName, dbRow.GetIdValue());
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

                    if (dbColumn.Name.Equals(_appConfig.HotfixDataRecordIdColumnName, StringComparison.CurrentCultureIgnoreCase))
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

        protected async Task<bool> DeleteAsync<T>(List<T> entities) 
            where T : new()
        {
            foreach(var entity in entities)
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
                var entities = await GetAsync<T>(parameters);

                foreach (var entity in entities)
                {
                    var dbParameters = new DbParameter[] { new DbParameter(nameof(HotfixData.RecordId), entity.GetIdValue()), new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild) };

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

        protected async Task<uint> GetIdByConditionsAsync<T>(uint? currentId, bool isUpdate)
            where T : new()
        {
            // Entity is null, and this ID will not be used.
            if (null == currentId)
                return 0;

            // Entity is new, or entity should be saved as new
            if ((uint)currentId == 0 || !isUpdate)
                return await GetNextIdAsync<T>();

            // Entity is being updated
            return (uint)currentId;
        }

        protected async Task<uint> GetNextIdAsync<T>()
            where T : new()
        {
            return await GetNextIdAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), FromId, ToId, GetIdPropertyNameOfEntity<T>());
        }

        protected async Task<uint> GetNextIdAsync(string tableName)
        {
            return await GetNextIdAsync(_appConfig.HotfixesSchema, tableName, FromId, ToId, "id");
        }

        async Task<uint> GetNextIdAsync(string schemaName, string tableName, uint fromId, uint toId, string idPropertyName)
        {
            var highestId = await _serverDbProvider.GetHighestIdAsync(schemaName, tableName, fromId, toId, idPropertyName);

            if (highestId > 0)
            {
                if (highestId == ToId)
                {
                    throw new Exception("Database is full.");
                }
                return highestId + 1;
            }
            else
            {
                return FromId;
            }
        }


        protected async Task DeleteAsync(string schemaName, string tableName, params DbParameter[] parameters)
        {
            await _serverDbProvider.DeleteAsync(schemaName, tableName, parameters);
        }

        protected async Task<List<string>> GetClientDefinitionNamesAsync()
        {
            return (await _clientDbDefinitionProvider.GetDefinitionNamesAsync()).ToList();
        }

        protected async Task<bool> Db2Exists(string clientDbLocation, string serverSchemaName, string db2Name)
        {
            return await _clientDbProvider.Db2ExistsAsync(clientDbLocation, db2Name) || await _serverDbProvider.TableExistsAsync(serverSchemaName, db2Name);
        }

        protected async Task<HotfixModsEntity> GetExistingOrNewHotfixModsEntity(Action<string, string, int> callback, Func<int> progress, uint id)
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(HotfixModsEntity).Name}", progress());
            return await GetExistingOrNewHotfixModsEntity(id);
        }

        protected async Task<HotfixModsEntity> GetExistingOrNewHotfixModsEntity(uint id)
        {
            return await GetSingleAsync<HotfixModsEntity>(new DbParameter(nameof(HotfixModsEntity.Id), id))
                ??
                new()
                {
                    Id = id,
                    VerifiedBuild = VerifiedBuild
                };
        }
    }
}
