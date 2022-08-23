using HotfixMods.Infrastructure.DtoModels.AnimKits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class AnimKitDto : Dto
    {
        public int OneShotDuration { get; set; }
        public int OneShotStopAnimKitId { get; set; }
        public List<AnimKitSegmentDto> Segments { get; set; }
    }
}
