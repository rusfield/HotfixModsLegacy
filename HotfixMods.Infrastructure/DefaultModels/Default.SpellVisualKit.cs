using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Core.Flags;

namespace HotfixMods.Infrastructure.DefaultModels
{
    public partial class Default
    {
        public static readonly SpellVisualKit SpellVisualKit = new()
        {

            // Temp
            Flags0 = SpellVisualKitFlags0.NONE,
            Flags1 = SpellVisualKitFlags1.NONE,
            ClutterLevel = 0,
            DelayMax = 0,
            DelayMin = 0,
            FallbackSpellVisualKitId = 0,

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
            Scale = 1,

            // Temp
            AnimId = 0,
            AnimKitId = 0,
            EndAnimId = -1,
            StartAnimId = -1,
            AttachmentId = SpellVisualKitModelAttachAttachments.MAIN_HAND,
            Field_9_0_1_33978_021 = 1,
            Flags = 0,
            LowDefModelAttachId = 0,
            Offset0 = 0,
            Offset1 = 0,
            Offset2 = 0,
            OffsetVariation0 = 0,
            OffsetVariation1 = 0,
            OffsetVariation2 = 0,
            Pitch = 0,
            PitchVariation = 0,
            PositionerId = 0,
            Roll = 0,
            RollVariation = 0,
            ScaleVariation = 0,
            StartDelay = 0,
            Yaw = 0,
            YawVariation = 0,

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

            // Temp
            Flags = 67108864,
            BaseMissileSpeed = 0,
            DissolveEffectId = 0,
            EffectRadius = 0,
            Field_9_1_0_38549_014 = 0,
            RibbonQualityId = 0,

            Id = -1,
            VerifiedBuild = -1
        };
    }
}
