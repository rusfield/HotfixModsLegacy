using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ItemInstanceTransmog : ICharactersSchema
    {
        public int ItemGuid { get; set; }
        public int ItemModifiedAppearanceAllSpecs { get; set; }
        public int SpellItemEnchantmentAllSpecs { get; set; }
    }
}
