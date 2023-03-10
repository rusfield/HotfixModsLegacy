namespace HotfixMods.Core.Flags.TrinityCore
{
    [Flags]
    public enum CreatureTemplateUnitFlags3 : uint
    {
        NONE = 0,
        UNIT_FLAG3_UNK0 = 0x00000001,
        UNIT_FLAG3_UNCONSCIOUS_ON_DEATH = 0x00000002,   // TITLE Unconscious on Death DESCRIPTION Shows "Unconscious" in unit tooltip instead of "Dead"
        UNIT_FLAG3_ALLOW_MOUNTED_COMBAT = 0x00000004,   // TITLE Allow mounted combat
        UNIT_FLAG3_GARRISON_PET = 0x00000008,   // TITLE Garrison pet DESCRIPTION Special garrison pet creatures that display one of favorite player battle pets - this flag allows querying name and turns off default battle pet behavior
        UNIT_FLAG3_UI_CAN_GET_POSITION = 0x00000010,   // TITLE UI Can Get Position DESCRIPTION Allows lua functions like UnitPosition to always get the position even for npcs or non-grouped players
        UNIT_FLAG3_AI_OBSTACLE = 0x00000020,
        UNIT_FLAG3_ALTERNATIVE_DEFAULT_LANGUAGE = 0x00000040,
        UNIT_FLAG3_SUPPRESS_ALL_NPC_FEEDBACK = 0x00000080,   // TITLE Suppress all NPC feedback DESCRIPTION Skips playing sounds on left clicking npc for all npcs as long as npc with this flag is visible
        UNIT_FLAG3_IGNORE_COMBAT = 0x00000100,   // TITLE Ignore Combat DESCRIPTION Same as SPELL_AURA_IGNORE_COMBAT
        UNIT_FLAG3_SUPPRESS_NPC_FEEDBACK = 0x00000200,   // TITLE Suppress NPC feedback DESCRIPTION Skips playing sounds on left clicking npc
        UNIT_FLAG3_UNK10 = 0x00000400,
        UNIT_FLAG3_UNK11 = 0x00000800,
        UNIT_FLAG3_UNK12 = 0x00001000,
        UNIT_FLAG3_FAKE_DEAD = 0x00002000,   // TITLE Show as dead
        UNIT_FLAG3_NO_FACING_ON_INTERACT_AND_FAST_FACING_CHASE = 0x00004000,   // Causes the creature to both not change facing on interaction and speeds up smooth facing changes while attacking (clientside)
        UNIT_FLAG3_UNTARGETABLE_FROM_UI = 0x00008000,   // TITLE Untargetable from UI DESCRIPTION Cannot be targeted from lua functions StartAttack, TargetUnit, PetAttack
        UNIT_FLAG3_NO_FACING_ON_INTERACT_WHILE_FAKE_DEAD = 0x00010000,   // Prevents facing changes while interacting if creature has flag UNIT_FLAG3_FAKE_DEAD
        UNIT_FLAG3_ALREADY_SKINNED = 0x00020000,
        UNIT_FLAG3_SUPPRESS_ALL_NPC_SOUNDS = 0x00040000,   // TITLE Suppress all NPC sounds DESCRIPTION Skips playing sounds on beginning and end of npc interaction for all npcs as long as npc with this flag is visible
        UNIT_FLAG3_SUPPRESS_NPC_SOUNDS = 0x00080000,   // TITLE Suppress NPC sounds DESCRIPTION Skips playing sounds on beginning and end of npc interaction
        UNIT_FLAG3_UNK20 = 0x00100000,
        UNIT_FLAG3_UNK21 = 0x00200000,
        UNIT_FLAG3_DONT_FADE_OUT = 0x00400000,
        UNIT_FLAG3_UNK23 = 0x00800000,
        UNIT_FLAG3_FORCE_HIDE_NAMEPLATE = 0x01000000,
        UNIT_FLAG3_UNK25 = 0x02000000,
        UNIT_FLAG3_UNK26 = 0x04000000,
        UNIT_FLAG3_UNK27 = 0x08000000,
        UNIT_FLAG3_UNK28 = 0x10000000,
        UNIT_FLAG3_UNK29 = 0x20000000,
        UNIT_FLAG3_UNK30 = 0x40000000,
        UNIT_FLAG3_UNK31 = 0x80000000,
    }
}
