using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels.AnimKits
{
    public class AnimKitSegmentDto
    {
        public int? OrderIndex { get; set; }
        public int? AnimId { get; set; }
        public int? AnimStartTime { get; set; }
        public int? AnimKitConfigId { get; set; }
        public int? StartCondition { get; set; }
        public int? StartConditionParam { get; set; }
        public int? StartConditionDelay { get; set; }
        public int? EndCondition { get; set; }
        public int? EndConditionParam { get; set; }
        public int? EndConditionDelay { get; set; }
        public double? Speed { get; set; }
        public int? SegmentFlags { get; set; }
        public int? OverrideConfigFlags { get; set; }
        public int? BlendInTimeMs { get; set; }
        public int? BlendOutTimeMs { get; set; }
    }
}
