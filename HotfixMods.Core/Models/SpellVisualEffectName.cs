using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SpellVisualEffectName : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public int ModelFileDataId { get; set; }
        public decimal Scale { get; set; }
        public decimal MinAllowedScale { get; set; }
        public decimal MaxAllowedScale { get; set; }
        public decimal Alpha { get; set; }
        public int TextureFileDataId { get; set; }
        public SpellVisualEffectNameType Type { get; set; }
        public int GenericId { get; set; } // Based on Type
        public int ModelPosition { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
