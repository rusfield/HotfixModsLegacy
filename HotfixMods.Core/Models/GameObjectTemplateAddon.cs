using HotfixMods.Core.Flags;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class GameObjectTemplateAddon : IWorldSchema
    {
        public int Entry { get; set; }
        public GameObjectAddonFlags Flags { get; set; }
        public int Faction { get; set; }

        //public int VerifiedBuild { get; set; } // Currently not implemented in TC 
    }
}
