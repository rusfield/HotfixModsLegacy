﻿namespace HotfixMods.Core.Flags.Db2
{
    [Flags]
    public enum SpellMiscAttributes10 : long
    {
        DEFAULT = 0,
        BYPASS_VISIBILITY_CHECK = 1,
        IGNORE_POSITIVE_DAMAGE_TAKEN_MODIFIERS = 2,
        USES_RANGED_SLOT_COSMETIC = 4,
        DO_NOT_LOG_FULL_OVERHEAL = 8,
        NPC_KNOCKBACK_IGNORE_DOORS = 16,
        FORCE_NON_BINARY_RESISTANCE = 32,
        NO_SUMMON_LOG = 64,
        IGNORE_INSTANCE_LOCK_AND_FARM_LIMIT_ON_TELEPORT = 128,
        AREA_EFFECTS_USE_TARGET_RADIUS = 256,
        CHARGE_JUMPCHARGE_USE_ABSOLUTE_SPEED = 512,
        PROC_COOLDOWN_ON_A_PER_TARGET_BASIS = 1024,
        LOCK_CHEST_AT_PRECAST = 2048,
        USE_SPELL_BASE_LEVEL_FOR_SCALING = 4096,
        RESET_COOLDOWN_UPON_ENDING_AN_ENCOUNTER = 8192,
        ROLLING_PERIODIC = 16384,
        SPELLBOOK_HIDDEN_UNTIL_OVERRIDDEN = 32768,
        DEFEND_AGAINST_FRIENDLY_CAST = 65536,
        ALLOW_DEFENSE_WHILE_CASTING = 131072,
        ALLOW_DEFENSE_WHILE_CHANNELING = 262144,
        ALLOW_FATAL_DUEL_DAMAGE = 524288,
        MULTI_CLICK_GROUND_TARGETING = 1048576,
        AOE_CAN_HIT_SUMMONED_INVIS = 2097152,
        ALLOW_WHILE_STUNNED_BY_HORROR_MECHANIC = 4194304,
        VISIBLE_ONLY_TO_CASTER_CONVERSATIONS_ONLY = 8388608,
        UPDATE_PASSIVES_ON_APPLY_OR_REMOVE = 16777216,
        NORMAL_MELEE_ATTACK = 33554432,
        IGNORE_FEIGN_DEATH = 67108864,
        CASTER_DEATH_CANCELS_PERSISTENT_AREA_AURAS = 134217728,
        DO_NOT_LOG_ABSORB = 268435456,
        THIS_MOUNT_IS_NOT_AT_THE_ACCOUNT_LEVEL = 536870912,
        PREVENT_CLIENT_CAST_CANCEL = 1073741824,
        ENFORCE_FACING_ON_PRIMARY_TARGET_ONLY = 2147483648,
    }
}
