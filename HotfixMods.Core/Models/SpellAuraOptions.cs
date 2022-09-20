using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SpellAuraOptions : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public int CumulativeAura { get; set; } // stacks
        public int ProcCategoryRecovery { get; set; }
        public int ProcChance { get; set; }
        public int ProcCharges { get; set; }
        public int SpellProcsPerMinuteId { get; set; }
        public int ProcTypeMask0 { get; set; }
        public int ProcTypeMask1 { get; set; }
        public int SpellId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
