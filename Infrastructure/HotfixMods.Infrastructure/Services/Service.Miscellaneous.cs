using HotfixMods.Core.Enums;
using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.Business;

namespace HotfixMods.Infrastructure.Services
{
    public partial class Service
    {
        void DefaultProgressCallback(string title, string subtitle, int progress)
        {
            Console.WriteLine($"{progress} %: {title} => {subtitle}");
        }


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
            return Activator.CreateInstance<T>().EntityToDbRowDefinition();
        }
    }
}
