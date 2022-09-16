using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SpellPower : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public int ManaCost { get; set; } // Mana is mana, energy, rage, etc. based on Power Type
        public decimal PowerCostPct { get; set; }
        public SpellPowerType PowerType { get; set; }
        public int RequiredAuraSpellId { get; set; }
        public int SpellId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
