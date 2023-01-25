using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellPower
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public byte OrderIndex { get; set; } = 0;
        public int ManaCost { get; set; } = 0;
        public int ManaCostPerLevel { get; set; } = 0;
        public int ManaPerSecond { get; set; } = 0;
        public uint PowerDisplayId { get; set; } = 0;
        public int AltPowerBarId { get; set; } = 0;
        public decimal PowerCostPct { get; set; } = 0;
        public decimal PowerCostMaxPct { get; set; } = 0;
        public decimal OptionalCostPct { get; set; } = 0;
        public decimal PowerPctPerSecond { get; set; } = 0;
        public sbyte PowerType { get; set; } = 0;
        public int RequiredAuraSpellId { get; set; } = 0;
        public uint OptionalCost { get; set; } = 0;
        [ParentIndexField]
        public int SpellId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
