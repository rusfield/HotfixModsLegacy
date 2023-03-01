using HotfixMods.Core.Attributes;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.AggregateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class HotfixService
    {
        const string missingStatus = "{0} is missing in {1}.";
        const string countMismatchStatus = "DB2 fields for {0} do not match MySQL fields for {1}. Count mismatch.";
        const string typeMismatchStatus = "DB2 fields for {0} do not match MySQL fields for {1}. Field {2}";
        public async Task<bool> CheckDto<TDto>()
        {

            return false;
        }




        public async Task<List<HealthModel>> CheckDtoHealthAsync()
        {
            var result = new List<HealthModel>();
            var dtoTypes = GetDtoTypes();
            foreach (var dtoType in dtoTypes)
            {
                var dtoPropertyTypes = GetDtoClassProperties(dtoType);
                foreach (var type in dtoPropertyTypes)
                {
                    if (type == typeof(HotfixModsEntity))
                        continue;

                    var tableName = GetTableNameOfType(type);
                    var schemaName = GetSchemaNameOfType(type);
                    var properties = type.GetProperties();
                    if (await TableExists(schemaName, tableName))
                    {
                        if (nameof(HotfixesSchemaAttribute).StartsWith(schemaName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            var serverDefinition = await GetDefinitionFromServerAsync(schemaName, tableName);
                            var clientDefinition = await GetDefinitionFromClientAsync(type.Name);


                            if(serverDefinition.ColumnDefinitions.Count == clientDefinition.ColumnDefinitions.Count && clientDefinition.ColumnDefinitions.Count == properties.Count())
                            {
                                for(int i = 0; i < properties.Count(); i++)
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
                                    result.Add(new()
                                    {
                                        Type = type,
                                        Status = HealthModel.HealthStatus.MISMATCH,
                                        Description = string.Format(typeMismatchStatus, type.Name,  tableName, properties[i].Name)
                                    });
                                }
                            }
                            else
                            {
                                result.Add(new()
                                {
                                    Type = type,
                                    Status = HealthModel.HealthStatus.MISMATCH,
                                    Description = string.Format(countMismatchStatus, type.Name, tableName)
                                });
                            }
                        }
                        else if (nameof(CharactersSchemaAttribute).StartsWith(schemaName, StringComparison.CurrentCultureIgnoreCase))
                        {

                        }
                        else if (nameof(WorldSchemaAttribute).StartsWith(schemaName, StringComparison.CurrentCultureIgnoreCase))
                        {

                        }
                        else
                        {
                            throw new Exception($"{type.Name} is missing Schema attribute.");
                        }
                    }
                    else
                    {
                        result.Add(new()
                        {
                            Type = type,
                            Status = HealthModel.HealthStatus.MISSING,
                            Description = string.Format(missingStatus, tableName, schemaName)
                        });
                    }
                }
            }
            return result;
        }

        List<Type> GetDtoTypes()
        {
            var assembly = Assembly.Load("HotfixMods.Infrastructure");
            List<Type> types = new List<Type>();
            foreach (Type type in assembly.ManifestModule.GetTypes())
            {
                if (type.Namespace == "HotfixMods.Infrastructure.DtoModels")
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
