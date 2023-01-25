using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemDisplayInfoModelMatRes
    {
        public int Id { get; set; } = 1;
        public int MaterialResourcesId { get; set; }
        public int TextureType { get; set; }
        public int ModelIndex { get; set; }
        public int ItemDisplayInfoId { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }

}
