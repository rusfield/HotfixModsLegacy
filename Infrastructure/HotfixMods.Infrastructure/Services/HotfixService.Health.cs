using HotfixMods.Core.Attributes;
using HotfixMods.Core.Enums.TrinityCore;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.AggregateModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using System.Reflection;

namespace HotfixMods.Infrastructure.Services
{
    public partial class HotfixService
    {
        const string missingStatus = "{0} is missing in {1}.";
        const string countMismatchStatus = "DB2 fields for {0} do not match MySQL fields for {1}. Count mismatch.";
        const string typeMismatchStatus = "DB2 fields for {0} do not match MySQL fields for {1}. Field {2}";
        const string schemaErrorStatus = "Unable to find any {0} schema named {1}.";

        public async Task<DbRowDefinition> GetDefinitionAsync(string db2Name)
        {
            return await GetDefinitionFromClientAsync(db2Name);
        }

        public async Task<List<HealthModel>> CheckServerAsync()
        {
            var results = new List<HealthModel>();
            try
            {
                if(!await SchemaExistsAsync(_appConfig.CharactersSchema))
                {
                    results.Add(new()
                    {
                        Type = null,
                        Status = HealthModel.HealthErrorStatus.ERROR,
                        Description = string.Format(schemaErrorStatus, "Characters", _appConfig.CharactersSchema)
                    });
                }

                if (!await SchemaExistsAsync(_appConfig.HotfixesSchema))
                {
                    results.Add(new()
                    {
                        Type = null,
                        Status = HealthModel.HealthErrorStatus.ERROR,
                        Description = string.Format(schemaErrorStatus, "Hotfixes", _appConfig.HotfixesSchema)
                    });
                }

                if (!await SchemaExistsAsync(_appConfig.WorldSchema))
                {
                    results.Add(new()
                    {
                        Type = null,
                        Status = HealthModel.HealthErrorStatus.ERROR,
                        Description = string.Format(schemaErrorStatus, "WorldSchema", _appConfig.WorldSchema)
                    });
                }
            }
            catch(Exception ex)
            {
                results.Add(new()
                {
                    Type = null,
                    Status = HealthModel.HealthErrorStatus.ERROR,
                    Description = "There is a problem connecting to MySQL. Please make sure the service is running and verify in Settings that the connection info is correct."
                });
            }
            return results;
        }

        public async Task<HealthModel?> CheckSingleModelHealthAsync(Type type)
        {
            // TODO: Special logic
            if(type == typeof(HotfixData))
            {
                return null;
            }
            else if (type == typeof(HotfixModsEntity))
            {
                return null;
            }

            var tableName = GetTableNameOfType(type);
            var schemaName = GetSchemaNameOfType(type, false);
            var properties = type.GetProperties();

            DbRowDefinition serverDefinition = new(type.Name);
            DbRowDefinition clientDefinition = new(type.Name);

            if (null == schemaName)
            {
                // These are for Client Only DB2s.
                // HotfixMods will serve as server

                serverDefinition = type.TypeToDbRowDefinition() ?? serverDefinition;
                clientDefinition = await GetDefinitionFromClientAsync(type.Name);
            }
            else if (await TableExistsAsync(schemaName, tableName))
            {
                if (nameof(HotfixesSchemaAttribute).StartsWith(schemaName, StringComparison.OrdinalIgnoreCase))
                {
                    // Normal 

                    serverDefinition = await GetDefinitionFromServerAsync(_appConfig.HotfixesSchema, tableName);
                    clientDefinition = await GetDefinitionFromClientAsync(type.Name);
                }
                else if (nameof(CharactersSchemaAttribute).StartsWith(schemaName, StringComparison.OrdinalIgnoreCase))
                {
                    // HotfixMods will serve as client

                    serverDefinition = await GetDefinitionFromServerAsync(_appConfig.CharactersSchema, tableName);
                    clientDefinition = type.TypeToDbRowDefinition() ?? clientDefinition;
                }
                else if (nameof(WorldSchemaAttribute).StartsWith(schemaName, StringComparison.OrdinalIgnoreCase))
                {
                    // HotfixMods will serve as client

                    serverDefinition = await GetDefinitionFromServerAsync(_appConfig.WorldSchema, tableName);
                    clientDefinition = type.TypeToDbRowDefinition() ?? clientDefinition;
                }
                else
                {
                    throw new Exception($"Unexpected schema name/attribute {schemaName}.");
                }

                if (serverDefinition.ColumnDefinitions.Count == clientDefinition.ColumnDefinitions.Count && clientDefinition.ColumnDefinitions.Count == properties.Count())
                {
                    for (int i = 0; i < properties.Count(); i++)
                    {
                        var serverType = serverDefinition.ColumnDefinitions[i].Type;
                        var clientType = clientDefinition.ColumnDefinitions[i].Type;
                        if (IsParentIndexField(properties[i]))
                        {
                            if (GetUnsignedType(serverType) == GetUnsignedType(clientType))
                                continue;
                        }
                        else
                        {
                            if (serverType == clientType)
                                continue;
                        }
                        return new()
                        {
                            Type = type,
                            Status = HealthModel.HealthErrorStatus.MISMATCH,
                            Description = string.Format(typeMismatchStatus, type.Name, tableName, properties[i].Name)
                        };
                    }
                }
                else
                {
                    return new()
                    {
                        Type = type,
                        Status = HealthModel.HealthErrorStatus.MISMATCH,
                        Description = string.Format(countMismatchStatus, type.Name, tableName)
                    };
                }
            }
            else
            {
                return new()
                {
                    Type = type,
                    Status = HealthModel.HealthErrorStatus.MISSING,
                    Description = string.Format(missingStatus, tableName, schemaName)
                };
            }
            return null;
        }




