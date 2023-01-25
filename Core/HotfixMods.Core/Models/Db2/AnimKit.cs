using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKit
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public uint OneShotDuration { get; set; } = 0;
        public ushort OneShotStopAnimKitId { get; set; } = 0;
        public ushort LowDefAnimKitId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
