namespace HotfixMods.Core.Flags
{
    [Flags]
    public enum CreatureDisplayInfoFlags : byte
    {
        DEFAULT = 0,
        NO_SHADOW_BLOB = 1,
        PERMANENT_VISUAL_KIT_PERSISTS_WHEN_DEAD = 2,
        DO_NOT_CHANGE_MOVE_ANIMS_BASED_ON_SCALE = 4,
        OVERRIDE_COMBAT_REACH = 8,
        OVERRIDE_MELEE_RANGE = 16,
        NO_FUZZY_HIT = 32,
        UNK_64 = 64,
        UNK_128 = 128
    }
}
