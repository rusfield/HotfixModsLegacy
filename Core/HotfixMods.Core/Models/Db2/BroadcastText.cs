using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class BroadcastText
    {
        public string Text { get; set; } = "";
        public string Text1 { get; set; } = "";
        [IndexField]
        public int ID { get; set; } = 0;
        public int LanguageID { get; set; } = 0;
        public int ConditionID { get; set; } = 0;
        public ushort EmotesID { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public int ChatBubbleDurationMs { get; set; } = 0;
        public int VoiceOverPriorityID { get; set; } = 0;
        public int SoundKitID1 { get; set; } = 0;
        public int SoundKitID2 { get; set; } = 0;
        public ushort EmoteID1 { get; set; } = 0;
        public ushort EmoteID2 { get; set; } = 0;
        public ushort EmoteID3 { get; set; } = 0;
        public ushort EmoteDelay1 { get; set; } = 0;
        public ushort EmoteDelay2 { get; set; } = 0;
        public ushort EmoteDelay3 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
