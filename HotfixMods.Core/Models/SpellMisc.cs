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
        public SpellAttributes0 Attributes0 { get; set; }
        // TODO: Lots more attributes

        public int Speed { get; set; }
        public int SpellIconFileDataId { get; set; }
        public int SpellId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
