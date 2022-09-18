using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ItemXItemEffect : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public int ItemEffectId { get; set; }
        public int ItemId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
