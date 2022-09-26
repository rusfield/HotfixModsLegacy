using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DefaultModels
{
    public partial class Default
    {
        public static readonly SpellVisualKit SpellVisualKit = new()
        {

            // Nothing yet

            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly SpellVisualKitEffect SpellVisualKitEffect = new()
        {
            Effect = 2,
            EffectType = SpellVisualKitEffectType.SPELL_PROCEDURAL_EFFECT,

            Id = -1,
            VerifiedBuild = -1,
            ParentSpellVisualKitId = -1
        };

        public static readonly SpellVisualKitModelAttach SpellVisualKitModelAttach = new()
        {
            ParentSpellVisualKitId = -1,
            SpellVisualEffectNameId = -1,
            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly SpellVisualEffectName SpellVisualEffectName = new()
        {
            ModelFileDataId = 0,
            Alpha = 1,
            Scale = 1,
            GenericId = 0,
            MaxAllowedScale = 100,
            MinAllowedScale = 0.01M,
            ModelPosition = -1,
            TextureFileDataId = 0,
            Type = SpellVisualEffectNameType.FILE_DATA_ID,

            Id = -1,
            VerifiedBuild = -1
        };
    }
}
