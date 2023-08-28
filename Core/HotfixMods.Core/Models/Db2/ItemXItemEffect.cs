using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemXItemEffect
    {
        
        public int ID { get; set; } = 0;
        public int ItemEffectID { get; set; } = 0;
        
        public int ItemID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
