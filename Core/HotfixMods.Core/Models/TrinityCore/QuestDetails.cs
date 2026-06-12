using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class QuestDetails
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public ushort Emote1 { get; set; } = 0;
        public ushort Emote2 { get; set; } = 0;
        public ushort Emote3 { get; set; } = 0;
        public ushort Emote4 { get; set; } = 0;
        public uint EmoteDelay1 { get; set; } = 0;
        public uint EmoteDelay2 { get; set; } = 0;
        public uint EmoteDelay3 { get; set; } = 0;
        public uint EmoteDelay4 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
