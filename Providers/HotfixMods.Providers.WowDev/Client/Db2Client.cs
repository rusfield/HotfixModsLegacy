using HotfixMods.Providers.Interfaces;
using HotfixMods.Providers.Models;

namespace HotfixMods.Providers.WowDev.Client
{
    public partial class Db2Client : IClientDbProvider, IClientDbDefinitionProvider
    {
        public string Db2Folder { get; set; }
        public string DbdFolder { get; set; }
        public string Build { get; set; }
        public Db2Client(string db2Folder, string dbdFolder, string build)
        {
            Db2Folder = db2Folder.TrimEnd('/');
            DbdFolder = dbdFolder.TrimEnd('/');
            Build = build; 
        }

        public async Task<bool> Db2ExistsAsync(string db2Name)
        {
            db2Name += ".db2";
            string filePath = Path.Combine(Db2Folder, db2Name);
            return await Task.Run(() =>
                File.Exists(filePath)
            );
        }

        public async Task<PagedDbResult> GetAsync(string db2Name, DbRowDefinition dbRowDefinition, int pageIndex = 0, int pageSize = int.MaxValue, params DbParameter[] parameters)
        {
            if (!await Db2ExistsAsync(db2Name))
                return new(pageIndex, pageSize, 0);

            return await ReadDb2FileAsync(db2Name, dbRowDefinition, 0, 1, parameters);
        }

        public async Task<DbRowDefinition?> GetDefinitionAsync(string db2Name)
        {
            return await GetDbDefinitionByDb2Name(db2Name);
        }

        public async Task<IEnumerable<string>> GetAvailableNamesAsync()
        {
            return await Task.Run(() =>
                Directory.GetFiles(Db2Folder).Select(file => Path.GetFileNameWithoutExtension(file))
            );
        }

        public async Task<DbRow?> GetSingleAsync(string db2Name, DbRowDefinition dbRowDefinition, params DbParameter[] parameters)
        {
            if (!await Db2ExistsAsync(db2Name))
                return null;

            var pagedResult = await ReadDb2FileAsync(db2Name, dbRowDefinition, 0, 1, parameters);
            if(pagedResult.TotalRowCount > 1)
            {
                // Consider informing this to user. Only the first result will be returned.
            }
            return pagedResult.Rows.FirstOrDefault();
        }
    }
}
