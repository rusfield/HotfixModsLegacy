using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKit
    {
        public int? Id { get; set; } = 0;
        public int? OneShotDuration { get; set; } = 0;
        public int? OneShotStopAnimKitId { get; set; } = 0;
        public int? LowDefAnimKitId { get; set; } = 0;
        public int? VerifiedBuild { get; set; } = -1;
    }
}
