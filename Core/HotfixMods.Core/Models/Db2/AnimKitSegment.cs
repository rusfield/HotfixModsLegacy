using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKitSegment
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        [ParentIndexField]
        public uint ParentAnimKitID { get; set; } = 0;
        [Db2Description("Order Index works as an ID for a group of segments linked to an AnimKit.$The order does not always matter, but can do based on the condition values.$To edit this value, move the segment using the controllers in the footer.")]
        public int OrderIndex { get; set; } = 0;
        [Db2Description("The ID of animation to play for this segment.$Note that the animation will only work if the creature that plays it supports this animation, and will play nothing or a fallback animation if not.$Use a tool like WoW Model Viewer to see what animations are available to a creature or a player character.")]
        public int AnimID { get; set; } = 0;
        [Db2Description("The frame inside AnimID to start playback from.$0 means the animation starts from its beginning.$This is important together with Speed and the end conditions, because many AnimKits are built from slices of the same base animation rather than always replaying it from frame 0.")]
        public int AnimStartTime { get; set; } = 0;
        [Db2Description("Maps to the AnimKitConfig record to use for this segment. AnimKitConfig and it's referencing tables holds values such as which bodyparts will play the given animation and with what priority.")]
        public uint AnimKitConfigID { get; set; } = 1;
        [Db2Description("Controls when this segment starts.$Condition 0: Start from the AnimKit root timeline. StartConditionDelay adds a delay in milliseconds.$Condition 1: Start when the segment referenced by StartConditionParam starts. StartConditionDelay adds a delay in milliseconds.$Condition 2: Start when the segment referenced by StartConditionParam ends. StartConditionDelay adds a delay in milliseconds.$StartConditionParam refers to another segment's OrderIndex.")]
        public int StartCondition { get; set; } = 0;
        [Db2Description("Reference OrderIndex used by StartCondition 1 and 2.$Condition 0 normally leaves this at 0.$Condition 1: Start relative to the referenced segment's start.$Condition 2: Start relative to the referenced segment's end.")]
        public int StartConditionParam { get; set; } = 0;
        [Db2Description("Additional delay in milliseconds used by the start condition.$Condition 0: Delay from the AnimKit root timeline.$Condition 1: Delay from the referenced segment's start.$Condition 2: Delay from the referenced segment's end.")]
        public int StartConditionDelay { get; set; } = 0;
        [Db2Description("Controls when this segment stops or how its playback window is bounded.$Condition 0: No clearly identified automatic stop behavior. Often used for persistent / held segments or chain glue.$Condition 1: Play until the animation naturally ends.$Condition 2: Timed stop variant. EndConditionDelay behaves like milliseconds, but this still differs from Condition 3 in subtle ways.$Condition 3: Stop the segment after EndConditionDelay milliseconds.$Condition 4: Timed slice / tail-window variant. Often behaves like a bounded end-slice of the animation, especially with non-zero AnimStartTime or looping.$Condition 5: Rare variant related to Condition 4.")]
        public int EndCondition { get; set; } = 0;
        [Db2Description("Extra parameter used by some end conditions.$Condition 0: Occasionally used as chain metadata, but still not fully decoded.$Condition 1: Usually 0.$Condition 2: Usually 0.$Condition 3: Usually 0 / not used.$Condition 4: Commonly 0 or the segment's own OrderIndex.$Condition 5: Similar to Condition 4, but rarer.")]
        public int EndConditionParam { get; set; } = 0;
        [Db2Description("Time value used by the end condition.$This behaves like milliseconds in observed test cases.$Condition 2: Timed stop variant.$Condition 3: Number of milliseconds before the segment stops.$Condition 4: Length of the bounded slice / tail-window in milliseconds.$Condition 5: Similar to Condition 4, but still not fully decoded.")]
        public int EndConditionDelay { get; set; } = 0;
        [Db2Description("The speed at which the animation will play is multiplied by this value.$$1 is regular speed.$2 is double speed.$0.5 is half speed.$0 is for standing still.$-1 is for playing the animation backwards.")]
        public float Speed { get; set; } = 1;
        [Db2Description("Bit flags that modify how the segment plays.$Known bits:$2: Enables ForcedVariation. With this flag enabled, ForcedVariation appears to select a specific variation index (0 = first variation, 1 = second variation, etc.).$256: Enables BlendInTimeMs.$512: Enables BlendOutTimeMs.$Most other bits are still not fully decoded.")]
        public int SegmentFlags { get; set; } = 0;
        [Db2Description("Selects a concrete animation variation when SegmentFlags enables variation control.$Observed behavior: with the variation flag enabled, 0 appears to mean the first variation, 1 the second variation, and so on.$Without the variation flag, this value does not appear to matter.$Use a tool like WoW Model Viewer to inspect which variations exist for a specific model and animation.")]
        public int ForcedVariation { get; set; } = 0;
        [Db2Description("Overrides / supplements flags from the referenced AnimKitConfig record.$This field is used in real client data, but the exact meaning of individual bits is still not fully decoded.$If possible, prefer changing AnimKitConfigID first and only use this field when reproducing data from an existing client record.")]
        public int OverrideConfigFlags { get; set; } = 0;
        [Db2Description("OrderIndex to jump back to when the AnimKit is played as a loop.$A non-negative value commonly points at another segment in the same AnimKit.$-1 commonly means there is no explicit loop target on the segment.")]
        public int LoopToSegmentIndex { get; set; } = 0;
        [Db2Description("Blend-in time in milliseconds.$This only appears to matter when SegmentFlags has ENABLE_BLEND_IN (256) set.")]
        public int BlendInTimeMs { get; set; } = 0;
        [Db2Description("Blend-out time in milliseconds.$This only appears to matter when SegmentFlags has ENABLE_BLEND_OUT (512) set.")]
        public int BlendOutTimeMs { get; set; } = 0;
        [Db2Description("Unknown field introduced in 9.0.1.34278.$Observed default is 1.$No reliable behavior has been decoded yet.")]
        public int Field_9_0_1_34278_018 { get; set; } = 1;
        public int VerifiedBuild { get; set; } = -1;
    }
}
