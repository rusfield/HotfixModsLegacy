using HotfixMods.Providers.Models;

namespace HotfixMods.Providers.Interfaces
{
    public interface IServerDbProvider
    {
        /// <summary>
        /// Check whether this provider is available and can retrieve data (for example if MySql can connect)
        /// </summary>
        /// <returns>Returns true if provider works and is ready</returns>
        Task<bool> IsAvailableAsync();
        /// <summary>
        /// Get many DbRows.
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="tableName"></param>
        /// <param name="dbRowDefinition"></param>
        /// <param name="pageIndex">Use -1 to get all</param>
        /// <param name="pageSize">Use -1 to get all</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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
