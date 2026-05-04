using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemXBonusTree
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public ushort ItemBonusTreeID { get; set; } = 0;
        [ParentIndexField]
        public int ItemID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
