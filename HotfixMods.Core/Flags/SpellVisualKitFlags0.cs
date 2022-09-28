using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Flags
{
    [Flags]
    public enum SpellVisualKitFlags0 : long
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
        UNK_512 = 512,
        UNK_1024 = 1024,
        UNK_2048 = 2048,
        PLAYER_ONLY = 4096, // at least for items
        UNK_8192 = 8192,
        UNK_16384 = 16384,
        UNK_32768 = 32768,
        UNK_65536 = 65536,
        UNK_131072 = 131072,
        UNK_262144 = 262144,
        UNK_524288 = 524288,
        UNK_1048576 = 1048576,
        UNK_2097152 = 2097152,
        NPC_ONLY = 4194304, // at least for items
        UNK_8388608 = 8388608,
        UNK_16777216 = 16777216,
        UNK_33554432 = 33554432,
        UNK_67108864 = 67108864,
        UNK_134217728 = 134217728,
        UNK_268435456 = 268435456,
        UNK_536870912 = 536870912,
        UNK_1073741824 = 1073741824,
        UNK_2147483648 = 2147483648,
    }
}
