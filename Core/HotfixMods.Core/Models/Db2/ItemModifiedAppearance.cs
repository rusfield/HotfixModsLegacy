using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemModifiedAppearance
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        [ParentIndexField]
        public int ItemID { get; set; } = 0;
        public int ItemAppearanceModifierID { get; set; } = 0;
        public int ItemAppearanceID { get; set; } = 0;
        public int OrderIndex { get; set; } = 0;
        public byte TransmogSourceTypeEnum { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
