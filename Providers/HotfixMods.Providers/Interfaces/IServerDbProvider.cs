using HotfixMods.Providers.Models;

namespace HotfixMods.Providers.Interfaces
{
    public interface IServerDbProvider
    {
        Task<bool> IsAvailableAsync();
        Task<PagedDbResult> GetAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition, int pageIndex, int pageSize, params DbParameter[] parameters);
        Task<DbRow> GetSingleAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition, params DbParameter[] parameters);
        Task AddOrUpdateAsync(string schemaName, string tableName, params DbRow[] dbRows);
        Task DeleteAsync(string schemaName, string tableName, params DbParameter[] parameters);
        Task CreateTableIfNotExistsAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition);
        Task<bool> TableExistsAsync(string schemaName, string tableName);
        Task<bool> SchemaExistsAsync(string schemaName);
        Task<string> GetHighestIdAsync(string schemaName, string tableName, string minId, string maxId, string idPropertyName = "id");
    }
}
