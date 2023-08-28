using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CloneEffect
    {
        
        public int ID { get; set; } = 0;
        public int DurationMs { get; set; } = 0;
        public int DelayMs { get; set; } = 0;
        public int FadeInTimeMs { get; set; } = 0; 
        public int FadeOutTimeMs { get; set; } = 0;
        public int StateSpellVisualKitID { get; set; } = 0;
        public int StartSpellVisualKitID { get; set; } = 0;
        public int OffsetMatrixID { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
