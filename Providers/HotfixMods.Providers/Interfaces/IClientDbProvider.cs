using HotfixMods.Providers.Models;

namespace HotfixMods.Providers.Interfaces
{
    public interface IClientDbProvider
    {
        /// <summary>
        /// Returns a list of rows from a source, based on what provider is being used. Source can be for example a MySql database or a DB2 file.
        /// </summary>
        /// <param name="db2Name">Name of DB2</param>
        /// <param name="parameters">Query and filter parameters</param>
        /// <returns></returns>
        Task<IEnumerable<DbRow>> GetAsync(string db2Name, DbRowDefinition dbRowDefinition, params DbParameter[] parameters);
        Task<DbRow> GetSingleAsync(string db2Name, DbRowDefinition dbRowDefinition, params DbParameter[] parameters);
        Task<bool> Db2ExistsAsync(string db2Name);
    }
}
