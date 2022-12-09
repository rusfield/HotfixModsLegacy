using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SoundKit
    {
        public int Id { get; set; }
        public int SoundType { get; set; }
        public decimal VolumeFloat { get; set; }
        public int Flags { get; set; }
        public decimal MinDistance { get; set; }
        public decimal DistanceCutoff { get; set; }
        public byte EAXDef { get; set; }
        public uint SoundKitAdvancedId { get; set; }
        public decimal VolumeVariationPlus { get; set; }
        public decimal VolumeVariationMinus { get; set; }
        public decimal PitchVariationPlus { get; set; }
        public decimal PitchVariationMinus { get; set; }
        public sbyte DialogType { get; set; }
        public decimal PitchAdjust { get; set; }
        public ushort BusOverwriteId { get; set; }
        public byte MaxInstances { get; set; }
        public uint SoundMixGroupId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
