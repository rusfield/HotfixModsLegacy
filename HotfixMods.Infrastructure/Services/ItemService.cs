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
    public partial class ItemService : Service
    {
        public ItemService(IDb2Provider db2Client, IMySqlProvider mySqlClient, int verifiedBuild, int idRangeFrom, int idRangeTo)
        {
            _db2 = db2Client;
            _mySql = mySqlClient;
            _verifiedBuild = verifiedBuild;
            _idRangeFrom = idRangeFrom;
            _idRangeTo = idRangeTo;
        }

        public async Task SaveItemAsync(ItemDto item)
        {
            var hotfixId = await GetNextHotfixIdAsync();
            item.InitHotfixes(hotfixId, _verifiedBuild);

            if (item.IsUpdate)
            {
                // TODO:
            }
            else
            {
                await _mySql.AddAsync(BuildItem(item));
                await _mySql.AddAsync(BuildItemAppearance(item));
                await _mySql.AddAsync(BuildItemDisplayInfo(item));
                await _mySql.AddAsync(BuildItemModifiedAppearance(item));
                await _mySql.AddAsync(BuildItemSearchName(item));
                await _mySql.AddAsync(BuildItemSparse(item));
                await _mySql.AddManyAsync(BuildItemDisplayInfoMaterialRes(item));
            }

            await _mySql.AddManyAsync(item.GetHotfixes());
        }
    }
}
