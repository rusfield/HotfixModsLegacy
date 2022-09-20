using HotfixMods.Core.Flags;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public DamageClassFlags SchoolMask { get; set; }
        [Column("Attributes1")]
        public SpellAttributeFlags0 Attributes0 { get; set; }
        [Column("Attributes2")]
        public SpellAttributeFlags1 Attributes1 { get; set; }
        [Column("Attributes3")]
        public SpellAttributeFlags2 Attributes2 { get; set; }
        [Column("Attributes4")]
        public SpellAttributeFlags3 Attributes3 { get; set; }
        [Column("Attributes5")]
        public SpellAttributeFlags4 Attributes4 { get; set; }
        [Column("Attributes6")]
        public SpellAttributeFlags5 Attributes5 { get; set; }
        [Column("Attributes7")]
        public SpellAttributeFlags6 Attributes6 { get; set; }
        [Column("Attributes8")]
        public SpellAttributeFlags7 Attributes7 { get; set; }
        [Column("Attributes9")]
        public SpellAttributeFlags8 Attributes8 { get; set; }
        [Column("Attributes10")]
        public SpellAttributeFlags9 Attributes9 { get; set; }
        [Column("Attributes11")]
        public SpellAttributeFlags10 Attributes10 { get; set; }
        [Column("Attributes12")]
        public SpellAttributeFlags11 Attributes11 { get; set; }
        [Column("Attributes13")]
        public SpellAttributeFlags12 Attributes12 { get; set; }
        [Column("Attributes14")]
        public SpellAttributeFlags13 Attributes13 { get; set; }
        [Column("Attributes15")]
        public SpellAttributeFlags14 Attributes14 { get; set; }
        public decimal Speed { get; set; }
        public int SpellIconFileDataId { get; set; }
        public int SpellId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
