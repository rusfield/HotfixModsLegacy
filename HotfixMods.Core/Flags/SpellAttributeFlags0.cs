﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Flags
{
    [Flags]
    public enum SpellAttributeFlags0 : long
    {
        NONE = 0,
        PROC_FAILURE_BURNS_CHARGE = 1,
        USES_RANGED_SLOT = 2,
        ON_NEXT_SWING_NO_DAMAGE = 4,
        DO_NOT_LOG_IMMUNE_MISSES = 8,
        IS_ABILITY = 16,
        IS_TRADESKILL = 32,
        PASSIVE = 64,
        DO_NOT_DISPLAY_SPELLBOOK = 128,
        DO_NOT_LOG = 256,
        HELD_ITEM_ONLY = 512,
        ON_NEXT_SWING = 1024,
        WEARER_CASTS_PROC_TRIGGER = 2048,
        SERVER_ONLY = 4096,
        ALLOW_ITEM_SPELL_IN_PVP = 8192,
        ONLY_INDOORS = 16384,
        ONLY_OUTDOORS = 32768,
        NOT_SHAPESHIFTED = 65536,
        ONLY_STEALTHED = 131072,
        DO_NOT_SHEATH = 262144,
        SCALES_W_CREATURE_LEVEL = 524288,
        CANCELS_AUTO_ATTACK_COMBAT = 1048576,
        NO_ACTIVE_DEFENSE = 2097152,
        TRACK_TARGET_IN_CAST_PLAYER_ONLY = 4194304,
        ALLOW_CAST_WHILE_DEAD = 8388608,
        ALLOW_WHILE_MOUNTED = 16777216,
        COOLDOWN_ON_EVENT = 33554432,
        AURA_IS_DEBUFF = 67108864,
        ALLOW_WHILE_SITTING = 134217728,
        NOT_IN_COMBAT_ONLY_PEACEFUL = 268435456,
        NO_IMMUNITIES = 536870912,
        HEARTBEAT_RESIST = 1073741824,
        NO_AURA_CANCEL = 2147483648,
    }
}
