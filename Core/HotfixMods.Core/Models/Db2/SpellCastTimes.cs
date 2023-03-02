using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    // Client only
    public class SpellCastTimes
    {
        public uint ID { get; set; } = 1;
        public int Base { get; set; }
        public int Minimum { get; set; }
    }
}
