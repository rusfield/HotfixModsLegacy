using HotfixMods.Core.Attributes;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Helpers;
using HotfixMods.Providers.Models;
using System.Reflection;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ServiceBase
    {
        protected void DefaultCallback(string title, string subtitle, int progress)
        {
            Console.WriteLine($"{progress} %: {title} => {subtitle}");
        }

        protected int DefaultProgress()
        {
            return -1;
        }

        protected string? GetSchemaNameOfEntity<T>(bool errorOnNotFound = true)
            where T : new()
        {
            return GetSchemaNameOfType(typeof(T), errorOnNotFound);
        }

        protected string? GetSchemaNameOfType(Type type, bool errorOnNotFound = true)
        {
            if (type.GetCustomAttribute(typeof(HotfixesSchemaAttribute)) != null)
                return _appConfig.HotfixesSchema;

            if (type.GetCustomAttribute(typeof(WorldSchemaAttribute)) != null)
                return _appConfig.WorldSchema;

            if (type.GetCustomAttribute(typeof(CharactersSchemaAttribute)) != null)
                return _appConfig.CharactersSchema;

            if (errorOnNotFound)
                throw new Exception($"{type.Name} is missing Schema Attribute");
            else
                return null;
        }

        protected string GetTableNameOfEntity<T>()
            where T : new()
        {
            return Activator.CreateInstance<T>().ToTableName();
        }

        protected string GetTableNameOfType(Type type)
        {
            return type.Name.ToTableName();
        }

        TableHashes GetTableHashOfEntity<T>()
            where T : new()
        {
            return Enum.Parse<TableHashes>(GetTableNameOfEntity<T>(), true);
        }

        string GetIdPropertyNameOfEntity<T>()
            where T : new()
        {
                // TODO
        }

        protected async Task<T> GetNextIdAsync<T>(T fromId, T toId)
            where T : new()
        {
            return await GetNextIdAsync(GetSchemaNameOfEntity<T>(), GetTableNameOfEntity<T>(), fromId, toId, GetIdPropertyNameOfEntity<T>());
        }

        protected async Task<T> GetNextIdAsync(string db2Name)
        {
            return await GetNextIdAsync(_appConfig.HotfixesSchema, db2Name.ToTableName(), FromId, ToId, "id");
        }

        async Task<T> GetNextIdAsync<T>(string schemaName, string tableName, string fromId, string toId, string idPropertyName)
        {
            var highestIdString = await _serverDbProvider.GetHighestIdAsync(schemaName, tableName, fromId, toId, idPropertyName);
            try
            {
                var highestId = ulong.Parse(highestIdString);
                var from = ulong.Parse(fromId);
                var to = ulong.Parse(toId);

                if (highestId > 0)
                {
                    if (highestId == to)
                    {
                        throw new Exception("Database is full.");
                    }
                    return (T)Convert.ChangeType((highestId + 1), typeof(T));
                }
                else
                {
                    return (T)Convert.ChangeType(from, typeof(T));
                }
            }
            catch(Exception e)
            {
                // TODO
                throw e;
            }
        }

        protected async Task<List<string>> GetClientDefinitionNamesAsync()
        {
            return (await _clientDbDefinitionProvider.GetDefinitionNamesAsync()).ToList();
        }

        protected async Task<bool> Db2ExistsAsync(string clientDbLocation, string serverSchemaName, string db2Name)
        {
            return await _clientDbProvider.Db2ExistsAsync(db2Name) || await _serverDbProvider.TableExistsAsync(serverSchemaName, db2Name);
        }

        protected async Task<bool> TableExistsAsync(string schemaName, string tableName)
        {
            return await _serverDbProvider.TableExistsAsync(schemaName, tableName);
        }

        protected async Task<bool> SchemaExistsAsync(string schemaName)
        {
            return await _serverDbProvider.SchemaExistsAsync(schemaName);
        }

        protected async Task<HotfixModsEntity> GetExistingOrNewHotfixModsEntityAsync(Action<string, string, int> callback, Func<int> progress, int entityId)
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(HotfixModsEntity).Name}", progress());
            return await GetExistingOrNewHotfixModsEntityAsync(entityId);
        }

        protected async Task<HotfixModsEntity> GetExistingOrNewHotfixModsEntityAsync(int entityId)
        {
            var entity = await GetSingleAsync<HotfixModsEntity>(DefaultCallback, DefaultProgress, true, new DbParameter(nameof(HotfixModsEntity.RecordID), entityId), new DbParameter(nameof(HotfixModsEntity.VerifiedBuild), VerifiedBuild));
            if (null == entity)
            {
                entity = new()
                {
                    ID = 0,
                    Name = "",
                    RecordID = entityId,
                    VerifiedBuild = VerifiedBuild
                };
            }
            return entity;
        }

        protected async Task<int> GetNextHotfixModsEntityIdAsync()
        {
            return await GetNextIdAsync(_appConfig.HotfixesSchema, GetTableNameOfEntity<HotfixModsEntity>(), 0, int.MaxValue, nameof(HotfixModsEntity.ID));
        }

        protected void HandleException(Exception exception)
        {
            _exceptionHandler.Handle(exception);
        }

        protected async Task<DbRowDefinition?> GetDefinitionFromClientAsync(string db2Name)
        {
            return await _clientDbDefinitionProvider.GetDefinitionAsync(db2Name);
        }

        protected async Task<DbRowDefinition?> GetDefinitionFromServerAsync(string schemaName, string tableName)
        {
            return await _serverDbDefinitionProvider.GetDefinitionAsync(schemaName, tableName);
        }

        protected async Task<int> GetIdByConditionsAsync<T>(int? currentId, bool isUpdate)
            where T : new()
        {
            // Entity is null, and this ID will not be used.
            if (null == currentId)
                return 0;

            // Entity is new, or entity should be saved as new
            // Also check if entity is HotfixModsEntity, which does not use the FromId/ToId rules
            if ((int)currentId == 0 || !isUpdate)
            {
                if (typeof(T) == typeof(HotfixModsEntity))
                    return await GetNextHotfixModsEntityIdAsync();
                else
                    return await GetNextIdAsync<T>();
            }


            // Entity is being updated
            return (int)currentId;
        }
    }
}
