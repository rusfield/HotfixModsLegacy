using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CameraEffect
    {
        public int Id { get; set; } = 1;
        public byte Flags { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
