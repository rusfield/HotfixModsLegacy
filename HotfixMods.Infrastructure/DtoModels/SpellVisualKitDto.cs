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
        public SpellVisualKitFlags0? Flags0 { get; set; }
        public SpellVisualKitFlags1? Flags1 { get; set; }
        public int? DelayMax { get; set; }
        public int? DelayMin { get; set; }
        public int? ClutterLevel { get; set; }
        public int? FallbackSpellVisualKitId { get; set; }


        // SpellVisualEffectName (used for SpellVisualModelAttach)
        public int? ModelFileDataId { get; set; }
        public decimal? Scale { get; set; }
        public decimal? Alpha { get; set; }
        public int? TextureFileDataId { get; set; }
        public SpellVisualEffectNameType? Type { get; set; }
        public int? GenericId { get; set; } // Based on Type
        public decimal? BaseMissileSpeed { get; set; }
        public int? RibbonQualityId { get; set; }
        public int? DissolveEffectId { get; set; }
        public decimal? EffectRadius { get; set; }
        public SpellVisualEffectNameFlags? SpellVisualEffectNameFlags { get; set; }

        // SpellVisualModelAttach
        public SpellVisualKitModelAttachAttachments? AttachmentId { get; set; }
        public decimal? Offset0 { get; set; }
        public decimal? Offset1 { get; set; }
        public decimal? Offset2 { get; set; }
        public decimal? Roll { get; set; } // https://howthingsfly.si.edu/flight-dynamics/roll-pitch-and-yaw
        public decimal? Pitch { get; set; }
        public decimal? Yaw { get; set; }
        public decimal? StartDelay { get; set; } // Can be negative
        public SpellVisualKitModelAttachFlags? ModelAttachFlags { get; set; }
        public int? PositionerId { get; set; }
        public int? StartAnimId { get; set; }
        public int? AnimId { get; set; }
        public int? EndAnimId { get; set; }
        public int? AnimKitId { get; set; }

    }
}
