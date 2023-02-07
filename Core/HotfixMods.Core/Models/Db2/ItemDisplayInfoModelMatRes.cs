using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemDisplayInfoModelMatRes
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public int MaterialResourcesID { get; set; }
        public int TextureType { get; set; }
        public int ModelIndex { get; set; }
        [ParentIndexField]
        public int ItemDisplayInfoID { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
