using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    // Client only
    public class SpellDuration
    {
        public uint ID { get; set; } = 1;
        public int Duration { get; set; }
        public int MaxDuration { get; set; }
    }

}
