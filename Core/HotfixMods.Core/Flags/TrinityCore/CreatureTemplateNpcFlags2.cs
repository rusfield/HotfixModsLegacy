namespace HotfixMods.Core.Flags.TrinityCore
{
    [Flags]
    public enum CreatureTemplateNpcFlags2 : uint
    {
        NONE = 0x00000000,
        ITEM_UPGRADE_MASTER = 0x00000001,   // TITLE is item upgrade
        GARRISON_ARCHITECT = 0x00000002,   // TITLE is garrison architect DESCRIPTION garrison building placement UI
        STEERING = 0x00000004,   // TITLE is avoiding obstacles DESCRIPTION clientside pathfinding
        SHIPMENT_CRAFTER = 0x00000010,   // TITLE is shipment crafter DESCRIPTION garrison work orders
        GARRISON_MISSION_NPC = 0x00000020,   // TITLE is garrison mission
        TRADESKILL_NPC = 0x00000040,   // TITLE is tradeskill DESCRIPTION crafting at npc
        BLACK_MARKET_VIEW = 0x00000080,   // TITLE is black market view DESCRIPTION only allows viewing black market auctions, no bidding
        GARRISON_TALENT_NPC = 0x00000200,   // TITLE is garrrison talent
        CONTRIBUTION_COLLECTOR = 0x00000400,   // TITLE is contribution collector
        AZERITE_RESPEC = 0x00004000,   // TITLE is azerite respec
        ISLANDS_QUEUE = 0x00008000,   // TITLE is islands queue
        SUPPRESS_NPC_SOUNDS_EXCEPT_END_OF_INTERACTION = 0x00010000,
    }
}
