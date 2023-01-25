using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKitConfigBoneSet
    {
        [IndexField]
        public int Id { get; set; }
        public byte AnimKitBoneSetId { get; set; }
        public ushort AnimKitPriorityId { get; set; }
        [ParentIndexField]
        public int ParentAnimKitConfigId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
