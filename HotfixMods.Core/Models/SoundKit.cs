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
        public int SoundType { get; set; } // Exists as enum in WowTools
        public double VolumeFloat { get; set; }
        public int Flags { get; set; } // Exists as flag in WowTools
        public int MinDistance { get; set; }
        public int DistanceCutoff { get; set; }
        public double VolumeVariationPlus { get; set; }
        public double VolumeVariationMinus { get; set; }
        public double PitchVariationPlus { get; set; }
        public double PitchVariationMinus { get; set; }
        public double PitchAdjust { get; set; }
    }
}
