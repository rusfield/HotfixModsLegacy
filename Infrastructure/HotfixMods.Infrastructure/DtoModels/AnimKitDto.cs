using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class AnimKitDto : DtoBase
    {
        public AnimKitDto() : base(nameof(AnimKit)) { }

        public AnimKit AnimKit { get; set; } = new();
        public List<SegmentGroup> SegmentGroups { get; set; } = new();

        public class SegmentGroup
        {
            public AnimKitSegment AnimKitSegment { get; set; } = new();
            public AnimKitConfig AnimKitConfig { get; set; } = new();
            public List<AnimKitConfigBoneSet> AnimKitConfigBoneSet { get; set; } = new();
        }
    }
}
