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
                PitchVariationMinus = soundKit.PitchVariationMinus ?? SoundDefaults.PitchVariationMinus,
                PitchVariationPlus = soundKit.PitchVariationPlus ?? SoundDefaults.PitchVariationPlus,
                VolumeVariationMinus = soundKit.VolumeVariationMinus ?? SoundDefaults.VolumeVariationMinus,
                VolumeVariationPlus = soundKit.VolumeVariationPlus ?? SoundDefaults.VolumeVariationPlus,

                MinDistance = SoundDefaults.MinDistance,
                VolumeFloat = SoundDefaults.VolumeFloat,
                SoundType = SoundDefaults.SoundType,
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

        public HotfixModsData BuildHotfixModsData(SoundKitDto soundKit, HotfixModsData hotfixModsData = null)
        {
            hotfixModsData = new HotfixModsData()
            {
                Id = soundKit.Id,
                Name = soundKit.HotfixModsName,
                Comment = soundKit.HotfixModsComment,
                RecordId = soundKit.Id,
                VerifiedBuild = VerifiedBuild
            };
            return hotfixModsData;
        }

    }
}
