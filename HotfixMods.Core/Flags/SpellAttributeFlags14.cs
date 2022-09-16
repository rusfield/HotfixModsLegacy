using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Flags
{
    [Flags]
    public enum SpellAttributeFlags14 : long
    {
        NONE = 0,
        PREVENT_JUMPING_DURING_PRECAST = 1
    }
}
