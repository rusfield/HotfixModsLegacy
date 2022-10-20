using HotfixMods.Core.Enums;
using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Business;

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
        public string Location { get; } = @"C:\hotfixMods";

        IServerDbDefinitionProvider _serverDbDefinitionProvider;
        IClientDbDefinitionProvider _clientDbDefinitionProvider;
        IServerDbProvider _serverDbProvider;
        IClientDbProvider _clientDbProvider;
        
        public Service(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider)
        {
            _serverDbDefinitionProvider = serverDbDefinitionProvider;
            _clientDbDefinitionProvider = clientDbDefinitionProvider;
            _serverDbProvider = serverDbProvider;
            _clientDbProvider = clientDbProvider;
        }

        protected async Task<T?> GetSingleAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            return (await _serverDbProvider.GetSingleAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), GetDbRowDefinitionOfEntity<T>(), parameters) ?? await _clientDbProvider.GetSingleAsync(Location, nameof(T), GetDbRowDefinitionOfEntity<T>(), parameters)).DbRowToEntity<T?>();
        }


        protected async Task<DbRow?> GetSingleAsync(string tableName, params DbParameter[] parameters)
        {
            DbRow? result = null;
            var serverDbDefinition = await _serverDbDefinitionProvider.GetDefinitionAsync(HotfixesSchema, tableName);
            if (serverDbDefinition != null)
                result = await _serverDbProvider.GetSingleAsync(HotfixesSchema, tableName, serverDbDefinition, parameters);

            if(null == result)
            {
                var clientDbDefinition = await _clientDbDefinitionProvider.GetDefinitionAsync(Location, tableName);
                if(clientDbDefinition != null)
                    result = await _clientDbProvider.GetSingleAsync(Location, tableName, clientDbDefinition, parameters);
            }

            return result;
        }
        

        protected async Task<IEnumerable<T>> GetAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            var results = await _serverDbProvider.GetAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), GetDbRowDefinitionOfEntity<T>(), parameters);
            if(!results.Any())
                results = await _clientDbProvider.GetAsync(Location, nameof(T), GetDbRowDefinitionOfEntity<T>(), parameters);

            return results.DbRowsToEntities<T>();
        }




        protected async Task SaveAsync<T>(params T[] entities)
            where T : new()
        {
            await SaveAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), entities.EntitiesToDbRows().ToArray());
        }

        protected async Task SaveAsync(string schemaName, string tableName, params DbRow[] dbRows)
        {
            await _serverDbProvider.AddOrUpdateAsync(schemaName, tableName, dbRows.ToArray());
        }

        protected async Task DeleteAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            var schemaName = GetSchemaNameOfEntity<T>();
            if(schemaName == nameof(IHotfixesSchema))
            {
                var entities = (await _serverDbProvider.GetAsync(schemaName, GetTableNameOfEntity<T>(), GetDbRowDefinitionOfEntity<T>(), parameters)).DbRowsToEntities<T>();
                var tableHash = GetTableHashOfEntity<T>();
                foreach(var entity in entities)
                {
                    var hotfixData = new HotfixData()
                    {
                        Id = await GetNextHotfixEntityIdAsync<HotfixData>(),
                        RecordId = entity.GetId(),
                        UniqueId = -1,
                        Status = HotfixStatuses.RECORD_REMOVED,
                        TableHash = tableHash,
                        VerifiedBuild = VerifiedBuild
                    };
                    await _serverDbProvider.AddOrUpdateAsync(schemaName, GetTableNameOfEntity<HotfixData>(), hotfixData.EntityToDbRow()!);
                    await _serverDbProvider.DeleteAsync(schemaName, GetTableNameOfEntity<T>(), parameters);
                }
            }
            else
            {
                await _serverDbProvider.DeleteAsync(schemaName, GetTableNameOfEntity<T>(), parameters);
            }
        }

        protected async Task DeleteAsync(string schemaName, string tableName, params DbParameter[] parameters)
        {
            await _serverDbProvider.DeleteAsync(schemaName, tableName, parameters);
        }

        public async Task<int> GetNextHotfixEntityIdAsync<T>()
            where T : new()
        {
            return 0;
        }


    }
}
