using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Flags
{
    [Flags]
    public enum SpellVisualKitModelAttachFlags : long
    {
        NONE = 0,
        UNK_1 = 1,
        UNK_2 = 2,
        UNK_4 = 4,
        UNK_8 = 8,
        UNK_16 = 16,
        UNK_32 = 32,
        UNK_64 = 64,
        UNK_128 = 128,
        UNK_256 = 256,
        UNK_512 = 512
    }
}
