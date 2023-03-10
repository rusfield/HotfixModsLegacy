namespace HotfixMods.Core.Flags.TrinityCore
{
    [Flags]
    public enum CreatureTemplateFlagsExtra : uint
    {
        NONE = 0,
        INSTANCE_BIND = 0x00000001,       // creature kill bind instance with killer and killer's group
        CIVILIAN = 0x00000002,       // not aggro (ignore faction/reputation hostility)
        NO_PARRY = 0x00000004,       // creature can't parry
        NO_PARRY_HASTEN = 0x00000008,       // creature can't counter-attack at parry
        NO_BLOCK = 0x00000010,       // creature can't block
        NO_CRUSHING_BLOWS = 0x00000020,       // creature can't do crush attacks
        NO_XP = 0x00000040,       // creature kill does not provide XP
        TRIGGER = 0x00000080,       // trigger creature
        NO_TAUNT = 0x00000100,       // creature is immune to taunt auras and 'attack me' effects
        NO_MOVE_FLAGS_UPDATE = 0x00000200,       // creature won't update movement flags
        GHOST_VISIBILITY = 0x00000400,       // creature will only be visible to dead players
        USE_OFFHAND_ATTACK = 0x00000800,       // creature will use offhand attacks
        NO_SELL_VENDOR = 0x00001000,       // players can't sell items to this vendor
        CANNOT_ENTER_COMBAT = 0x00002000,       // creature is not allowed to enter combat
        WORLDEVENT = 0x00004000,       // custom flag for world event creatures (left room for merging)
        GUARD = 0x00008000,       // Creature is guard
        IGNORE_FEIGN_DEATH = 0x00010000,       // creature ignores feign death
        NO_CRIT = 0x00020000,       // creature can't do critical strikes
        NO_SKILL_GAINS = 0x00040000,       // creature won't increase weapon skills
        OBEYS_TAUNT_DIMINISHING_RETURNS = 0x00080000,       // Taunt is subject to diminishing returns on this creature
        ALL_DIMINISH = 0x00100000,       // creature is subject to all diminishing returns as players are
        NO_PLAYER_DAMAGE_REQ = 0x00200000,       // creature does not need to take player damage for kill credit
        UNUSED_22 = 0x00400000,
        UNUSED_23 = 0x00800000,
        UNUSED_24 = 0x01000000,
        UNUSED_25 = 0x02000000,
        UNUSED_26 = 0x04000000,
        UNUSED_27 = 0x08000000,
        DUNGEON_BOSS = 0x10000000,       // creature is a dungeon boss (SET DYNAMICALLY, DO NOT ADD IN DB)
        IGNORE_PATHFINDING = 0x20000000,       // creature ignore pathfinding
        IMMUNITY_KNOCKBACK = 0x40000000,       // creature is immune to knockback effects
        UNUSED_31 = 0x80000000,
    }
}
