using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class CreatureTemplateScaling
    {
        
        public uint Entry { get; set; } = 0;
        public byte DifficultyId { get; set; } = 0;
        public short LevelScalingDeltaMin { get; set; } = 0;
        public short LevelScalingDeltaMax { get; set; } = 0;
        public int ContentTuningId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
