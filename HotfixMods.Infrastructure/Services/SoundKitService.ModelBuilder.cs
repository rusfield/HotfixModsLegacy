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
        public SoundKit BuildSoundKit(SoundKitDto soundKit)
        {
            soundKit.AddHotfix(soundKit.Id, TableHashes.SoundKit, HotfixStatuses.VALID);
            return new SoundKit()
            {
                Id = soundKit.Id,

                DistanceCutoff = SoundDefaults.DistanceCutoff,
                PitchAdjust = soundKit.PitchAdjust ?? SoundDefaults.PitchAdjust,
                PitchVariationMinus = soundKit.PitchVariation ?? SoundDefaults.PitchVariationMinus,
                PitchVariationPlus = soundKit.PitchVariation ?? SoundDefaults.PitchVariationPlus,
                VolumeVariationMinus = soundKit.VolumeVariation ?? SoundDefaults.VolumeVariationMinus,
                VolumeVariationPlus = soundKit.VolumeVariation ?? SoundDefaults.VolumeVariationPlus,
                VolumeFloat = soundKit.VolumeAdjust ?? SoundDefaults.VolumeFloat,
                SoundType = soundKit.SoundType ?? SoundDefaults.SoundType,

                MinDistance = SoundDefaults.MinDistance,
                Flags = SoundDefaults.Flags
            };
        }

        public List<SoundKitEntry> BuildSoundKitEntry(SoundKitDto soundKit)
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
                    
                    Frequency = SoundDefaults.Frequency,
                    Volume = SoundDefaults.Volume
                });
                id++;
            }
            return result;
        }
    }
}
