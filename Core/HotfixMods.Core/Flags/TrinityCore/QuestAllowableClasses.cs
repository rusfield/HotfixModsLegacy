namespace HotfixMods.Core.Flags.TrinityCore
{
    [Flags]
    public enum QuestAllowableClasses : uint
    {
        NONE = 0x00000000,
        WARRIOR = 0x00000001,
        PALADIN = 0x00000002,
        HUNTER = 0x00000004,
        ROGUE = 0x00000008,
        PRIEST = 0x00000010,
        DEATH_KNIGHT = 0x00000020,
        SHAMAN = 0x00000040,
        MAGE = 0x00000080,
        WARLOCK = 0x00000100,
        MONK = 0x00000200,
        DRUID = 0x00000400,
        DEMON_HUNTER = 0x00000800,
        EVOKER = 0x00001000
    }
}
