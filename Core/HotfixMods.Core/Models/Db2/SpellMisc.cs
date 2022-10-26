using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellMisc
    {
        public int Id { get; set; }
        public int Attributes1 { get; set; }
        public int Attributes2 { get; set; }
        public int Attributes3 { get; set; }
        public int Attributes4 { get; set; }
        public int Attributes5 { get; set; }
        public int Attributes6 { get; set; }
        public int Attributes7 { get; set; }
        public int Attributes8 { get; set; }
        public int Attributes9 { get; set; }
        public int Attributes10 { get; set; }
        public int Attributes11 { get; set; }
        public int Attributes12 { get; set; }
        public int Attributes13 { get; set; }
        public int Attributes14 { get; set; }
        public int Attributes15 { get; set; }
        public byte DifficultyId { get; set; }
        public ushort CastingTimeIndex { get; set; }
        public ushort DurationIndex { get; set; }
        public ushort RangeIndex { get; set; }
        public byte SchoolMask { get; set; }
        public decimal Speed { get; set; }
        public decimal LaunchDelay { get; set; }
        public decimal MinDuration { get; set; }
        public int SpellIconFileDataId { get; set; }
        public int ActiveIconFileDataId { get; set; }
        public int ContentTuningId { get; set; }
        public int ShowFutureSpellPlayerConditionId { get; set; }
        public int SpellVisualScript { get; set; }
        public int ActiveSpellVisualScript { get; set; }
        public int SpellId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
