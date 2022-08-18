using HotfixMods.Core.Enums;
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
                id = hotfixIdsInRange.Max(c => c.Id) + 1;
            }
            if (id < IdRangeTo)
                return id;
            else
                throw new ArgumentOutOfRangeException("Database is full.");
        }

        protected async Task AddHotfixes(List<HotfixData> newHotfixData)
        {
            if(newHotfixData.Count > 0)
            {
                var id = newHotfixData.First().UniqueId;
                var existingHotfixData = await _mySql.GetManyAsync<HotfixData>(h => h.Status == HotfixStatuses.VALID && h.UniqueId == id && h.VerifiedBuild == VerifiedBuild);
                if (existingHotfixData != null && existingHotfixData.Count() > 0)
                {
                    foreach (var hotfix in existingHotfixData)
                    {
                        hotfix.Status = HotfixStatuses.INVALID;
                    }
                    await _mySql.UpdateManyAsync(existingHotfixData);
                }
                await _mySql.AddManyAsync(newHotfixData);
            }
        }

        public async Task<int> GetNextIdAsync(bool quickScan = true)
        {
            int id = IdRangeFrom;
            var entitiesInRange = await _mySql.GetManyAsync<HotfixData>(c => c.RecordId >= IdRangeFrom && c.RecordId < IdRangeTo);
            if (entitiesInRange.Count() > 0)
            {
                var nextEmpty = entitiesInRange.Max(c => c.RecordId) + 1;
                id = (int)(Math.Ceiling(nextEmpty / IdSize) * IdSize);
            }
            if (id < IdRangeTo)
                return id;
            else
                throw new ArgumentOutOfRangeException("Database is full.");
        }

        protected void ConsoleProgressCallback(string stepTitle, string stepSubTitle, int progress)
        {
            Console.WriteLine($"{progress} %: {stepTitle} => {stepSubTitle}");
        }
    }
}
