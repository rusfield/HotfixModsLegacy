using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Enums
{
    [Flags]
    public enum UnitFlags : long
    {
        NONE = 0,
        SERVER_CONTROLLED = 1,                        // set only when unit movement is controlled by server - by SPLINE/MONSTER_MOVE packets, together with STUNNED; only set to units controlled by client; client function CGUnit_C::IsClientControlled returns false when set for owner
        NON_ATTACKABLE = 2,                           // not attackable, set when creature starts to cast spells with SPELL_EFFECT_SPAWN and cast time, removed when spell hits caster, original name is SPAWNING. Rename when it will be removed from all scripts
        REMOVE_CLIENT_CONTROL = 4,                    // This is a legacy flag used to disable movement player's movement while controlling other units, SMSG_CLIENT_CONTROL replaces this functionality clientside now. CONFUSED and FLEEING flags have the same effect on client movement asDISABLE_MOVE_CONTROL in addition to preventing spell casts/autoattack (they all allow climbing steeper hills and emotes while moving)
        PLAYER_CONTROLLED = 8,                        // controlled by player, use _IMMUNE_TO_PC instead of _IMMUNE_TO_NPC
        RENAME = 16,
        PREPARATION = 32,                             // don't take reagents for spells with SPELL_ATTR5_NO_REAGENT_WHILE_PREP
        UNK_6 = 64,
        NOT_ATTACKABLE_1 = 128,                       // ?? (PLAYER_CONTROLLED | NOT_ATTACKABLE_1) is NON_PVP_ATTACKABLE
        IMMUNE_TO_PC = 256,                           // disables combat/assistance with PlayerCharacters (PC) - see Unit::IsValidAttackTarget, Unit::IsValidAssistTarget
        IMMUNE_TO_NPC = 512,                          // disables combat/assistance with NonPlayerCharacters (NPC) - see Unit::IsValidAttackTarget, Unit::IsValidAssistTarget
        LOOTING = 1024,                               // loot animation
        PET_IN_COMBAT = 2048,                         // on player pets: whether the pet is chasing a target to attack || on other units: whether any of the unit's minions is in combat
        PVP_ENABLING = 4096,                          // changed in 3.0.3, now UNIT_BYTES_2_OFFSET_PVP_FLAG from UNIT_FIELD_BYTES_2
        SILENCED = 8192,                              // silenced, 2.1.1
        CANT_SWIM = 16384,                            // TITLE Can't Swim
        CAN_SWIM = 32768,                             // TITLE Can Swim DESCRIPTION shows swim animation in water
        NON_ATTACKABLE_2 = 65536,                     // removes attackable icon, if on yourself, cannot assist self but can cast TARGET_SELF spells - added by SPELL_AURA_MOD_UNATTACKABLE
        PACIFIED = 131072,                            // 3.0.3 ok
        STUNNED = 262144,                             // 3.0.3 ok
        IN_COMBAT = 524288,
        ON_TAXI = 1048576,                            // disable casting at client side spell not allowed by taxi flight (mounted?), probably used with 0x4 flag
        DISARMED = 2097152,                           // 3.0.3, disable melee spells casting..., "Required melee weapon" added to melee spells tooltip.
        CONFUSED = 4194304,
        FLEEING = 8388608,
        POSSESSED = 16777216,                         // under direct client control by a player (possess or vehicle)
        UNINTERACTIBLE = 33554432,
        SKINNABLE = 67108864,
        MOUNT = 134217728,
        UNK_28 = 268435456,
        PREVENT_EMOTES_FROM_CHAT_TEXT = 536870912,    // Prevent automatically playing emotes from parsing chat text, for example "lol" in /say, ending message with ? or !, or using /yell
        SHEATHE = 1073741824,
        IMMUNE = 2147483648,                          // Immune to damage
    };
}

