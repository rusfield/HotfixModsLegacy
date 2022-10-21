using HotfixMods.Core.Attributes;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Interfaces;

namespace HotfixMods.Core.Models.TrinityCore
{
    [HotfixesSchema]
    public class HotfixData
    {
        public int Id { get; set; }
        public long UniqueId { get; set; }
        public TableHashes TableHash { get; set; }
        public int RecordId { get; set; }
        public HotfixStatuses Status { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
