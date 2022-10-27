using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Enums.Db2
{
    public enum ItemEffectTriggerType
    {
        ON_USE = 0,
        ON_EQUIP = 1,
        ON_PROC = 2,
        SUMMONED_BY_SPELL = 3,
        ON_DEATH = 4,
        ON_PICKUP = 5,
        ON_LEARN = 6,
        ON_LOOTED = 7,
        TEACH_MOUNT = 8,
    }
}
