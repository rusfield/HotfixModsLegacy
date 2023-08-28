using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualKitEffect
    {
        
        public int ID { get; set; } = 0;
        public int EffectType { get; set; } = 1;
        public int Effect { get; set; } = 0;
        
        public int ParentSpellVisualKitID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
