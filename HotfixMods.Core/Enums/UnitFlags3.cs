using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Enums
{
    [Flags]
    public enum UnitFlags3 : long
    {
        NONE = 0,
        UNK0 = 1,
        UNCONSCIOUS_ON_DEATH = 2,                            // TITLE Unconscious on Death DESCRIPTION Shows "Unconscious" in unit tooltip instead of "Dead"
        ALLOW_MOUNTED_COMBAT = 4,                            // TITLE Allow mounted combat
        GARRISON_PET = 8,                                    // TITLE Garrison pet DESCRIPTION Special garrison pet creatures that display one of favorite player battle pets - this flag allows querying name and turns off default battle pet behavior
        UI_CAN_GET_POSITION = 16,                            // TITLE UI Can Get Position DESCRIPTION Allows lua functions like UnitPosition to always get the position even for npcs or non-grouped players
        AI_OBSTACLE = 32,
        ALTERNATIVE_DEFAULT_LANGUAGE = 64,
        SUPPRESS_ALL_NPC_FEEDBACK = 128,                     // TITLE Suppress all NPC feedback DESCRIPTION Skips playing sounds on left clicking npc for all npcs as long as npc with this flag is visible
        IGNORE_COMBAT = 256,                                 // TITLE Ignore Combat DESCRIPTION Same as SPELL_AURA_IGNORE_COMBAT
        SUPPRESS_NPC_FEEDBACK = 512,                         // TITLE Suppress NPC feedback DESCRIPTION Skips playing sounds on left clicking npc
        UNK10 = 1024,
        UNK11 = 2048,
        UNK12 = 4096,
        FAKE_DEAD = 8192,                                    // TITLE Show as dead
        NO_FACING_ON_INTERACT_AND_FAST_FACING_CHASE = 16384, // Causes the creature to both not change facing on interaction and speeds up smooth facing changes while attacking (clientside)
        UNTARGETABLE_FROM_UI = 32768,                        // TITLE Untargetable from UI DESCRIPTION Cannot be targeted from lua functions StartAttack, TargetUnit, PetAttack
        NO_FACING_ON_INTERACT_WHILE_FAKE_DEAD = 65536,       // Prevents facing changes while interacting if creature has flag FAKE_DEAD
        ALREADY_SKINNED = 131072,
        SUPPRESS_ALL_NPC_SOUNDS = 262144,                    // TITLE Suppress all NPC sounds DESCRIPTION Skips playing sounds on beginning and end of npc interaction for all npcs as long as npc with this flag is visible
        SUPPRESS_NPC_SOUNDS = 524288,                        // TITLE Suppress NPC sounds DESCRIPTION Skips playing sounds on beginning and end of npc interaction
        UNK20 = 1048576,
        UNK21 = 2097152,
        DONT_FADE_OUT = 4194304,
        UNK23 = 8388608,
        UNK24 = 16777216,
        UNK25 = 33554432,
        UNK26 = 67108864,
        UNK27 = 134217728,
        UNK28 = 268435456,
        UNK29 = 536870912,
        UNK30 = 1073741824,
        UNK31 = 2147483648,
    }
}
