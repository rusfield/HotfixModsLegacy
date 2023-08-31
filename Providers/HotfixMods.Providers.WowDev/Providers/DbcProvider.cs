using DBCD.Providers;

namespace HotfixMods.Providers.WowDev.Providers
{
    internal class DbcProvider : IDBCProvider
    {
        public DbcProvider(string db2FolderPath)
        {
            _db2FolderPath = db2FolderPath;
        }

        string _db2FolderPath;

        public Stream StreamForTableName(string db2Name, string build)
        {
            db2Name += ".db2";
            string db2Path = Path.Combine(_db2FolderPath, db2Name);
            if (File.Exists(db2Path))
            {
                return File.Open(db2Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else
            {
                throw new FileNotFoundException("Could not find " + db2Name);
            }
        }
    }
}
