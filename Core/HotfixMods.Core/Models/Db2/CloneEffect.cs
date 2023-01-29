using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CloneEffect
    {
        [IndexField]
        public uint Id { get; set; } = 0;
        public int DurationMs { get; set; } = 0;
        public int DelayMs { get; set; } = 0;
        public int FadeInTimeMs { get; set; } = 0; 
        public int FadeOutTimeMs { get; set; } = 0;
        public int StateSpellVisualKitId { get; set; } = 0;
        public int StartSpellVisualKitId { get; set; } = 0;
        public int OffsetMatrixId { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
