using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellCooldowns
    {
        [IndexField]
        public uint Id { get; set; } = 0;
        public byte DifficultyId { get; set; } = 0;
        public int CategoryRecoveryTime { get; set; } = 0;
        public int RecoveryTime { get; set; } = 0;
        public int StartRecoveryTime { get; set; } = 0;
        public int AuraSpellId { get; set; } = 0;
        [ParentIndexField]
        public int SpellId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
