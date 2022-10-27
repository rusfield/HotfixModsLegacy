using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Enums.Db2
{
    public enum ItemModifiedAppearanceTransmogSourceType
    {
        UNKNOWN = 0,
        DUNGEON_JOURNAL_ENCOUNTER = 1,
        QUEST = 2,
        VENDOR = 3,
        WORLD_DROP = 4,
        HIDDEN_UNTIL_COLLECTED = 5,
        CAN_NOT_COLLECT = 6,
        ACHIEVEMENT = 7,
        PROFESSION = 8,
        NOT_VALID_FOR_TRANSMOG = 9,
    }
}
