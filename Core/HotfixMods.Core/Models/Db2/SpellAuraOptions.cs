using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellAuraOptions
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public byte DifficultyID { get; set; } = 0;
        public ushort CumulativeAura { get; set; } = 0;
        public int ProcCategoryRecovery { get; set; } = 0;
        public byte ProcChance { get; set; } = 0;
        public int ProcCharges { get; set; } = 0;
        public ushort SpellProcsPerMinuteID { get; set; } = 0;
        public int ProcTypeMask0 { get; set; } = 0;
        public int ProcTypeMask1 { get; set; } = 0;
        [ParentIndexField]
        public int SpellID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
