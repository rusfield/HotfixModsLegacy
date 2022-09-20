using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class Spell : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public string NameSubtext { get; set; }
        public string Description { get; set; }
        public string AuraDescription { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
