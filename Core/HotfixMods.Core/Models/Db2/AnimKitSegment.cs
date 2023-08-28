using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKitSegment
    {
        
        public int ID { get; set; } = 0;
        
        public ushort ParentAnimKitID { get; set; } = 0;
        [Db2Description("Order Index works as an ID for a group of segments linked to an AnimKit.$The order does not always matter, but can do based on the condition values.$To edit this value, move the segment using the controllers in the footer.")]
        public byte OrderIndex { get; set; } = 0;
        [Db2Description("The ID of animation to play for this segment.$Note that the animation will only work if the creature that plays it supports this animation, and will play nothing or a fallback animation if not.$Use a tool like WoW Model Viewer to see what animations are available to a creature or a player character.")]
        public ushort AnimID { get; set; } = 0;
        [Db2Description("The frame to start play the animation from.$Use a tool like WoW Model Viewer to play an animation and find the specific frame you want to start play from, if not from the beginning (which is 0).")]
        public uint AnimStartTime { get; set; } = 0;
        [Db2Description("Maps to the AnimKitConfig record to use for this segment. AnimKitConfig and it's referencing tables holds values such as which bodyparts will play the given animation and with what priority.")]
        public ushort AnimKitConfigID { get; set; } = 1;
        [Db2Description("Condition 0: ??$Condition 1: ??$ Condition 2: Used to chain segments. The very first segment in the cina should have Start Condition 0. Then start chaining the following segments using Condition Param as index.")]
        public byte StartCondition { get; set; } = 0;
        [Db2Description("Condition 0: ??$Condition 1: ??$Condition 2: Index of segment in the chain.")]
        public byte StartConditionParam { get; set; } = 0;
        public uint StartConditionDelay { get; set; } = 0;
        public byte EndCondition { get; set; } = 0;
        [Db2Description("Condition 0: ??$Condition 1:??$Condition 2: ??$Condition 3: Not used.$Condition 4: ??$Condition 5: ??")]
        public uint EndConditionParam { get; set; } = 0;
        [Db2Description("Condition 0: ??$Condition 1: ??$Condition 2: ??$Condition 3: Amount of time?/frames? the segment lasts. This is including Blend In and excluding Blend Out.$Condition 4: ??$Condition 5: ??")]
        public uint EndConditionDelay { get; set; } = 0;
        [Db2Description("The speed at which the animation will play is multiplied by this value.$$1 is regular speed.$2 is double speed.$0.5 is half speed.$0 is for standing still.$-1 is for playing the animation backwards.")]
        public decimal Speed { get; set; } = 1;
        public ushort SegmentFlags { get; set; } = 0;
        [Db2Description("Some animations have multiple variations, for example dance animations or attack animations. At 0, a random variation will be played. Anything higher will force it to a specific variation.$Use a tool like WoW Model Viewer to see available variations for an animation.$Note that this may vary by creatures or player characters.$$For this value to work, enable it in the flags.")]
        public byte ForcedVariation { get; set; } = 0;
        public int OverrideConfigFlags { get; set; } = 0;
        public sbyte LoopToSegmentIndex { get; set; } = 0;
        public ushort BlendInTimeMs { get; set; } = 0;
        public ushort BlendOutTimeMs { get; set; } = 0;
        public decimal Field_9_0_1_34278_018 { get; set; } = 1;
        public int VerifiedBuild { get; set; } = -1;
    }
}
