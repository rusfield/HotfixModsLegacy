namespace HotfixMods.Core.Flags.TrinityCore
{
    [Flags]
    public enum CreatureTemplateTypeFlags2 : uint
    {
        NONE = 0,
        PREDICTIVE_POWER_REGEN = 0x00000001,
        HIDE_LEVEL_INFO_IN_TOOLTIP = 0x00000002,
        HIDE_HEALTH_BAR_UNDER_TOOLTIP = 0x00000004,
        NEVER_DISPLAY_EMOTE_OR_CHAT_TEXT_IN_A_CHAT_BUBBLE = 0x00000008,
        NO_DEATH_THUD = 0x00000010,
        NO_INTERACT_ON_LEFT_CLICK = 0x00000020,
        UNK_7 = 0x00000040,
        UNK_8 = 0x00000080
    }
}
