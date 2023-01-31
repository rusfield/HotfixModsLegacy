﻿namespace HotfixMods.Core.Flags.Db2
{
    [Flags]
    public enum SpellMiscAttributes11 : long
    {
        DEFAULT = 0,
        LOCK_CASTER_MOVEMENT_AND_FACING_WHILE_CASTING = 1,
        DO_NOT_CANCEL_WHEN_ALL_EFFECTS_ARE_DISABLED = 2,
        SCALES_WITH_CASTING_ITEMS_LEVEL = 4,
        DO_NOT_LOG_ON_LEARN = 8,
        HIDE_SHAPESHIFT_REQUIREMENTS = 16,
        ABSORB_FALLING_DAMAGE = 32,
        UNBREAKABLE_CHANNEL = 64,
        IGNORE_CASTERS_SPELL_LEVEL = 128,
        TRANSFER_MOUNT_SPELL = 256,
        IGNORE_SPELLCAST_OVERRIDE_SHAPESHIFT_REQUIREMENTS = 512,
        NEWEST_EXCLUSIVE_COMPLETE = 1024,
        NOT_IN_INSTANCES = 2048,
        OBSOLETE = 4096,
        IGNORE_PVP_POWER = 8192,
        CAN_ASSIST_UNINTERACTIBLE = 16384,
        CAST_WHEN_INITIAL_LOGGING_IN = 32768,
        NOT_IN_MYTHIC_PLUS_MODE = 65536,
        CHEAPER_NPC_KNOCKBACK = 131072,
        IGNORE_CASTER_ABSORB_MODIFIERS = 262144,
        IGNORE_TARGET_ABSORB_MODIFIERS = 524288,
        HIDE_LOSS_OF_CONTROL_UI = 1048576,
        ALLOW_HARMFUL_ON_FRIENDLY = 2097152,
        CHEAP_MISSILE_AOI = 4194304,
        EXPENSIVE_MISSILE_AOI = 8388608,
        NO_CLIENT_FAIL_ON_NO_PET = 16777216,
        AI_ATTEMPT_CAST_ON_IMMUNE_PLAYER = 33554432,
        ALLOW_WHILE_STUNNED_BY_STUN_MECHANIC = 67108864,
        DO_NOT_CLOSE_LOOT_WINDOW = 134217728,
        HIDE_DAMAGE_ABSORB_UI = 268435456,
        DO_NOT_TREAT_AS_AREA_EFFECT = 536870912,
        CHECK_REQUIRED_TARGET_AURA_BY_CASTER = 1073741824,
        APPLY_ZONE_AURA_SPELL_TO_PETS = 2147483648,
    }
}