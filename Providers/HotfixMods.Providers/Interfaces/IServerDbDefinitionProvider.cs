using HotfixMods.Providers.Models;

namespace HotfixMods.Providers.Interfaces
{
    public interface IServerDbDefinitionProvider
    {
        Task<DbRowDefinition?> GetDefinitionAsync(string schemaName, string tableName);
    }
}
