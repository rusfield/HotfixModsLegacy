using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellMisc
    {
        public int Id { get; set; } = 0;
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
        public int Attributes15 { get; set; } = 0;
        public byte DifficultyId { get; set; } = 0;
        public ushort CastingTimeIndex { get; set; } = 0;
        public ushort DurationIndex { get; set; } = 0;
        public ushort RangeIndex { get; set; } = 0;
        public byte SchoolMask { get; set; } = 0;
        public decimal Speed { get; set; } = 0;
        public decimal LaunchDelay { get; set; } = 0;
        public decimal MinDuration { get; set; } = 0;
        public int SpellIconFileDataId { get; set; } = 0;
        public int ActiveIconFileDataId { get; set; } = 0;
        public int ContentTuningId { get; set; } = 0;
        public int ShowFutureSpellPlayerConditionId { get; set; } = 0;
        public int SpellVisualScript { get; set; } = 0;
        public int ActiveSpellVisualScript { get; set; } = 0;
        public int SpellId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
