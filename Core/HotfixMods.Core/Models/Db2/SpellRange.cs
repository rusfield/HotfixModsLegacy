using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    // Client only
    public class SpellRange
    {
        public uint ID { get; set; } = 1;
        public string DisplayName { get; set; }
        public string DisplayNameShort { get; set; }
        public byte Flags { get; set; }
        public decimal RangeMin0 { get; set; }
        public decimal RangeMin1 { get; set; }
        public decimal RangeMax0 { get; set; }
        public decimal RangeMax1 { get; set; }
    }

}
