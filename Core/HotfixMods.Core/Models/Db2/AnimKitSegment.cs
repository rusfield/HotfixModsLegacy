using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKitSegment
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        [ParentIndexField]
        public ushort ParentAnimKitID { get; set; } = 0;
        public byte OrderIndex { get; set; } = 0;
        public ushort AnimID { get; set; } = 0;
        public uint AnimStartTime { get; set; } = 0;
        public ushort AnimKitConfigID { get; set; } = 1;
        public byte StartCondition { get; set; } = 0;
        public byte StartConditionParam { get; set; } = 0;
        public uint StartConditionDelay { get; set; } = 0;
        public byte EndCondition { get; set; } = 0;
        public uint EndConditionParam { get; set; } = 0;
        public uint EndConditionDelay { get; set; } = 0;
        public decimal Speed { get; set; } = 1;
        public ushort SegmentFlags { get; set; } = 0;
        public byte ForcedVariation { get; set; } = 0;
        public int OverrideConfigFlags { get; set; } = 0;
        public sbyte LoopToSegmentIndex { get; set; } = 0;
        public ushort BlendInTimeMs { get; set; } = 0;
        public ushort BlendOutTimeMs { get; set; } = 0;
        public decimal Field_9_0_1_34278_018 { get; set; } = 1;
        public int VerifiedBuild { get; set; } = -1;
    }
}
