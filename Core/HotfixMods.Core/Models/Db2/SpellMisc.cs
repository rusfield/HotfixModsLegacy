using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellMisc
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public int Attributes0 { get; set; } = 0;
        public int Attributes1 { get; set; } = 0;
        public int Attributes2 { get; set; } = 0;
        public int Attributes3 { get; set; } = 0;
        public int Attributes4 { get; set; } = 0;
        public int Attributes5 { get; set; } = 0;
        public int Attributes6 { get; set; } = 0;
        public int Attributes7 { get; set; } = 0;
        public int Attributes8 { get; set; } = 0;
        public int Attributes9 { get; set; } = 0;
        public int Attributes10 { get; set; } = 0;
        public int Attributes11 { get; set; } = 0;
        public int Attributes12 { get; set; } = 0;
        public int Attributes13 { get; set; } = 0;
        public int Attributes14 { get; set; } = 0;
        public byte DifficultyID { get; set; } = 0;
        public ushort CastingTimeIndex { get; set; } = 1;
        public ushort DurationIndex { get; set; } = 1;
        public ushort RangeIndex { get; set; } = 1;
        public byte SchoolMask { get; set; } = 0;
        public decimal Speed { get; set; } = 0;
        public decimal LaunchDelay { get; set; } = 0;
        public decimal MinDuration { get; set; } = 0;
        public int SpellIconFileDataID { get; set; } = 0;
        public int ActiveIconFileDataID { get; set; } = 0;
        public int ContentTuningID { get; set; } = 0;
        public int ShowFutureSpellPlayerConditionID { get; set; } = 0;
        public int SpellVisualScript { get; set; } = 0;
        public int ActiveSpellVisualScript { get; set; } = 0;
        [ParentIndexField]
        public int SpellID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
