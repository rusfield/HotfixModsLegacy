using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SpellXSpellVisual : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public int SpellVisualId { get; set; }
        public int SpellId { get; set; }
        public int Probability { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
