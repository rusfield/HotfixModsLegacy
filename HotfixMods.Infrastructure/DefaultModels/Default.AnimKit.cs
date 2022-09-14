using HotfixMods.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DefaultModels
{
    public static partial class Default
    {
        public static readonly AnimKit AnimKit = new()
        {
            OneShotDuration = 0,
            OneShotStopAnimKitId = 0,

            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly AnimKitSegment AnimKitSegment = new()
        {
            AnimKitConfigId = 0,
            AnimStartTime = 0,
            BlendInTimeMs = 0,
            BlendOutTimeMs = 0, 
            EndCondition = 0,
            EndConditionDelay = 0,
            EndConditionParam = 0,
            ForcedVariation = 0,
            LoopToSegmentIndex = -1,
            OrderIndex = 0,
            OverrideConfigFlags = 0,
            SegmentFlags = 0,
            StartCondition = 0,
            StartConditionDelay = 0,
            StartConditionParam = 0,
            Speed = 1,

            ParentAnimKitId = -1,
            AnimId = -1,
            Id = -1,
            VerifiedBuild = -1,
        };
    }
}
