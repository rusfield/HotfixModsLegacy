using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class AnimKitSegment : IHotfixesSchema, IDb2
    {
        public int Id { get; set; }
        public int ParentAnimKitId { get; set; }
        public int OrderIndex { get; set; }
        public int AnimId { get; set; }
        public int AnimStartTime { get; set; }
        public int AnimKitConfigId { get; set; }
        public int StartCondition { get; set; }
        public int StartConditionParam { get; set; }
        public int StartConditionDelay { get; set; }
        public int EndCondition { get; set; }
        public int EndConditionParam { get; set; }
        public int EndConditionDelay { get; set; }
        public double Speed { get; set; }
        public int SegmentFlags { get; set; }
        public int ForcedVariation { get; set; }
        public int OverrideConfigFlags { get; set; }
        public int BlendInTimeMs { get; set; }
        public int BlendOutTimeMs { get; set; }
        public int LoopToSegmentIndex { get; set; }
        // public double Unknown_column { get; set; } // Just default to 0
        public int VerifiedBuild { get; set; }
    }
}
