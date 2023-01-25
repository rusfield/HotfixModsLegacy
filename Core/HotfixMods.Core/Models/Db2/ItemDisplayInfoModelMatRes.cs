using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemDisplayInfoModelMatRes
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public int MaterialResourcesId { get; set; }
        public int TextureType { get; set; }
        public int ModelIndex { get; set; }
        [ParentIndexField]
        public int ItemDisplayInfoId { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
