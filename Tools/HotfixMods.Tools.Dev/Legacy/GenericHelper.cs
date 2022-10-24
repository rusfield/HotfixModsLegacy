using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Tools.Dev.Legacy
{
    public static class GenericHelper
    {
        public static void GenerateEnumValues(long maxValue)
        {
            Console.WriteLine("NONE = 0");
            for (long i = 1; i <= maxValue; i = i * 2)
            {
                Console.WriteLine($"UNK_{i} = {i},");
            }
        }
    }
}
