using DBCD.Providers;

namespace HotfixMods.Providers.WowDev.Libs.Internal
{
    internal class DbdProvider : IDBDProvider
    {
        Stream _stream;

        public DbdProvider(Stream rawDbDef)
        {
            _stream = rawDbDef;
        }

        public Stream StreamForTableName(string db2Name, string build = null)
        {
            throw new NotImplementedException();
        }
    }
}
