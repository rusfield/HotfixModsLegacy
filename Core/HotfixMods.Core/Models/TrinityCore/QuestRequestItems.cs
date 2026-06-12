using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class QuestRequestItems
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public ushort EmoteOnComplete { get; set; } = 0;
        public ushort EmoteOnIncomplete { get; set; } = 0;
        public uint EmoteOnCompleteDelay { get; set; } = 0;
        public uint EmoteOnIncompleteDelay { get; set; } = 0;
        public string CompletionText { get; set; } = "";
        public int VerifiedBuild { get; set; } = -1;
    }
}
