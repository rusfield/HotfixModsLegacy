using HotfixMods.Core.Models;

namespace HotfixMods.Core.Interfaces
{
    public interface IServerDbDefinitionProvider
    {
        Task<DbRowDefinition> GetDefinitionsAsync(string schemaName, string tableName);
    }
}
