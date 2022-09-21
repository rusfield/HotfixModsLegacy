using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SpellVisualKitModelAttach : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }


        // TODO: Check properties!!


        public int SpellVisualEffectNameId { get; set; }
        public int ParentSpellVisualKitId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
