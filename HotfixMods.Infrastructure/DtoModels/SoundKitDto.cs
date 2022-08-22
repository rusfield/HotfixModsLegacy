using HotfixMods.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class SoundKitDto : Dto
    {
        public SoundKitSoundTypes? SoundType { get; set; }
        public double? PitchVariation { get; set; }
        public double? VolumeVariation { get; set; }
        public double? PitchAdjust { get; set; }
        public double? VolumeAdjust { get; set; }
        public List<int> FileDataIds { get; set; }
    }
}
