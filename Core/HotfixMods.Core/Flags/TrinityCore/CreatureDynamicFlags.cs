namespace HotfixMods.Core.Flags.TrinityCore
{
    [Flags]
    public enum CreatureDynamicFlags : uint
    {
        NONE = 0x0000,
        HIDE_MODEL = 0x0002,
        LOOTABLE = 0x0004,
        TRACK_UNIT = 0x0008,
        TAPPED = 0x0010,
        SPECIALINFO = 0x0020,
        CAN_SKIN = 0x0040,
        REFER_A_FRIEND = 0x0080
    }
}
