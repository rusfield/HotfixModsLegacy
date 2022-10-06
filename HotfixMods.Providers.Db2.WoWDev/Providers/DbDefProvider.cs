using DBCD.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Providers.Db2.WoWDev.Providers
{
    internal class DbDefProvider : IDBDProvider
    {
        Stream _stream;

        public DbDefProvider(Stream rawDbDef)
        {
            _stream = rawDbDef;
        }

        public Stream StreamForTableName(string? tableName = null, string? build = null)
        {
            return _stream;
        }
    }
}
