using HotfixMods.Core.Models;

namespace HotfixMods.Core.Interfaces
{
    public interface IClientDbProvider
    {
        /// <summary>
        /// Returns a list of rows from a source, based on what provider is being used. Source can be for example a MySql database or a DB2 file.
        /// </summary>
        /// <param name="location">For example schema name for MySql or path for DB2 file.</param>
        /// <param name="db2Name">Name of DB2 table. Provider should format appropriately with underscores if needed.</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<IEnumerable<DbRow>> GetAsync(string location, string db2Name, DbRowDefinition dbRowDefinition, params DbParameter[] parameters);
        Task<DbRow> GetSingleAsync(string location, string db2Name, DbRowDefinition dbRowDefinition, params DbParameter[] parameters);
    }
}
