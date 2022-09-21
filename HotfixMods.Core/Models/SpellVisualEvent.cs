using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SpellVisualEvent : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public SpellVisualEventTypes StartEvent { get; set; }
        public SpellVisualEventTypes EndEvent { get; set; }
        public int StartMinOffsetMs { get; set; }
        public int StartMaxOffsetMs { get; set; }
        public int EndMinOffsetMs { get; set; } 
        public int EndMaxOffsetMs { get; set; }
        public SpellVisualEventTargetType TargetType { get; set; }
        public int SpellVisualKitId { get; set; }
        public int SpellVisualId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
