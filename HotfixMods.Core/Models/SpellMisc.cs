using HotfixMods.Core.Flags;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SpellMisc : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public int CastingTimeIndex { get; set; }
        public int DurationIndex { get; set; }
        public int RangeIndex { get; set; }
        public DamageClass SchoolMask { get; set; }
        public SpellAttributeFlags0 Attributes0 { get; set; }
        public SpellAttributeFlags1 Attributes1 { get; set; }
        public SpellAttributeFlags2 Attributes2 { get; set; }
        public SpellAttributeFlags3 Attributes3 { get; set; }
        public SpellAttributeFlags4 Attributes4 { get; set; }
        public SpellAttributeFlags5 Attributes5 { get; set; }
        public SpellAttributeFlags6 Attributes6 { get; set; }
        public SpellAttributeFlags7 Attributes7 { get; set; }
        public SpellAttributeFlags8 Attributes8 { get; set; }
        public SpellAttributeFlags9 Attributes9 { get; set; }
        public SpellAttributeFlags10 Attributes10 { get; set; }
        public SpellAttributeFlags11 Attributes11 { get; set; }
        public SpellAttributeFlags12 Attributes12 { get; set; }
        public SpellAttributeFlags13 Attributes13 { get; set; }
        public SpellAttributeFlags14 Attributes14 { get; set; }
        public int Speed { get; set; }
        public int SpellIconFileDataId { get; set; }
        public int SpellId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
