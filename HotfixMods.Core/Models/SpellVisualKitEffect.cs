using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SpellVisualKitEffect : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public SpellVisualKitEffectType EffectType { get; set; }
        public int Effect { get; set; } // based on EffectType
        public int ParentSpellVisualKitId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
