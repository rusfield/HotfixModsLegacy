using HotfixMods.Core.Constants;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SoundService
    {
        public SoundKit BuildSoundKit(SoundDto sound)
        {
            return new SoundKit()
            {
                Id = sound.Id,

                DistanceCutoff = SoundDefaults.DistanceCutoff,
                PitchAdjust = sound.PitchAdjust ?? SoundDefaults.PitchAdjust,
                PitchVariationMinus = sound.PitchVariationMinus ?? SoundDefaults.PitchVariationMinus,
                PitchVariationPlus = sound.PitchVariationPlus ?? SoundDefaults.PitchVariationPlus,
                VolumeVariationMinus = sound.VolumeVariationMinus ?? SoundDefaults.VolumeVariationMinus,
                VolumeVariationPlus = sound.VolumeVariationPlus ?? SoundDefaults.VolumeVariationPlus,

                MinDistance = SoundDefaults.MinDistance,
                VolumeFloat = SoundDefaults.VolumeFloat,
                SoundType = SoundDefaults.SoundType,
                Flags = SoundDefaults.Flags
            };
        }

        public List<SoundKitEntry> BuildSoundKitEntry(SoundDto sound)
        {
            var result = new List<SoundKitEntry>();
            var id = sound.Id;
            foreach(var fileDataId in sound.FileDataIds)
            {
                result.Add(new SoundKitEntry()
                {
                    Id = id++,
                    FileDataId = fileDataId,
                    SoundKitId = sound.Id,
                    
                    Frequency = SoundDefaults.Frequency,
                    Volume = SoundDefaults.Volume
                });
            }
            return result;
        }

    }
}
