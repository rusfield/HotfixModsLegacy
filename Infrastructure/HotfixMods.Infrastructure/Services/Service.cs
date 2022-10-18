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
        public string Db2Path { get; } = @"C:\hotfixMods";

        IServerDbDefinitionProvider _serverDbDefinitionProvider;
        IClientDbDefinitionProvider _clieIClientDbDefinitionProvider;
        IServerDbProvider _serverDbProvider;
        IClientDbProvider _clientDbProvider;
        
        public Service(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider)
        {
            _serverDbDefinitionProvider = serverDbDefinitionProvider;
            _clieIClientDbDefinitionProvider = clientDbDefinitionProvider;
            _serverDbProvider = serverDbProvider;
            _clientDbProvider = clientDbProvider;
        }

        public async Task<IEnumerable<DbRow>> GetHotfixEntitiesAsync(string tableName, params DbParameter[] parameters)
        {
            var result = await _serverDbProvider.GetAsync(HotfixesSchema, tableName, parameters);
            if(!result.Any())
                result = await _clientDbProvider.GetAsync(Db2Path, tableName, parameters);

            return result;
        }

        protected async Task<T?> GetSingleAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            return (await _serverDbProvider.GetSingleAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), parameters) ?? await _clientDbProvider.GetSingleAsync(Db2Path, nameof(T), parameters)).DbRowToEntity<T?>();
        }

        protected async Task<DbRow?> GetSingleAsync(string tableName, params DbParameter[] parameters)
        {
            return await _serverDbProvider.GetSingleAsync(HotfixesSchema, tableName, parameters) ?? await _clientDbProvider.GetSingleAsync(Db2Path, tableName, parameters);
        }

        protected async Task<IEnumerable<T>> GetAsync<T>(params DbParameter[] parameters)
            where T : new()
        {
            var results = await _serverDbProvider.GetAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), parameters);
            if(!results.Any())
                results = await _clientDbProvider.GetAsync(Db2Path, nameof(T), parameters);

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
                var entities = (await _serverDbProvider.GetAsync(schemaName, GetTableNameOfEntity<T>(), parameters)).DbRowsToEntities<T>().Cast<IHotfixesSchema>();
                var tableHash = GetTableHashOfEntity<T>();
                foreach(var entity in entities)
                {
                    var hotfixData = new HotfixData()
                    {
                        Id = await GetNextHotfixEntityIdAsync<HotfixData>(),
                        RecordId = entity.Id,
                        UniqueId = -1,
                        Status = HotfixStatuses.RECORD_REMOVED,
                        TableHash = tableHash,
                        VerifiedBuild = VerifiedBuild
                    };
                    await _serverDbProvider.AddOrUpdateAsync(schemaName, GetTableNameOfEntity<HotfixData>(), hotfixData.EntityToDbRow()!);
                    await _serverDbProvider.DeleteAsync(schemaName, GetTableNameOfEntity<T>(), new DbParameter(nameof(IHotfixesSchema.Id), entity.Id));
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
