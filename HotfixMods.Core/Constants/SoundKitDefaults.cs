using HotfixMods.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Constants
{
    public abstract class SoundKitDefaults
    {
        public const decimal DistanceCutoff = 40;
        public const decimal VolumeFloat = 1;
        public const int MinDistance = 6;
        public const decimal VolumeVariationPlus = 0.1M;
        public const decimal VolumeVariationMinus = 0.1M;
        public const decimal PitchVariationPlus = 0.1M;
        public const decimal PitchVariationMinus = 0.1M;
        public const decimal PitchAdjust = 0;
        public const SoundKitSoundTypes SoundType = SoundKitSoundTypes.NPC;
        public const int Flags = 0;
        public const int Frequency = 1;
        public const decimal Volume = 1;
    }
}
