using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKitConfigBoneSet
    {
        [IndexField]
        public uint ID { get; set; }
        public byte AnimKitBoneSetID { get; set; }
        public ushort AnimKitPriorityID { get; set; }
        [ParentIndexField]
        public int ParentAnimKitConfigID { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
