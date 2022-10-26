using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualKitEffect
    {
        public int Id { get; set; }
        public int EffectType { get; set; }
        public int Effect { get; set; }
        public int ParentSpellVisualKitId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
