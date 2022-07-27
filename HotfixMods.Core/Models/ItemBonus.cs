using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ItemBonus : IDb2
    {
        public int Id { get; set; }
        public int Value0 { get; set; }
        public int ParentItemBonusListId { get; set; }
        public int Type { get; set; } // Type 7 is used for Item Modifiers
    }
}
