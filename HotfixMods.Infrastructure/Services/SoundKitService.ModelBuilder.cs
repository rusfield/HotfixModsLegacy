using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Core.Enums;
using HotfixMods.Infrastructure.DefaultModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SoundKitService
    {
        SoundKit BuildSoundKit(SoundKitDto soundKit)
        {
            soundKit.AddHotfix(soundKit.Id, TableHashes.SOUND_KIT, HotfixStatuses.VALID);
            return new SoundKit()
            {
                Id = soundKit.Id,

                DistanceCutoff = Default.SoundKit.DistanceCutoff,
                PitchAdjust = soundKit.PitchAdjust ?? Default.SoundKit.PitchAdjust,
                PitchVariationMinus = soundKit.PitchVariation ?? Default.SoundKit.PitchVariationMinus,
                PitchVariationPlus = soundKit.PitchVariation ?? Default.SoundKit.PitchVariationPlus,
                VolumeVariationMinus = soundKit.VolumeVariation ?? Default.SoundKit.VolumeVariationMinus,
                VolumeVariationPlus = soundKit.VolumeVariation ?? Default.SoundKit.VolumeVariationPlus,
                VolumeFloat = soundKit.VolumeAdjust ?? Default.SoundKit.VolumeFloat,
                SoundType = soundKit.SoundType ?? Default.SoundKit.SoundType,

                MinDistance = Default.SoundKit.MinDistance,
                Flags = Default.SoundKit.Flags
            };
        }

        SoundKitEntry[] BuildSoundKitEntry(SoundKitDto soundKit)
        {
            var result = new List<SoundKitEntry>();
            var id = soundKit.Id;
            foreach(var fileDataId in soundKit.FileDataIds)
            {
                soundKit.AddHotfix(id, TableHashes.SOUND_KIT_ENTRY, HotfixStatuses.VALID);
                result.Add(new SoundKitEntry()
                {
                    Id = id,
                    FileDataId = fileDataId,
                    SoundKitId = soundKit.Id,
                    
                    Frequency = Default.SoundKitEntry.Frequency,
                    Volume = Default.SoundKitEntry.Volume
                });
                id++;
            }
            return result.ToArray();
        }
    }
}
