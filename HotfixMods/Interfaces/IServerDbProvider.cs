using HotfixMods.Models;

namespace HotfixMods.Interfaces
{
    public interface IServerDbProvider
    {
        Task<bool> IsAvailableAsync();
        Task<IEnumerable<DbRow>> GetAsync(string schemaName, string tableName, string? whereClause = null);
        Task<DbRow> GetSingleAsync(string schemaName, string tableName, string? whereClause = null);
        Task AddOrUpdateAsync(string schemaName, string tableName, params DbRow[] dbRows);
        Task DeleteAsync(string schemaName, string tableName, string? whereClause = null);
    }
}
