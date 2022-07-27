using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Enums
{
    public enum ItemBondings
    {
        NOT_BOUND = 0,
        BINDS_WHEN_PICKED_UP = 1,
        BINDS_WHEN_EQUIPPED = 2,
        BINDS_ON_USE = 3,
        QUEST_ITEM = 4
    }
}
