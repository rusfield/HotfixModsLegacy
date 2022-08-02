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
        public int VerifiedBuild { get; set; }
        public int IdRangeFrom { get; set; }
        public int IdRangeTo { get; set; }
        public double IdSize { get; set; }

        protected IDb2Provider _db2;
        protected IMySqlProvider _mySql;

        public Service(IDb2Provider db2Provider, IMySqlProvider mySqlProvider)
        {
            _db2 = db2Provider;
            _mySql = mySqlProvider;
        }

        public async Task<int> GetNextHotfixIdAsync(bool quickScan = true)
        {
            int id = IdRangeFrom;
            var hotfixIdsInRange = await _mySql.GetManyAsync<HotfixData>(c => c.Id >= IdRangeFrom && c.Id < IdRangeTo);
            if (hotfixIdsInRange.Count() > 0)
            {
                return hotfixIdsInRange.Max(c => c.Id) + 1;
            }
            return id;
        }

        public async Task<int> GetNextIdAsync(bool quickScan = true)
        {
            int id = IdRangeFrom;
            var creaturesIdsInRange = await _mySql.GetManyAsync<CreatureTemplate>(c => c.Entry >= IdRangeFrom && c.Entry < IdRangeTo);
            if (creaturesIdsInRange.Count() > 0)
            {
                var maxId = creaturesIdsInRange.Max(c => c.Entry) + 1;
                id = (int)(Math.Ceiling(maxId / IdSize) * IdSize);
            }
            return id;
        }
    }
}
