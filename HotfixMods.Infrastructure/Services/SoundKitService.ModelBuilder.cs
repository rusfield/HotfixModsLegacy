using HotfixMods.Core.Constants;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SoundKitService
    {
        SoundKit BuildSoundKit(SoundKitDto soundKit)
        {
            soundKit.AddHotfix(soundKit.Id, TableHashes.SoundKit, HotfixStatuses.VALID);
            return new SoundKit()
            {
                Id = soundKit.Id,

                DistanceCutoff = SoundKitDefaults.DistanceCutoff,
                PitchAdjust = soundKit.PitchAdjust ?? SoundKitDefaults.PitchAdjust,
                PitchVariationMinus = soundKit.PitchVariation ?? SoundKitDefaults.PitchVariationMinus,
                PitchVariationPlus = soundKit.PitchVariation ?? SoundKitDefaults.PitchVariationPlus,
                VolumeVariationMinus = soundKit.VolumeVariation ?? SoundKitDefaults.VolumeVariationMinus,
                VolumeVariationPlus = soundKit.VolumeVariation ?? SoundKitDefaults.VolumeVariationPlus,
                VolumeFloat = soundKit.VolumeAdjust ?? SoundKitDefaults.VolumeFloat,
                SoundType = soundKit.SoundType ?? SoundKitDefaults.SoundType,

                MinDistance = SoundKitDefaults.MinDistance,
                Flags = SoundKitDefaults.Flags
            };
        }

        SoundKitEntry[] BuildSoundKitEntry(SoundKitDto soundKit)
        {
            var result = new List<SoundKitEntry>();
            var id = soundKit.Id;
            foreach(var fileDataId in soundKit.FileDataIds)
            {
                soundKit.AddHotfix(id, TableHashes.SoundKitEntry, HotfixStatuses.VALID);
                result.Add(new SoundKitEntry()
                {
                    Id = id,
                    FileDataId = fileDataId,
                    SoundKitId = soundKit.Id,
                    
                    Frequency = SoundKitDefaults.Frequency,
                    Volume = SoundKitDefaults.Volume
                });
                id++;
            }
            return result.ToArray();
        }
    }
}
