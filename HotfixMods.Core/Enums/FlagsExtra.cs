using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Enums
{
    [Flags]
    public enum FlagsExtra : long
    {
        NONE = 0,
        INSTANCE_BIND = 1,                          // creature kill bind instance with killer and killer's group
        CIVILIAN = 2,                               // not aggro (ignore faction/reputation hostility)
        NO_PARRY = 4,                               // creature can't parry
        NO_PARRY_HASTEN = 8,                        // creature can't counter-attack at parry
        NO_BLOCK = 16,                              // creature can't block
        NO_CRUSHING_BLOWS = 32,                     // creature can't do crush attacks
        NO_XP = 64,                                 // creature kill does not provide XP
        TRIGGER = 128,                              // trigger creature
        NO_TAUNT = 256,                             // creature is immune to taunt auras and 'attack me' effects
        NO_MOVE_FLAGS_UPDATE = 512,                 // creature won't update movement flags
        GHOST_VISIBILITY = 1024,                    // creature will only be visible to dead players
        USE_OFFHAND_ATTACK = 2048,                  // creature will use offhand attacks
        NO_SELL_VENDOR = 4096,                      // players can't sell items to this vendor
        CANNOT_ENTER_COMBAT = 8192,                 // creature is not allowed to enter combat
        WORLDEVENT = 16384,                         // custom flag for world event creatures (left room for merging)
        GUARD = 32768,                              // Creature is guard
        IGNORE_FEIGN_DEATH = 65536,                 // creature ignores feign death
        NO_CRIT = 131072,                           // creature can't do critical strikes
        NO_SKILL_GAINS = 262144,                    // creature won't increase weapon skills
        OBEYS_TAUNT_DIMINISHING_RETURNS = 524288,   // Taunt is subject to diminishing returns on this creature
        ALL_DIMINISH = 1048576,                     // creature is subject to all diminishing returns as players are
        NO_PLAYER_DAMAGE_REQ = 2097152,             // creature does not need to take player damage for kill credit
        UNUSED_22 = 4194304,
        UNUSED_23 = 8388608,
        UNUSED_24 = 16777216,
        UNUSED_25 = 67108864,
        UNUSED_26 = 134217728,
        UNUSED_27 = 268435456,
        DUNGEON_BOSS = 536870912,                   // creature is a dungeon boss (SET DYNAMICALLY, DO NOT ADD IN DB)
        IGNORE_PATHFINDING = 1073741824,            // creature ignore pathfinding
        IMMUNITY_KNOCKBACK = 2147483648,            // creature is immune to knockback effects
        UNUSED_31 = 4294967296,
    }
}
