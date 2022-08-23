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
        public decimal? PitchVariation { get; set; }
        public decimal? VolumeVariation { get; set; }
        public decimal? PitchAdjust { get; set; }
        public decimal? VolumeAdjust { get; set; }
        public List<int> FileDataIds { get; set; }
    }
}
