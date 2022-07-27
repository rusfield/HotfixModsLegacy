using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Enums
{
    [Flags]
    public enum UnitFlags2 : long
    {
        NONE = 0,
        FEIGN_DEATH = 1,
        HIDE_BODY = 2,                                               // TITLE Hide Body DESCRIPTION Hide unit model (show only player equip)
        IGNORE_REPUTATION = 4,
        COMPREHEND_LANG = 8,
        MIRROR_IMAGE = 16,
        DONT_FADE_IN = 32,                                           // TITLE Don't Fade In DESCRIPTION Unit model instantly appears when summoned (does not fade in)
        FORCE_MOVEMENT = 64,
        DISARM_OFFHAND = 128,
        DISABLE_PRED_STATS = 256,                                    // Player has disabled predicted stats (Used by raid frames)
        ALLOW_CHANGING_TALENTS = 512,                                // Allows changing talents outside rest area
        DISARM_RANGED = 1024,                                        // this does not disable ranged weapon display (maybe additional flag needed?)
        REGENERATE_POWER = 2048,
        RESTRICT_PARTY_INTERACTION = 4096,                           // Restrict interaction to party or raid
        PREVENT_SPELL_CLICK = 8192,                                  // Prevent spellclick
        INTERACT_WHILE_HOSTILE = 16384,                              // TITLE Interact while Hostile
        CANNOT_TURN = 32768,                                         // TITLE Cannot Turn
        UNK2 = 65536,
        PLAY_DEATH_ANIM = 131072,                                    // Plays special death animation upon death
        ALLOW_CHEAT_SPELLS = 262144,                                 // Allows casting spells with AttributesEx7 & SPELL_ATTR7_IS_CHEAT_SPELL
        SUPPRESS_HIGHLIGHT_WHEN_TARGETED_OR_MOUSED_OVER = 524288,    // TITLE Suppress highlight when targeted or moused over
        TREAT_AS_RAID_UNIT_FOR_HELPFUL_SPELLS = 1048576,             // TITLE Treat as Raid Unit For Helpful Spells (Instances ONLY)
        LARGE_AOI = 2097152,                                         // TITLE Large (AOI)
        GIGANTIC_AOI = 4194304,                                      // TITLE Gigantic (AOI)
        NO_ACTIONS = 8388608,
        AI_WILL_ONLY_SWIM_IF_TARGET_SWIMS = 134217728,               // TITLE AI will only swim if target swims
        DONT_GENERATE_COMBAT_LOG_WHEN_ENGAGED_WITH_NPCS = 268435456, // TITLE Don't generate combat log when engaged with NPC's
        UNTARGETABLE_BY_CLIENT = 536870912,                          // TITLE Untargetable By Client
        ATTACKER_IGNORES_MINIMUM_RANGES = 1073741824,                // TITLE Attacker Ignores Minimum Ranges
        UNINTERACTIBLE_IF_HOSTILE = 2147483648,                      // TITLE Uninteractible If Hostile
        UNUSED_11 = 4294967296,
        INFINITE_AOI = 8589934592,                                   // TITLE Infinite (AOI)
        UNUSED_13 = 17179869184,
    }
}
