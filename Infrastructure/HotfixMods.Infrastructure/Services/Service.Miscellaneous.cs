using HotfixMods.Core.Attributes;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.Business;
using System.Reflection;

namespace HotfixMods.Infrastructure.Services
{
    public partial class Service
    {
        void DefaultProgressCallback(string title, string subtitle, int progress)
        {
            Console.WriteLine($"{progress} %: {title} => {subtitle}");
        }

        /*
        string GetSchemaNameOfEntity<T>()
            where T : new()
        {
            if (typeof(T).GetInterface(nameof(IHotfixesSchema)) != null)
                return HotfixesSchema;
            if (typeof(T).GetInterface(nameof(ICharactersSchema)) != null)
                return CharactersSchema;
            if (typeof(T).GetInterface(nameof(IWorldSchema)) != null)
                return WorldSchema;
            if (typeof(T).GetInterface(nameof(IHotfixModsSchema)) != null)
                return HotfixModsSchema;

            throw new NotImplementedException("Entity is missing interface used for schema identification.");
        }
        */

        string GetSchemaNameOfEntity<T>()
            where T : new()
        {
            if (typeof(T).GetCustomAttribute(typeof(HotfixesSchemaAttribute)) != null)
                return _appConfig.HotfixesSchema;

            if (typeof(T).GetCustomAttribute(typeof(WorldSchemaAttribute)) != null)
                return _appConfig.WorldSchema;

            if (typeof(T).GetCustomAttribute(typeof(HotfixModsSchemaAttribute)) != null)
                return _appConfig.HotfixModsSchema;

            if (typeof(T).GetCustomAttribute(typeof(CharactersSchemaAttribute)) != null)
                return _appConfig.CharactersSchema;

            throw new Exception($"{typeof(T)} is missing Schema Attribute");
        }

        string GetTableNameOfEntity<T>()
            where T : new()
        {
            return Activator.CreateInstance<T>().ToTableName();
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

            var idAttributeProperties = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(false).Any(a => a.GetType() == typeof(IdAttribute)));
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
