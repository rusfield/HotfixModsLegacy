using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SoundKitEntry
    {
        [IndexField]
        public int ID { get; set; }
        [ParentIndexField]
        public uint SoundKitID { get; set; }
        public int FileDataID { get; set; }
        public byte Frequency { get; set; }
        public decimal Volume { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
