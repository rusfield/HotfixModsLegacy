using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellAuraOptions
    {
        public int Id { get; set; }
        public byte DifficultyId { get; set; }
        public ushort CumulativeAura { get; set; }
        public int ProcCategoryRecovery { get; set; }
        public byte ProcChance { get; set; }
        public int ProcCharges { get; set; }
        public ushort SpellProcsPerMinuteId { get; set; }
        public int ProcTypeMask1 { get; set; }
        public int ProcTypeMask2 { get; set; }
        public int SpellId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
