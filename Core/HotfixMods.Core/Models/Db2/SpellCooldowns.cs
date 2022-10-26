using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellCooldowns
    {
        public int Id { get; set; }
        public byte DifficultyId { get; set; }
        public int CategoryRecoveryTime { get; set; }
        public int RecoveryTime { get; set; }
        public int StartRecoveryTime { get; set; }
        public int BuffSpellId { get; set; }
        public int SpellId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
