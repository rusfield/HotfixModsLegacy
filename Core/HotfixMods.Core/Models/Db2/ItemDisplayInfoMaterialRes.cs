using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemDisplayInfoMaterialRes
    {
        public int ID { get; set; } = 0;
        public sbyte ComponentSection { get; set; } = 0;
        public int MaterialResourcesID { get; set; } = 0;
        [ParentIndexField] 
        public int ItemDisplayInfoID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
