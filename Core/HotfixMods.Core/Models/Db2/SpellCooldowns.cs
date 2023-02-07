using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellCooldowns
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public byte DifficultyID { get; set; } = 0;
        public int CategoryRecoveryTime { get; set; } = 0;
        public int RecoveryTime { get; set; } = 0;
        public int StartRecoveryTime { get; set; } = 0;
        public int AuraSpellID { get; set; } = 0;
        [ParentIndexField]
        public int SpellID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
