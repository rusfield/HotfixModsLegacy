using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class QuestV2
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public int UniqueBitFlag { get; set; } = 0;
        public int UiQuestDetailsTheme { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
