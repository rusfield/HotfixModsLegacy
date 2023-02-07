using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CameraEffect
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public byte Flags { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
