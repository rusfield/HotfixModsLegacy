using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class GameObjectTemplate : IWorldSchema
    {
        public int Entry { get; set; }
        public GameObjectTypes Type { get; set; }
        public int DisplayId { get; set; }
        public string Name { get; set; }
        public string IconName { get; set; }
        public string CastBarCaption { get; set; }
        public string Unk1 { get; set; }        
        public decimal Size { get; set; }
        public string AiName { get; set; }
        public string ScriptName { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
