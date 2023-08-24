using HotfixMods.Providers.Interfaces;
using HotfixMods.Providers.Models;

namespace HotfixMods.Providers.WowDev.Client
{
    public partial class Db2Client : IClientDbProvider, IClientDbDefinitionProvider
    {
        public string Db2Folder { get; set; }
        public string DbdFolder { get; set; }
        public Db2Client(string db2Folder, string dbdFolder)
        {
            if (db2Folder.EndsWith("/"))
                db2Folder = db2Folder.Substring(0, db2Folder.Length - 1);

            if (dbdFolder.EndsWith("/"))
                dbdFolder = dbdFolder.Substring(0, dbdFolder.Length - 1);

            Db2Folder = db2Folder;
            DbdFolder = dbdFolder;
        }

        public async Task<bool> Db2ExistsAsync(string db2Name)
        {
            db2Name += ".db2";
            string filePath = Path.Combine(Db2Folder, db2Name);
            return await Task.Run(() =>
                File.Exists(filePath)
            );
        }

        public Task<IEnumerable<DbRow>> GetAsync(string db2Name, DbRowDefinition dbRowDefinition, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<DbRowDefinition?> GetDefinitionAsync(string db2Name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetAvailableDb2Async()
        {
            return await Task.Run(() =>
                Directory.GetFiles(Db2Folder).Select(file => Path.GetFileNameWithoutExtension(file))
            );
        }

        public Task<DbRow> GetSingleAsync(string db2Name, DbRowDefinition dbRowDefinition, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
