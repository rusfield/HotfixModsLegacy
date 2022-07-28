using HotfixMods.Core.Providers;
using HotfixMods.Db2Provider.WowToolsFiles.Clients;
using HotfixMods.Infrastructure.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService
    {
        IDb2Provider _db2Client;
        IMySqlProvider _mySqlClient;
        int _verifiedBuild;

        public ItemService(IDb2Provider db2Client, IMySqlProvider mySqlClient, int verifiedBuild)
        {
            _db2Client = db2Client;
            _mySqlClient = mySqlClient;
            _verifiedBuild = verifiedBuild;
        }

        public async Task SaveItemAsync(ItemDto item)
        {

        }
    }
}
