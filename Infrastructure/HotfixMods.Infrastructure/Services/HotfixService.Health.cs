using HotfixMods.Core.Attributes;
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
                    var tableName = GetTableNameOfType(type);
                    var schemaName = GetSchemaNameOfType(type);
                    if (await TableExists(schemaName, tableName))
                    {
                        if (nameof(HotfixesSchemaAttribute).StartsWith(schemaName))
                        {
                            
                        }
                        else if (nameof(CharactersSchemaAttribute).StartsWith(schemaName))
                        {

                        }
                        else if (nameof(WorldSchemaAttribute).StartsWith(schemaName))
                        {

                        }
                        else
                        {
                            // TODO
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
    }
}
