using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Enums
{
    [Flags]
    public enum ItemFlags1 : long
    {
        DEFAULT = 0,
        HORDE_SPECIFIC = 1,
        ALLIANCE_SPECIFIC = 2,
        DEPRECATED_DONT_IGNORE_BUY_PRICE = 4, // When item uses extended cost, gold is also required (possibly deprecated)
        CASTER_NEED_ROLL_ONLY = 8,
        MELEE_NEED_ROLL_ONLY = 16,
        EVERYONE_CAN_NEED_ROLL = 32,
        NO_TRADE_ON_PICKUP = 64,
        CAN_TRADE_ON_PICKUP = 128,
        CAN_ONLY_ROLL_GREED = 256,
        CASTER_WEAPON = 512,
        DELETE_ON_LOGIN = 1024,
        TEST_ITEM = 2048,
        NO_VENDOR_VALUE = 4096, // Vendors will not purchase this item (has no vendor value)
        APPEAR_IN_TRANSMOG_BEFORE_COLLECT = 8192,
        OVERRIDE_DEFAULT_COST_PRICE = 16384,
        IGNORE_RATED_BG_RESTRICTIONS = 32768,
        NOT_USABLE_IN_RATED_BG = 65536,
        BOUND_TO_BNET_ACCOUNT = 131072, // Must be Bound To Account on Flag0 as well
        CONFIRM_BEFORE_USE = 262144, // Ask for confirmation before use
        REEVALUATE_BONDING_ON_TRANSFORM = 524288, // Reevaluate binding when transforming to this item
        NO_TRANSFORM_ON_CHARGE_DEPLETION = 1048576, // Don\'t transform when all charges are consumed
        NO_ALTER_ITEM_VISUAL = 2097152, // Cannot alter this item's visual appearance
        NO_SOURCE_FOR_ITEM_VISUAL = 4194304, // Cannot alter items to look like this item
        IGNORE_QUALITY_FOR_ITEM_VISUAL_SOURCE = 8388608, // Can be used as a source for item visual regardless of quality
        NO_DURABILITY = 16777216,
        ONLY_TANK_NEED_IN_LFR = 33554432,
        ONLY_HEALER_NEED_IN_LFR = 67108864,
        ONLY_DPS_NEED_IN_LFR = 134217728,
        CAN_DROP_IN_CHALLENGE_MODE = 268435456,
        NO_STACK_ON_LOOT_UI = 536870912,
        DISENCHANT_TO_LOOT_TABLE = 1073741824,
        CAN_BE_PLACED_IN_REAGENT_BANK = 2147483648
    }
}
