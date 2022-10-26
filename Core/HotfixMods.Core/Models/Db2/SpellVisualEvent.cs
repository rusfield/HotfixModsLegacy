using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualEvent
    {
        public int Id { get; set; }
        public int StartEvent { get; set; }
        public int EndEvent { get; set; }
        public int StartMinOffsetMs { get; set; }
        public int StartMaxOffsetMs { get; set; }
        public int EndMinOffsetMs { get; set; }
        public int EndMaxOffsetMs { get; set; }
        public int TargetType { get; set; }
        public int SpellVisualKitId { get; set; }
        public int Field_10_0_0_44649_008 { get; set; }
        public int Field_10_0_0_44649_009 { get; set; }
        public int SpellVisualId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
