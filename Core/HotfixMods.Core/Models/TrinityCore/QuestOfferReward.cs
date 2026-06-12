using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class QuestOfferReward
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public short Emote1 { get; set; } = 0;
        public short Emote2 { get; set; } = 0;
        public short Emote3 { get; set; } = 0;
        public short Emote4 { get; set; } = 0;
        public uint EmoteDelay1 { get; set; } = 0;
        public uint EmoteDelay2 { get; set; } = 0;
        public uint EmoteDelay3 { get; set; } = 0;
        public uint EmoteDelay4 { get; set; } = 0;
        public string RewardText { get; set; } = "";
        public int VerifiedBuild { get; set; } = -1;
    }
}
