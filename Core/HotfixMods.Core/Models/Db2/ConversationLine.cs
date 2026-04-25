using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ConversationLine
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public int BroadcastTextID { get; set; } = 0;
        public int Unused1020 { get; set; } = 0;
        public int SpellVisualKitID { get; set; } = 0;
        public int AdditionalDuration { get; set; } = 0;
        public ushort NextConversationLineID { get; set; } = 0;
        public ushort AnimKitID { get; set; } = 0;
        public byte SpeechType { get; set; } = 0;
        public byte StartAnimation { get; set; } = 0;
        public byte EndAnimation { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
