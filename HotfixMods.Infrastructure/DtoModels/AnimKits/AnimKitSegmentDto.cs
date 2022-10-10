using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels.AnimKits
{
    // StartCondition 0: play animation as normal
    // Params: seems to do nothing
    // Delay: ms delay before start play

    // StartCondition 1:UNKNOWN
    // Nothing happens. Probably related to OrderIndex

    // StartCondition 2: UNKNOWN
    // NOthing happens. Probably related to OrderIndex




    // EndCondition 0: loop animation X times
    // Params: X (0 and 1 plays animation 1 time, 2 plays animation 2 times, etc)
    // Delay: Wait/Freeze on last frame (in ms)

    // EndCondition 1: repeat animation infinitely
    // Params/Delay: seems to do nothing

    // EndCondition 2: Play animation one time for X ms
    // Params: seems to do nothing
    // Delay: X ms for when to stop animation (stops early if ms is before animation stops, freezes at end if animation is longer, ends instantly if 0)

    // EndCondition 3: UNKNNOWN
    // It seems to do the same as EndCondition2. Maybe it has to do with OrderIndex.

    // EndCondition 4: UNKNOWN
    // It seems to do the same as EndCondition2. Maybe it has to do with OrderIndex.

    // EndCondition 5: Freeze animation on last frame (can perform other animations while in position of last frame)
    // Params/Delay: Unknown



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
        public decimal? Speed { get; set; }
        public int? SegmentFlags { get; set; }
        public int? OverrideConfigFlags { get; set; }
        public int? BlendInTimeMs { get; set; }
        public int? BlendOutTimeMs { get; set; }
    }
}
