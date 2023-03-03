using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKitConfigBoneSet
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public byte AnimKitBoneSetID { get; set; } = 0;
        public ushort AnimKitPriorityID { get; set; } = 0;
        [ParentIndexField]
        public int ParentAnimKitConfigID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
