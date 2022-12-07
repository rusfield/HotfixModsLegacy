using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualEvent
    {
        public int Id { get; set; } = 0;
        public int StartEvent { get; set; } = 0;
        public int EndEvent { get; set; } = 0;
        public int StartMinOffsetMs { get; set; } = 0;
        public int StartMaxOffsetMs { get; set; } = 0;
        public int EndMinOffsetMs { get; set; } = 0;
        public int EndMaxOffsetMs { get; set; } = 0;
        public int TargetType { get; set; } = 0;
        public int SpellVisualKitId { get; set; } = 0;
        public int Field_10_0_0_44649_008 { get; set; } = 0;
        public int Field_10_0_0_44649_009 { get; set; } = 0;
        public int SpellVisualId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
