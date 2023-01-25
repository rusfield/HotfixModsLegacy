using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemXItemEffect
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public int ItemEffectId { get; set; } = 0;
        [ParentIndexField]
        public int ItemId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
