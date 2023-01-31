﻿namespace HotfixMods.Core.Flags
{
    [Flags]
    public enum SpellMiscAttributes2 : long
    {
        DEFAULT = 0,
        ALLOW_DEAD_TARGET = 1,
        NO_SHAPESHIFT_UI = 2,
        IGNORE_LINE_OF_SIGHT = 4,
        ALLOW_LOW_LEVEL_BUFF = 8,
        USE_SHAPESHIFT_BAR = 16,
        AUTO_REPEAT = 32,
        CANNOT_CAST_ON_TAPPED = 64,
        DO_NOT_REPORT_SPELL_FAILURE = 128,
        INCLUDE_IN_ADVANCED_COMBAT_LOG = 256,
        ALWAYS_CAST_AS_UNIT = 512,
        SPECIAL_TAMING_FLAG = 1024,
        NO_TARGET_PER_SECOND_COSTS = 2048,
        CHAIN_FROM_CASTER = 4096,
        ENCHANT_OWN_ITEM_ONLY = 8192,
        ALLOW_WHILE_INVISIBLE = 16384,
        DO_NOT_CONSUME_IF_GAINED_DURING_CAST = 32768,
        NO_ACTIVE_PETS = 65536,
        DO_NOT_RESET_COMBAT_TIMERS = 131072,
        NO_JUMP_WHILE_CAST_PENDING = 262144,
        ALLOW_WHILE_NOT_SHAPESHIFTED_CASTER_FORM = 524288,
        INITIATE_COMBAT_POST_CAST_ENABLES_AUTO_ATTACK = 1048576,
        FAIL_ON_ALL_TARGETS_IMMUNE = 2097152,
        NO_INITIAL_THREAT = 4194304,
        PROC_COOLDOWN_ON_FAILURE = 8388608,
        ITEM_CAST_WITH_OWNER_SKILL = 16777216,
        DO_NOT_BLOCK_MANA_REGEN = 33554432,
        NO_SCHOOL_IMMUNITIES = 67108864,
        IGNORE_WEAPONSKILL = 134217728,
        NOT_AN_ACTION = 268435456,
        CAN_NOT_CRIT = 536870912,
        ACTIVE_THREAT = 1073741824,
        RETAIN_ITEM_CAST = 2147483648,
    }

}
