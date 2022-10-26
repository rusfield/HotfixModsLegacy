using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellPower
    {
        public int Id { get; set; }
        public byte OrderIndex { get; set; }
        public int ManaCost { get; set; }
        public int ManaCostPerLevel { get; set; }
        public int ManaPerSecond { get; set; }
        public uint PowerDisplayId { get; set; }
        public int AltPowerBarId { get; set; }
        public decimal PowerCostPct { get; set; }
        public decimal PowerCostMaxPct { get; set; }
        public decimal Field_10_0_2_45969_009 { get; set; }
        public decimal PowerPctPerSecond { get; set; }
        public sbyte PowerType { get; set; }
        public int RequiredAuraSpellId { get; set; }
        public uint OptionalCost { get; set; }
        public int SpellId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
