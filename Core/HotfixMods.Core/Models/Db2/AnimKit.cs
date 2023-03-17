using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKit
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public uint OneShotDuration { get; set; } = 0;
        [Db2Description("The next AnimKit to play after current AnimKit has been played as one shot.")]
        public ushort OneShotStopAnimKitID { get; set; } = 0;
        public ushort LowDefAnimKitID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
