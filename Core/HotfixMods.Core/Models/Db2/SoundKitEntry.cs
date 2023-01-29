using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SoundKitEntry
    {
        [IndexField]
        public uint Id { get; set; }
        [ParentIndexField]
        public uint SoundKitId { get; set; }
        public int FileDataId { get; set; }
        public byte Frequency { get; set; }
        public decimal Volume { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
