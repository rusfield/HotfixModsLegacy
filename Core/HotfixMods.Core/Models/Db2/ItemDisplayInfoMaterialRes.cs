using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemDisplayInfoMaterialRes
    {
        public int Id { get; set; }
        public sbyte ComponentSection { get; set; }
        public int MaterialResourcesId { get; set; }
        public int ItemDisplayInfoId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
