using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Enums
{
    [Flags]
    public enum ItemFlags0 : long
    {
        NONE = 0,
        NO_PICKUP = 1,
        CONJURED = 2,
        HAS_LOOT_TABLE = 4, // Item can be right clicked to open for loot
        HEROIC_TOOLTIP = 8,
        DEPRECATED = 16, // Cannot equip or use
        NO_USER_DESTROY = 32,
        PLAYER_CAST = 64, // Item's spells are castable by players
        NO_EQUIP_COOLDOWN = 128, // No default 30 sec cooldown on equip
        MULTI_LOOT_QUEST = 256,
        GIFT_WRAP = 512, // Item can wrap otehr items
        USES_RESOURCES = 1024,
        MULTI_DROP = 2048, // Looting this item does not remove it from available loot
        IN_GAME_REFUND = 4096, // ITEM_PURCHASE_RECORD -- Item can be returned to vendor for its original cost (extended cost)
        PETITION = 8192, // Item is guild or arena charter
        HAS_TEXT = 16384, // Only readable items has this (but not all)
        NO_DISENCHANT = 32768,
        REAL_DURATION = 65536,
        NO_CREATOR = 131072,
        PROSPECTABLE = 262144,
        UNIQUE_EQUIPPABLE = 524288,
        DISABLE_AURAS_QUOTAS = 1048576, // 'Disable Auto Qutoes' -- IGNORE_FOR_AURAS
        IGNORE_DEFAULT_ARENA_RESTRICTIONS = 2097152, // Item can be used during arena
        NO_DURABILITY_LOSS = 4194304, // Only some thrown items has this
        USABLE_WHILE_SHAPESHIFTED = 8388608,
        HAS_QUEST_GLOW = 16777216,
        HIDE_UNUSABLE_RECIPE = 33554432, // Profession recipes can only be looted if you meet requirements and don't already know it
        NOT_USABLE_IN_ARENA = 67108864,
        BOUND_TO_ACCOUNT = 134217728,
        NO_REAGENT_COST = 268435456, // Spell is cast ignoring reagents
        MILLABLE = 536870912,
        REPORT_TO_GUILD_CHAT = 1073741824,
        NO_DYNAMIC_DROP_CHANCE = 2147483648, // Dont use dynamic drop chance (quest)
    }
}
