﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class SoundKitDto : Dto
    {
        public double? PitchVariationPlus { get; set; }
        public double? PitchVariationMinus { get; set; }
        public double? VolumeVariationMinus { get; set; }
        public double? VolumeVariationPlus { get; set; }
        public double? PitchAdjust { get; set; }
        public List<int> FileDataIds { get; set; }
    }
}
