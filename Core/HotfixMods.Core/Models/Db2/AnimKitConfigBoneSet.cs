using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKitConfigBoneSet
    {
        [IndexField]
        public int ID { get; set; } = 0;
        [Db2Description("The ID of the bone set that the current segment will be played on.$All bone sets except Full Body has a parent, so this property can be used to isolate an animation on a specific body part. Remember to set Priority accordingly.")]
        public byte AnimKitBoneSetID { get; set; } = 0;
        [Db2Description("The ID of the priority that the current segment will be played with.$With multiple segments, the client needs to know in what priority the bone set in the current segment shall be played on.$The higher the number, the higher the priority.")]
        public ushort AnimKitPriorityID { get; set; } = 0;
        [ParentIndexField]
        public int ParentAnimKitConfigID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
