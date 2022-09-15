using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Flags
{
    [Flags]
    public enum DamageClass
    {
        NONE = 0,
        PHYSICAL = 1,
        HOLY = 2,
        FIRE = 4,
        NATURE = 8,
        FROST = 16,
        SHADOW = 32,
        ARCANE = 64
    }
}
