using HotfixMods.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKitSegment
    {
        public int ID { get; set; }
        public ushort ParentAnimKitID { get; set; }
        public byte OrderIndex { get; set; }
        public ushort AnimID { get; set; }
        public uint AnimStartTime { get; set; }
        public ushort AnimKitConfigID { get; set; }
        public byte StartCondition { get; set; }
        public byte StartConditionParam { get; set; }
        public uint StartConditionDelay { get; set; }
        public byte EndCondition { get; set; }
        public uint EndConditionParam { get; set; }
        public uint EndConditionDelay { get; set; }
        public decimal Speed { get; set; }
        public ushort SegmentFlags { get; set; }
        public byte ForcedVariation { get; set; }
        public int OverrideConfigFlags { get; set; }
        public sbyte LoopToSegmentIndex { get; set; }
        public ushort BlendInTimeMs { get; set; }
        public ushort BlendOutTimeMs { get; set; }
        public decimal Field_9_0_1_34278_018 { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
