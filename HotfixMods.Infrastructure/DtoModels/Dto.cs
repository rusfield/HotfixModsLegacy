using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels
{
    public abstract class Dto
    {
        public int Id { get; set; }
        public bool IsUpdate { get; set; }
        public string SearchResultName { get;set; } // For example "Heroic" for items, or female name for creatures.

        public string HotfixModsName { get; set; }
        public string HotfixModsComment { get; set; }


        // Used by code to save/update
        List<HotfixData> hotfixes;
        int initHotfixId;
        int verifiedBuild;
        public void InitHotfixes(int initHotfixId, int verifiedBuild)
        {
            hotfixes = new List<HotfixData>();
            this.initHotfixId = initHotfixId;
            this.verifiedBuild = verifiedBuild;
        }
        public void AddHotfix(int recordId, TableHashes tableHash, HotfixStatuses status)
        {
            if (hotfixes == null)
                throw new Exception("Call InitHotfixes before adding any.");

            hotfixes.Add(new HotfixData()
            {
                Id = hotfixes.Count > 0 ? hotfixes.Max(c => c.Id) + 1 : initHotfixId,
                RecordId = recordId,
                UniqueId = Id,
                TableHash = (long)tableHash,
                Status = status,
                VerifiedBuild = verifiedBuild
            });
        }
        public List<HotfixData> GetHotfixes()
        {
            if (hotfixes == null)
                throw new Exception("Call InitHotfixes before adding any.");

            return hotfixes;
        }
    }
}
