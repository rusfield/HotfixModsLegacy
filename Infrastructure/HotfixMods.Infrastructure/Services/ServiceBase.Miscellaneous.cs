using HotfixMods.Core.Attributes;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.Extensions;
using System.Reflection;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ServiceBase
    {
        protected void DefaultProgressCallback(string title, string subtitle, int progress)
        {
            Console.WriteLine($"{progress} %: {title} => {subtitle}");
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

            if (type.GetCustomAttribute(typeof(HotfixModsSchemaAttribute)) != null)
                return _appConfig.HotfixModsSchema;

            if (type.GetCustomAttribute(typeof(CharactersSchemaAttribute)) != null)
                return _appConfig.CharactersSchema;

            throw new Exception($"{type.Name} is missing Schema Attribute");
        }

        protected string GetTableNameOfEntity<T>()
            where T : new()
        {
            return Activator.CreateInstance<T>().ToTableName();
        }

        protected string GetTableNameOfType(Type type)
        {
            return Activator.CreateInstance(type).ToTableName();
        }

        TableHashes GetTableHashOfEntity<T>()
            where T : new()
        {
            return Enum.Parse<TableHashes>(GetTableNameOfEntity<T>(), true);
        }

        DbRowDefinition GetDbRowDefinitionOfEntity<T>()
            where T : new()
        {
            return Activator.CreateInstance<T>().EntityToDbRowDefinition()!;
        }

        string GetIdPropertyNameOfEntity<T>()
            where T : new()
        {
            string idPropertyName = "id";
            var idProperties = typeof(T).GetProperties().Where(p => p.Name.Equals(idPropertyName, StringComparison.InvariantCultureIgnoreCase));
            if (idProperties.Count() > 1)
                throw new Exception($"{typeof(T).Name} contains multiple {idPropertyName} properties.");

            if (idProperties.Count() == 1)
            {
                return idProperties.First().Name;
            }

            var idAttributeProperties = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(false).Any(a => a.GetType() == typeof(IndexFieldAttribute)));
            if (idAttributeProperties.Count() > 1)
                throw new Exception($"{typeof(T).Name} contains multiple column attributes named {idPropertyName}.");

            if (idAttributeProperties.Count() == 1)
            {
                return idAttributeProperties.First().Name;
            }

            throw new Exception($"{typeof(T)} does not contain any {idPropertyName} properties");
        }
    }
}
