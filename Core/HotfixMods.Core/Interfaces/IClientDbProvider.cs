using HotfixMods.Core.Models;

namespace HotfixMods.Core.Interfaces
{
    public interface IClientDbProvider
    {
        Task<bool> IsAvailableAsync();
        Task<IEnumerable<DbRow>> GetAsync(string pathOrSchemaName, string fileOrTableName, IDictionary<string, object> parameters);
        Task<DbRow> GetSingleAsync(string pathOrSchemaName, string fileOrTableName, IDictionary<string, object> parameters);
        Task AddOrUpdateAsync(string pathOrSchemaName, string fileOrTableName, params DbRow[] dbRows);
    }
}
