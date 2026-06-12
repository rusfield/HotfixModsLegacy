namespace HotfixMods.Core.Flags.TrinityCore
{
    [Flags]
    public enum QuestSpecialFlags : byte
    {
        NONE = 0x00,
        REPEATABLE = 0x01,
        AUTO_PUSH_TO_PARTY = 0x02,
        AUTO_ACCEPT = 0x04,
        DF_QUEST = 0x08,
        MONTHLY = 0x10
    }
}
