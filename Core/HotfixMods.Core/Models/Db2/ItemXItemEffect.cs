using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemXItemEffect
    {
        public int Id { get; set; }
        public int ItemEffectId { get; set; }
        public int ItemId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
