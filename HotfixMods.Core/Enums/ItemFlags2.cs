using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Enums
{
    [Flags]
    public enum ItemFlags2 : long
    {
        DEFAULT = 0,
        DONT_DESTROY_ON_QUEST_ACCEPT = 1,
        CAN_BE_UPGRADED = 2,
        UPGRADE_OVERRIDES_DROP_UPGRADE = 4,
        ALWAYS_FFA_IN_LOOT = 8,
        HIDE_UPGRADE_IF_NOT_UPGRADED = 16,
        UPDATE_NPC_INTERACTIONS_ON_PICKUP = 32,
        NO_LEAVE_PROGRESSIVE_WIN_HISTORY = 64, // UPDATE_DOESNT_LEAVE_PROGRESSIVE_WIN_HISTORY
        IGNORE_HISTORY_TRACKER = 128,
        IGNORE_ITEM_LEVEL_CAP_PVP = 256,
        DISPLAY_AS_HEIRLOOM = 512, // Item appears as having heirloom quality ingame regardless of its real quality (does not affect stat calculation)
        SKIP_USE_CHECK_PICKUP = 1024,
        DEPRECATED_NO_LOOT_OVERFLOW_MAIL = 2048, // Obsolete
        DONT_DISPLAY_IN_GUILD_NEWS = 4096,
        PVP_TOURNAMENT_GEAR = 8192,
        REQUIRES_STACK_CHANGE_LOG = 16384, // Requires log line to be generated on stack count change
        TOY = 32768,
        HIDE_NAME_SUFFIX = 65536,
        PUSH_LOOT = 131072,
        DONT_REPORT_LOOT_TO_GROUP = 262144,
        ALWAYS_ALLOW_DUAL_WIELD = 524288,
        OBLITERATABLE = 1048576,
        TRANSMOG_HIDDEN_VISUAL_OPTION = 2097152,
        EXPIRE_ON_WEEKLY_RESET = 4194304,
        NOT_IN_TRANSMOG_UI_BEFORE_LOOT = 8388608,
        CAN_STORE_ENCHANTS = 16777216,
        HIDE_QUEST_ITEM_FROM_TOOLTIP = 33554432,
        DO_NOT_TOAST = 67108864,
        IGNORE_CREATION_CONTEXT_PROGRESSIVE_WIN_HISTORY = 134217728, // Ignore item creation context for progressive win history
        FORCE_ALL_SPECS_FOR_ITEM_HISTORY = 268435456,
        SAVE_AFTER_CONSUME = 536870912,
        CONTAINER_SAVE_PLAYER_DATA = 1073741824,
        NO_VOID_STORAGE = 2147483648,
    }
}
