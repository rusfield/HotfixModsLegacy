using HotfixMods.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class SpellVisualKitDto : Dto
    {
        public SpellVisualKitEffectType? EffectType { get; set; }
        public int? Effect { get; set; }


        // SpellVisualEffectName (used for SpellVisualModelAttach)
        public int? ModelFileDataId { get; set; }
        public decimal? Scale { get; set; }
        public decimal? MinAllowedScale { get; set; }
        public decimal? MaxAllowedScale { get; set; }
        public decimal? Alpha { get; set; }
        public int? TextureFileDataId { get; set; }
        public SpellVisualEffectNameType? Type { get; set; }
        public int? GenericId { get; set; } // Based on Type
        public int? ModelPosition { get; set; }
    }
}
