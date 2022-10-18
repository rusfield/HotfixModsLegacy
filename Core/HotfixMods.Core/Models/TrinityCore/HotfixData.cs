using HotfixMods.Core.Enums;
using HotfixMods.Core.Interfaces;

namespace HotfixMods.Core.Models.TrinityCore
{
    public class HotfixData : IHotfixesSchema
    {
        public int Id { get; set; }
        public long UniqueId { get; set; }
        public TableHashes TableHash { get; set; }
        public int RecordId { get; set; }
        public HotfixStatuses Status { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
