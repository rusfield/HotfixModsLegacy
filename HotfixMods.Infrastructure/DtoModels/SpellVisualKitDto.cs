using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
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

        // SpellVisualEffectName (used for SpellVisualModelAttach)
        public int? ModelFileDataId { get; set; }
        public SpellVisualKitModelAttachAttachments? AttachmentId { get; set; }
        public decimal? Scale { get; set; }
        public decimal? Alpha { get; set; }
        public int? TextureFileDataId { get; set; }
        public SpellVisualEffectNameType? Type { get; set; }
        public int? GenericId { get; set; } // Based on Type
        public int? ModelPosition { get; set; }
        public SpellVisualKitFlags0? Flags0 { get; set; }
        public SpellVisualKitFlags1? Flags1 { get; set; }
        public int? ClutterLevel { get; set; }
        public int? DelayMax { get; set; }
        public int? DelayMin { get; set; }
        public int? FallbackSpellVisualKitId { get; set; }

    }
}
