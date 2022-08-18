using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class AnimKit : IHotfixesSchema, IDb2
    {
        public int Id { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
