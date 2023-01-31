﻿namespace HotfixMods.Core.Flags
{
    [Flags]
    public enum SpellMiscAttributes12 : long
    {
        DEFAULT = 0,
        ENABLE_PROCS_FROM_SUPPRESSED_CASTER_PROCS = 1,
        CAN_PROC_FROM_SUPPRESSED_CASTER_PROCS = 2,
        SHOW_COOLDOWN_AS_CHARGE_UP = 4,
        NO_PVP_BATTLE_FATIGUE = 8,
        TREAT_SELF_CAST_AS_REFLECT = 16,
        DO_NOT_CANCEL_AREA_AURA_ON_SPEC_SWITCH = 32,
        COOLDOWN_ON_AURA_CANCEL_UNTIL_COMBAT_ENDS = 64,
        DO_NOT_RE_APPLY_AREA_AURA_IF_IT_PERSISTS_THROUGH_UPDATE = 128,
        DISPLAY_TOAST_MESSAGE = 256,
        ACTIVE_PASSIVE = 512,
        IGNORE_DAMAGE_CANCELS_AURA_INTERRUPT = 1024,
        FACE_DESTINATION = 2048,
        IMMUNITY_PURGES_SPELL = 4096,
        DO_NOT_LOG_SPELL_MISS = 8192,
        IGNORE_DISTANCE_CHECK_ON_CHARGE_OR_JUMP_CHARGE_DONE_TRIGGER_SPELL = 16384,
        DISABLE_KNOWN_SPELLS_WHILE_CHARMED = 32768,
        IGNORE_DAMAGE_ABSORB = 65536,
        NOT_IN_PROVING_GROUNDS = 131072,
        OVERRIDE_DEFAULT_SPELLCLICK_RANGE = 262144,
        IS_IN_GAME_STORE_EFFECT = 524288,
        ALLOW_DURING_SPELL_OVERRIDE = 1048576,
        USE_FLOAT_VALUES_FOR_SCALING_AMOUNTS = 2097152,
        SUPPRESS_TOASTS_ON_ITEM_PUSH = 4194304,
        TRIGGER_COOLDOWN_ON_SPELL_START = 8388608,
        NEVER_LEARN = 16777216,
        NO_DEFLECT = 33554432,
        DEPRECATED_USE_START_OF_CAST_LOCATION_FOR_SPELL_DEST = 67108864,
        RECOMPUTE_AURA_ON_MERCENARY_MODE = 134217728,
        USE_WEIGHTED_RANDOM_FOR_FLEX_MAX_TARGETS = 268435456,
        IGNORE_RESILIENCE = 536870912,
        APPLY_RESILIENCE_TO_SELF_DAMAGE = 1073741824,
        ONLY_PROC_FROM_CLASS_ABILITIES = 2147483648,
    }
}
