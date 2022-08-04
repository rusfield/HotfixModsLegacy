using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Flags
{
    [Flags]
    public enum ItemRaceFlags : long
    {
        // Some are probably missing here...

        ANY_HORDE_RACE = -6184943489809468494,
        ALL = -1,
        UNK_0 = 0,
        HUMAN = 1,
        UNK_2 = 2,
        DWARF = 4,
        UNK_8 = 8,
        UNK_16 = 16,
        TAUREN = 32,
        GNOME = 64,
        TROLL = 128,
        GOBLIN = 256,
        BLOOD_ELF = 512,
        UNK_1024 = 1024,
        DARK_IRON_DWARF = 2048,
        VULPERA = 4096,
        MAGHAR_ORC = 8192,
        MECHAGNOME = 16384,
        UNK_32768 = 32768,
        UNK_65536 = 65536,
        UNK_131072 = 131072,
        UNK_262144 = 262144,
        UNK_524288 = 524288,
        UNK_1048576 = 1048576,
        UNK_2097152 = 2097152,
        UNK_4194304 = 4194304,
        UNK_8388608 = 8388608,
        UNK_16777216 = 16777216,
        UNK_33554432 = 33554432,
        NIGHTBORNE = 67108864,
        HIGHMOUNTAIN_TAUREN = 134217728,
        VOID_ELF = 268435456,
        LIGHTFORGED_DRAENEI = 536870912,
        ZANDALARI_TROLL = 1073741824,
        KUL_TIRAN = 2147483648,
        ANY_ALLIANCE_RACE = 6130900294268439629
    }
}
