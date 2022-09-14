using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Defaults
{
    public static partial class Default
    {
        public static readonly SoundKit SoundKit = new()
        {
            DistanceCutoff = 40,
            Flags = 0,
            MinDistance = 6,
            PitchAdjust = 0,
            PitchVariationMinus = 0.1M,
            PitchVariationPlus = 0.1M,
            VolumeVariationMinus = 0.1M,
            VolumeVariationPlus = 0.1M,
            SoundType = SoundKitSoundTypes.NPC,
            VolumeFloat = 1,

            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly SoundKitEntry SoundKitEntry = new()
        {
            Frequency = 1,
            Volume = 1,

            SoundKitId = -1,
            Id = -1,
            FileDataId = -1,
            VerifiedBuild = -1
        };
    }
}
