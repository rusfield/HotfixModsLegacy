using HotfixMods.Providers.Models;

namespace HotfixMods.Providers.Interfaces
{
    public interface IClientDbProvider
    {
        /// <summary>
        /// Returns a list of rows from a source, based on what provider is being used. Source can be for example a MySql database or a DB2 file.
        /// </summary>
        /// <param name="db2Name">Name of DB2</param>
        /// <param name="DbRowDefinition">Definition of Db2 (how the DbRow should look after retrieving)</param>
        /// <param name="parameters">Query and filter parameters</param>
        /// <returns></returns>
        Task<PagedDbResult> GetAsync(string db2Name, DbRowDefinition dbRowDefinition, int pageIndex, int pageSize, params DbParameter[] parameters);
        Task<DbRow?> GetSingleAsync(string db2Name, DbRowDefinition dbRowDefinition, params DbParameter[] parameters);
        Task<bool> Db2ExistsAsync(string db2Name);
        Task<IEnumerable<string>> GetAvailableNamesAsync();

    }
}
