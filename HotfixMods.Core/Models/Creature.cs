using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class Creature : IWorldSchema
    {
        public int Guid { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
