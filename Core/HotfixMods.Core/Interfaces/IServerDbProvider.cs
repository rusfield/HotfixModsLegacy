using HotfixMods.Core.Models;

namespace HotfixMods.Core.Interfaces
{
    public interface IServerDbProvider
    {
        Task<bool> IsAvailableAsync();
        Task<IEnumerable<DbRow>> GetAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition, params DbParameter[] parameters);
        Task<DbRow> GetSingleAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition, params DbParameter[] parameters);
        Task AddOrUpdateAsync(string schemaName, string tableName, params DbRow[] dbRows);
        Task DeleteAsync(string schemaName, string tableName, params DbParameter[] parameters);
        Task CreateTableIfNotExistsAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition);
        Task<bool> TableExistsAsync(string schemaName, string tableName);
        Task<bool> SchemaExistsAsync(string schemaName);
        Task<int> GetHighestIdAsync(string schema, string tableName, int minId, int maxId, string idPropertyName = "id");
    }
}
