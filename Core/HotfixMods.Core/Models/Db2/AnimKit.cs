using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKit
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public uint OneShotDuration { get; set; } = 0;
        public ushort OneShotStopAnimKitID { get; set; } = 0;
        public ushort LowDefAnimKitID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