        public async Task<List<HealthModel>> CheckModelHealthAsync()
        {
            var result = new List<HealthModel>();
            var types = GetAllModels();
            foreach (var type in types)
            {
                // Devs will check Client Only models before releases.
                if (Attribute.IsDefined(type, typeof(ClientOnlyAttribute)))
                    continue;

                var healthModel = await CheckSingleModelHealthAsync(type);
                if (healthModel != null)
                    result.Add(healthModel);
            }
            return result;
        }

        List<Type> GetAllModels()
        {
            var assembly = Assembly.Load("HotfixMods.Core");
            var types = new List<Type>();
            foreach (Type type in assembly.ManifestModule.GetTypes())
            {
                if (type.Namespace == "HotfixMods.Core.Models.Db2" || type.Namespace == "HotfixMods.Core.Models.TrinityCore")
                {
                    types.Add(type);
                }
            }
            return types;
        }

        List<Type> GetDtoTypes()
        {
            var assembly = Assembly.Load("HotfixMods.Infrastructure");
            var types = new List<Type>();
            foreach (Type type in assembly.ManifestModule.GetTypes())
            {
                if (type.Namespace == "HotfixMods.Infrastructure.DtoModels" && type.GetInterface(nameof(IDto)) != null)
                {
                    types.Add(type);
                }
            }
            return types;
        }

        List<Type> GetDtoClassProperties(Type type)
        {
            var externalClasses = new List<Type>();

            foreach (var property in type.GetProperties())
            {
                var propertyType = property.PropertyType;

                if (propertyType == typeof(List<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                }

                if (!propertyType.IsPrimitive && propertyType.Namespace != "System")
                {
                    if (propertyType.GetCustomAttribute<HotfixesSchemaAttribute>() != null)
                    {
                        externalClasses.Add(propertyType);
                    }

                    externalClasses.AddRange(GetDtoClassProperties(propertyType));
                }
            }

            foreach (var innerType in type.GetNestedTypes())
            {
                if (innerType.IsClass && innerType.IsNestedPublic)
                {
                    externalClasses.AddRange(GetDtoClassProperties(innerType));
                }
            }

            return externalClasses.Distinct().ToList();
        }

        bool IsParentIndexField(PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes(true).OfType<Attribute>();
            foreach (var attribute in attributes)
            {
                var attrType = attribute.GetType();
                if (attrType.Name.Contains("parentindexfield", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        Type GetUnsignedType(Type type)
        {
            var name = type.Name.ToLower();
            type = (name) switch
            {
                "int8" => typeof(byte),
                "int16" => typeof(ushort),
                "int32" => typeof(uint),
                "int64" => typeof(ulong),
                _ => type
            };

            return type;
        }
    }
}
