using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ItemEffect : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public ItemTriggerType TriggerType { get; set; }
        public int Charges { get; set; }
        public int CoolDownMSec { get; set; }
        public int CategoryCoolDownMSec { get; set; }
        public int SpellCategoryId { get; set; }
        public int SpellId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
