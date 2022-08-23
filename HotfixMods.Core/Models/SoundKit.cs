using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SoundKit : IHotfixesSchema, IDb2
    {
        public int Id { get; set; }
        public SoundKitSoundTypes SoundType { get; set; }
        public decimal VolumeFloat { get; set; }
        public int Flags { get; set; } // Exists as flag in WowTools
        public decimal MinDistance { get; set; }
        public decimal DistanceCutoff { get; set; }
        public decimal VolumeVariationPlus { get; set; }
        public decimal VolumeVariationMinus { get; set; }
        public decimal PitchVariationPlus { get; set; }
        public decimal PitchVariationMinus { get; set; }
        public decimal PitchAdjust { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
