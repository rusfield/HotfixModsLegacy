using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualScreenEffect
    {
        
        public int ID { get; set; } = 0;
        public int ScreenEffectID { get; set; } = 0;
        public int ScreenEffectTypeID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
