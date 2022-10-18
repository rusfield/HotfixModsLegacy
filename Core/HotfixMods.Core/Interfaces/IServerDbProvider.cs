using HotfixMods.Core.Models;

namespace HotfixMods.Core.Interfaces
{
    public interface IServerDbProvider
    {
        Task<bool> IsAvailableAsync();
        Task<IEnumerable<DbRow>> GetAsync(string schemaName, string tableName, IDictionary<string, object> parameters);
        Task<DbRow> GetSingleAsync(string schemaName, string tableName, IDictionary<string, object> parameters);
        Task AddOrUpdateAsync(string schemaName, string tableName, params DbRow[] dbRows);
        Task DeleteAsync(string schemaName, string tableName, IDictionary<string, object> parameters);
    }
}
