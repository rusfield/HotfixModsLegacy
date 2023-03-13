using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualEvent
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public int StartEvent { get; set; } = 0;
        public int EndEvent { get; set; } = 0;
        public int StartMinOffsetMs { get; set; } = 0;
        public int StartMaxOffsetMs { get; set; } = 0;
        public int EndMinOffsetMs { get; set; } = 0;
        public int EndMaxOffsetMs { get; set; } = 0;
        public int TargetType { get; set; } = 0;
        public int SpellVisualKitID { get; set; } = 0;
        public int Field_10_0_0_44649_008 { get; set; } = 0;
        public int Field_10_0_0_44649_009 { get; set; } = 0;
        [ParentIndexField]
        public int SpellVisualID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
