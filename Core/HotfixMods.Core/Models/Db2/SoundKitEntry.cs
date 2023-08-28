using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SoundKitEntry
    {
        
        public int ID { get; set; }
        
        public uint SoundKitID { get; set; }
        public int FileDataID { get; set; }
        public byte Frequency { get; set; }
        public decimal Volume { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
