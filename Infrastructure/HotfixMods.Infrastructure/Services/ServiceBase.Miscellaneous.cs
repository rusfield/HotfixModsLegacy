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

        protected string GetSchemaNameOfEntity<T>()
            where T : new()
        {
            return GetSchemaNameOfType(typeof(T));
        }

        protected string GetSchemaNameOfType(Type type)
        {
            if (type.GetCustomAttribute(typeof(HotfixesSchemaAttribute)) != null)
                return _appConfig.HotfixesSchema;

            if (type.GetCustomAttribute(typeof(WorldSchemaAttribute)) != null)
                return _appConfig.WorldSchema;

            if (type.GetCustomAttribute(typeof(CharactersSchemaAttribute)) != null)
                return _appConfig.CharactersSchema;

            throw new Exception($"{type.Name} is missing Schema Attribute");
        }

        protected async Task<DbRowDefinition?> GetDefinitionOfEntity<T>()
            where T : new()
        {
            var schema = GetSchemaNameOfEntity<T>();
            var table = typeof(T).Name;
            DbRowDefinition? rowDefinition = null;
            if (schema.Equals(_appConfig.HotfixesSchema, StringComparison.InvariantCultureIgnoreCase))
            {
                rowDefinition = await GetDefinitionFromClientAsync(table);
            }
            else
            {
                rowDefinition = await GetDefinitionFromServerAsync(schema, table);
            }
            return rowDefinition;
        }

        protected string GetTableNameOfType(Type type)
        {
            return type.Name.ToTableName();
        }

        protected async Task<ulong> GetNextIdAsync<T>()
            where T : new()
        {
            var result = await GetNextIdAsync(GetSchemaNameOfEntity<T>(), typeof(T).Name);
            return ulong.Parse(result);
        }



        async Task<string> GetNextIdAsync(string schemaName, string tableName)
        {
            var fromIdString = "1";
            var toIdString = "1";
            var customRange = _appConfig.CustomRanges.Where(c => c.Table.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var definition = await GetDefinitionFromServerAsync(schemaName, tableName);
            var idColumn = definition.ColumnDefinitions.First(p => p.IsIndex);

            if (customRange != null)
            {
                fromIdString = customRange.FromId.ToString();
                toIdString = customRange.ToId.ToString();
            }
            else
            {
                toIdString = GetMaxValue(idColumn.Type);
            }

            var highestIdString = await _serverDbProvider.GetHighestIdAsync(schemaName, tableName, fromIdString, toIdString, idColumn.Name);

            try
            {
                var highestId = ulong.Parse(highestIdString);
                var from = ulong.Parse(fromIdString);
                var to = ulong.Parse(toIdString);

                if (highestId > 0)
                {
                    if (highestId == to)
                    {
                        throw new Exception("Database is full.");
                    }
                    return (highestId + 1).ToString();
                }
                else
                {
                    return from.ToString();
                }
            }
            catch (Exception e)
            {
                // TODO
                throw e;
            }
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

        protected async Task<HotfixModsEntity> GetExistingOrNewHotfixModsEntityAsync(Action<string, string, int> callback, Func<int> progress, ulong entityId)
        {
            callback.Invoke(LoadingHelper.Loading, $"Loading {typeof(HotfixModsEntity).Name}", progress());
            return await GetExistingOrNewHotfixModsEntityAsync(entityId);
        }

        protected async Task<HotfixModsEntity> GetExistingOrNewHotfixModsEntityAsync(ulong entityId)
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

        protected async Task<ulong> GetIdByConditionsAsync<T>(ulong? currentId, bool isUpdate)
            where T : new()
        {
            // Entity is null, and this ID will not be used.
            if (null == currentId)
                return 0;

            // Entity is new, or entity should be saved as new
            if (currentId == 0 || !isUpdate)
            {
                return await GetNextIdAsync<T>();
            }

            // Entity is being updated
            return (ulong)currentId;
        }

        protected async Task<IEnumerable<string>> GetAvailableDb2sAsync()
        {
            return await _clientDbProvider.GetAvailableNamesAsync();
        }

        string GetMaxValue(Type type)
        {
            return type.ToString() switch
            {
                "System.SByte" => sbyte.MaxValue.ToString(),
                "System.Int16" => short.MaxValue.ToString(),
                "System.Int32" => int.MaxValue.ToString(),
                "System.Int64" => long.MaxValue.ToString(),
                "System.Byte" => byte.MaxValue.ToString(),
                "System.UInt16" => ushort.MaxValue.ToString(),
                "System.UInt32" => uint.MaxValue.ToString(),
                "System.UInt64" => ulong.MaxValue.ToString(),
                _ => throw new Exception($"Max value of {type} not implemented.")
            };
        }
    }
}
