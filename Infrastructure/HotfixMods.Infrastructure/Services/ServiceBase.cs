using HotfixMods.Core.Enums;
using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ServiceBase
    {
        public int VerifiedBuild { get; set; }
        public int FromId { get; set; }


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

        protected async Task<T?> GetSingleByIdAsync<T>(int id)
            where T : new()
        {
            return await GetSingleAsync<T>(new DbParameter(GetIdPropertyNameOfEntity<T>(), id));
        }

        protected async Task<T?> GetSingleAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            return (await _serverDbProvider.GetSingleAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), GetDbRowDefinitionOfEntity<T>(), parameters) ?? await _clientDbProvider.GetSingleAsync(_appConfig.Location, typeof(T).Name, GetDbRowDefinitionOfEntity<T>(), parameters)).DbRowToEntity<T?>();
        }


        protected async Task<DbRow?> GetSingleAsync(string tableName, params DbParameter[] parameters)
        {
            DbRow? result = null;
            var serverDbDefinition = await _serverDbDefinitionProvider.GetDefinitionAsync(_appConfig.HotfixesSchema, tableName);
            if (serverDbDefinition != null)
                result = await _serverDbProvider.GetSingleAsync(_appConfig.HotfixesSchema, tableName, serverDbDefinition, parameters);

            if(null == result)
            {
                var clientDbDefinition = await _clientDbDefinitionProvider.GetDefinitionAsync(_appConfig.Location, tableName);
                if(clientDbDefinition != null)
                    result = await _clientDbProvider.GetSingleAsync(_appConfig.Location, tableName, clientDbDefinition, parameters);
            }

            return result;
        }
        

        protected async Task<List<T>> GetAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            var results = await _serverDbProvider.GetAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), GetDbRowDefinitionOfEntity<T>(), parameters);
            if(!results.Any())
                results = await _clientDbProvider.GetAsync(_appConfig.Location, typeof(T).Name, GetDbRowDefinitionOfEntity<T>(), parameters);

            return results.DbRowsToEntities<T>().ToList();
        }


        protected async Task SaveAsync<T>(IEnumerable<T> entities)
            where T : new()
        {
            await SaveAsync(entities.ToArray());
        }

        protected async Task SaveAsync<T>(params T[] entities)
            where T : new()
        {
            await SaveAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), entities.EntitiesToDbRows().ToArray());
        }

        protected async Task SaveAsync(string schemaName, string tableName, params DbRow[] dbRows)
        {
            var hotfixDataTableDefinition = await _serverDbDefinitionProvider.GetDefinitionAsync(_appConfig.HotfixesSchema, _appConfig.HotfixDataTableName);
            if(null == hotfixDataTableDefinition)
            {
                throw new Exception("Unable to load Hotfix Data table.");
            }

            var hotfixDbRows = new List<DbRow>();
            foreach(var dbRow in dbRows)
            {
                var tableHash = Enum.Parse<TableHashes>(tableName.Replace("_", ""), true);

                var dbParameters = new DbParameter[3];
                dbParameters[0] = new DbParameter(_appConfig.HotfixDataRecordIdColumnName, dbRow.GetId());
                dbParameters[1] = new DbParameter(_appConfig.HotfixDataTableStatusColumnName, HotfixStatuses.VALID);
                dbParameters[2] = new DbParameter(_appConfig.HotfixDataTableHashColumnName, tableHash);

                var existingHotfix = await _serverDbProvider.GetSingleAsync(_appConfig.HotfixesSchema, _appConfig.HotfixDataTableName, hotfixDataTableDefinition, dbParameters);
                if(existingHotfix != null)
                {
                    existingHotfix.SetColumnValue(_appConfig.HotfixDataTableStatusColumnName, HotfixStatuses.INVALID);
                    hotfixDbRows.Add(existingHotfix);
                }

                var hotfixDbRow = new DbRow(tableName);
                foreach (var definition in hotfixDataTableDefinition.ColumnDefinitions)
                {
                    var dbColumn = new DbColumn()
                    {
                        Name = definition.Name,
                        Type= definition.Type,
                        Value = Activator.CreateInstance(definition.Type)!
                    };

                    if (dbColumn.Name.Equals(_appConfig.HotfixDataRecordIdColumnName, StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = dbRow.GetId();
                    else if (dbColumn.Name.Equals(_appConfig.HotfixDataTableStatusColumnName, StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = HotfixStatuses.VALID;
                    else if (dbColumn.Name.Equals(_appConfig.HotfixDataTableHashColumnName, StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = tableHash;
                    else if (dbColumn.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = await _serverDbProvider.GetNextIdAsync(_appConfig.HotfixesSchema, _appConfig.HotfixDataTableName, 1);
                    else if (dbColumn.Name.Equals("verifiedbuild", StringComparison.CurrentCultureIgnoreCase))
                        dbColumn.Value = VerifiedBuild;

                    hotfixDbRow.Columns.Add(dbColumn);
                };
                hotfixDbRows.Add(hotfixDbRow);
            }

            if(dbRows.Length > 0)
                await _serverDbProvider.AddOrUpdateAsync(schemaName, tableName, dbRows);

            if(hotfixDbRows.Count > 0)
                await _serverDbProvider.AddOrUpdateAsync(_appConfig.HotfixesSchema, _appConfig.HotfixDataTableName, hotfixDbRows.ToArray()); 
        }

        protected async Task DeleteAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            var schemaName = GetSchemaNameOfEntity<T>();
            if(schemaName == _appConfig.HotfixesSchema)
            {
                var entities = await GetAsync<T>(parameters);
                var tableHash = GetTableHashOfEntity<T>();

                foreach(var entity in entities)
                {
                    var dbParameters = new DbParameter[] { new DbParameter(nameof(HotfixData.RecordId), entity.GetId()), new DbParameter(nameof(HotfixData.TableHash), tableHash) };

                    var hotfixData = await GetSingleAsync<HotfixData>(dbParameters);
                    if(hotfixData != null)
                    {
                        hotfixData.Status = HotfixStatuses.RECORD_REMOVED;
                        await SaveAsync(hotfixData);
                    }
                }
            }
            await _serverDbProvider.DeleteAsync(schemaName, GetTableNameOfEntity<T>(), parameters);
        }

        protected async Task<int> GetNextIdAsync<T>()
            where T : new()
        {
            return 1337;
            return await _serverDbProvider.GetNextIdAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), FromId);
        }

        protected async Task<int> GetNextIdAsync(string tableName)
        {
            return 1337;
            return await _serverDbProvider.GetNextIdAsync(_appConfig.HotfixesSchema, tableName, 1);
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

        protected async Task<HotfixModsEntity> GetHotfixModsEntity(int id)
        {
            return await GetSingleAsync<HotfixModsEntity>(new DbParameter(nameof(HotfixModsEntity.RecordId), id), new DbParameter(nameof(HotfixModsEntity.VerifiedBuild), VerifiedBuild)) ?? new();
        }
    }
}
