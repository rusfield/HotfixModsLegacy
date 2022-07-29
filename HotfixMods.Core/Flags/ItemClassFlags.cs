using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Flags
{
    [Flags]
    public enum ItemClassFlags
    {
        ALL = -1,
        WARRIOR = 1,
        PALADIN = 2,
        HUNTER = 4,
        ROGUE = 8,
        PRIEST = 16,
        DEATH_KNIGHT = 32,
        SHAMAN = 64,
        MAGE = 128,
        WARLOCK = 256,
        MONK = 512,
        DRUID = 1024,
        DEMON_HUNTER = 2048
    }
}
