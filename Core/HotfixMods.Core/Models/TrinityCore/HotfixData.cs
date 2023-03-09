using HotfixMods.Core.Attributes;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Interfaces;

namespace HotfixMods.Core.Models.TrinityCore
{
    [HotfixesSchema]
    public class HotfixData
    {
        public int ID { get; set; }
        public uint UniqueID { get; set; }
        public uint TableHash { get; set; }
        public int RecordID { get; set; }
        public byte Status { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
