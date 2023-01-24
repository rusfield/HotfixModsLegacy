namespace HotfixMods.Infrastructure.InfoModels
{
    public class AnimKitSegmentInfo : IInfoModel
    {
        public string OrderIndex = "Order Index works as an ID for a group of segments linked to an AnimKit.\nThe order does not always matter, but can do based on the condition values.\nTo edit this value, move the segment using the controllers in the footer.";
        public string AnimId = "The ID of animation to play for this segment.\nNote that the animation will only work if the creature that plays it supports this animation, and will play nothing or a fallback animation if not.\nUse a tool like WoW Model Viewer to see what animations are available to a creature or a player character.";
        public string AnimStartTime = "The frame to start play the animation from.\nUse a tool like WoW Model Viewer to play an animation and find the specific frame you want to start play from, if not from the beginning (which is 0).";
        public string AnimKitConfigId = "Maps to the AnimKitConfig record to use for this segment. AnimKitConfig and it's referencing tables holds values such as which bodyparts will play the given animation and with what priority.";
        public string StartCondition = "TODO";
        public string StartConditionParam = "TODO";
        public string StartConditionDelay = "TODO";
        public string EndCondition = "TODO";
        public string EndConditionParam = "TODO";
        public string EndConditionDelay = "TODO";
        public string Speed = "The speed at which the animation will play is multiplied by this value.\n\n1 is regular speed.\n2 is double speed.\n0.5 is half speed.\n0 is for standing still.\n-1 is for playing the animation backwards.";
        public string SegmentFlags = "TODO";
        public string ForcedVariation = "Some animations have multiple variations, for example dance animations or attack animations. At 0, a random variation will be played. Anything higher will force it to a specific variation.\nUse a tool like WoW Model Viewer to see available variations for an animation.\nNote that this may vary by creatures or player characters.";
        public string OverrideConfigFlags = "TODO";
        public string LoopToSegmentIndex = "TODO";
        public string BlendInTimeMs = "TODO";
        public string BlendOutTimeMs = "TODO";
        public string Field_9_0_1_34278_018 = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}
