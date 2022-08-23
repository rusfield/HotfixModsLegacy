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
        public const int DistanceCutoff = 40;
        public const double VolumeFloat = 1;
        public const int MinDistance = 6;
        public const double VolumeVariationPlus = 0.1;
        public const double VolumeVariationMinus = 0.1;
        public const double PitchVariationPlus = 0.1;
        public const double PitchVariationMinus = 0.1;
        public const double PitchAdjust = 0;
        public const SoundKitSoundTypes SoundType = SoundKitSoundTypes.NPC;
        public const int Flags = 0;
        public const int Frequency = 1;
        public const double Volume = 1;
    }
}
