using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellAuraOptions
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public byte DifficultyId { get; set; } = 0;
        public ushort CumulativeAura { get; set; } = 0;
        public int ProcCategoryRecovery { get; set; } = 0;
        public byte ProcChance { get; set; } = 0;
        public int ProcCharges { get; set; } = 0;
        public ushort SpellProcsPerMinuteId { get; set; } = 0;
        public int ProcTypeMask1 { get; set; } = 0;
        public int ProcTypeMask2 { get; set; } = 0;
        [ParentIndexField]
        public int SpellId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
