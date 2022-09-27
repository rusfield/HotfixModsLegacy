using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SpellVisual : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        // TODO: Investigate properties
        public int MissileAttachment { get; set; }
        public int MissileDestinationAttachment { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
