using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [ClientOnly]
    public class AnimKitBoneSet
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public int BoneDataID { get; set; }
        public sbyte ParentAnimKitBoneSetID { get; set; }
        public sbyte AltAnimKitBoneSetID { get; set; }
        public int AltBoneDataID { get; set; }
    }

}
