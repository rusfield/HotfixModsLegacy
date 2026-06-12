namespace HotfixMods.Core.Flags.TrinityCore
{
    [Flags]
    public enum QuestObjectiveFlags : uint
    {
        NONE = 0x0000,
        TRACKED_ON_MINIMAP = 0x0001,
        SEQUENCED = 0x0002,
        OPTIONAL = 0x0004,
        HIDDEN = 0x0008,
        HIDE_CREDIT_MSG = 0x0010,
        PRESERVE_QUEST_ITEMS = 0x0020,
        PART_OF_PROGRESS_BAR = 0x0040,
        KILL_PLAYERS_SAME_FACTION = 0x0080,
        NO_SHARE_PROGRESS = 0x0100,
        IGNORE_SOULBOUND_ITEMS = 0x0200
    }
}
