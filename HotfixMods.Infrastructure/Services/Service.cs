using HotfixMods.Core.Models;
using HotfixMods.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public abstract class Service
    {
        protected IDb2Provider _db2;
        protected IMySqlProvider _mySql;
        protected int _verifiedBuild;
        protected int _idRangeFrom;
        protected int _idRangeTo;

        public async Task<int> GetNextHotfixIdAsync(bool quickScan = true)
        {
            int id = _idRangeFrom;
            var hotfixIdsInRange = await _mySql.GetManyAsync<HotfixData>(c => c.Id >= _idRangeFrom && c.Id < _idRangeTo);
            if (hotfixIdsInRange.Count() > 0)
            {
                return hotfixIdsInRange.Max(c => c.Id) + 1;
            }
            return id;
        }
    }
}
