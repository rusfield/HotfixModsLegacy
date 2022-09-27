using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Core.Enums;
using HotfixMods.Infrastructure.DefaultModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellVisualKitService
    {
        SpellVisualKit BuildSpellVisualKit(SpellVisualKitDto dto)
        {
            dto.AddHotfix(dto.Id, TableHashes.SPELL_VISUAL_KIT, HotfixStatuses.VALID);
            return new()
            {
                Id = dto.Id,
                VerifiedBuild = VerifiedBuild,
                
                ClutterLevel = Default.SpellVisualKit.ClutterLevel,
                FallbackSpellVisualKitId = Default.SpellVisualKit.FallbackSpellVisualKitId,
                DelayMin = Default.SpellVisualKit.DelayMin,
                DelayMax = Default.SpellVisualKit.DelayMax,
                Flags0 = Default.SpellVisualKit.Flags0,
                Flags1 = Default.SpellVisualKit.Flags1
            };
        }

        SpellVisualKitEffect BuildSpellVisualKitEffect(SpellVisualKitDto dto)
        {
            dto.AddHotfix(dto.Id, TableHashes.SPELL_VISUAL_KIT_EFFECT, HotfixStatuses.VALID);
            return new()
            {
                ParentSpellVisualKitId = dto.Id,
                Id = dto.Id,
                VerifiedBuild = VerifiedBuild,
                Effect = dto.Id,

                EffectType = dto.EffectType ?? Default.SpellVisualKitEffect.EffectType
            };
        }

        SpellVisualKitModelAttach BuildSpellVisualKitModelAttach(SpellVisualKitDto dto)
        {
            dto.AddHotfix(dto.Id, TableHashes.SPELL_VISUAL_KIT_MODEL_ATTACH, HotfixStatuses.VALID);
            return new()
            {
                ParentSpellVisualKitId = dto.Id,
                SpellVisualEffectNameId = dto.Id,
                Id = dto.Id,
                VerifiedBuild = VerifiedBuild,

                Scale = Default.SpellVisualKitModelAttach.Scale,
                YawVariation = Default.SpellVisualKitModelAttach.YawVariation,
                AnimId = Default.SpellVisualKitModelAttach.AnimId,
                AnimKitId = Default.SpellVisualKitModelAttach.AnimKitId,
                EndAnimId = Default.SpellVisualKitModelAttach.EndAnimId,
                AttachmentId = Default.SpellVisualKitModelAttach.AttachmentId,
                Field_9_0_1_33978_021 = Default.SpellVisualKitModelAttach.Field_9_0_1_33978_021,
                Flags = Default.SpellVisualKitModelAttach.Flags,
                LowDefModelAttachId = Default.SpellVisualKitModelAttach.LowDefModelAttachId,
                Offset0 = Default.SpellVisualKitModelAttach.Offset0,
                Offset1 = Default.SpellVisualKitModelAttach.Offset1,
                Offset2 = Default.SpellVisualKitModelAttach.Offset2,
                OffsetVariation0 = Default.SpellVisualKitModelAttach.OffsetVariation0,
                OffsetVariation1 = Default.SpellVisualKitModelAttach.OffsetVariation1,
                OffsetVariation2 = Default.SpellVisualKitModelAttach.OffsetVariation2,
                Pitch = Default.SpellVisualKitModelAttach.Pitch,
                PitchVariation = Default.SpellVisualKitModelAttach.PitchVariation,
                PositionerId = Default.SpellVisualKitModelAttach.PositionerId,
                Roll = Default.SpellVisualKitModelAttach.Roll,
                RollVariation = Default.SpellVisualKitModelAttach.RollVariation,
                ScaleVariation = Default.SpellVisualKitModelAttach.ScaleVariation,
                StartAnimId = Default.SpellVisualKitModelAttach.StartAnimId,
                StartDelay = Default.SpellVisualKitModelAttach.StartDelay,
                Yaw = Default.SpellVisualKitModelAttach.Yaw
            };
        }

        SpellVisualEffectName BuildSpellVisualEffectName(SpellVisualKitDto dto)
        {
            dto.AddHotfix(dto.Id, TableHashes.SPELL_VISUAL_EFFECT_NAME, HotfixStatuses.VALID);
            return new()
            {
                Id = dto.Id,
                VerifiedBuild = VerifiedBuild,

                Alpha = dto.Alpha ?? Default.SpellVisualEffectName.Alpha,
                GenericId = dto.GenericId ?? Default.SpellVisualEffectName.GenericId,
                MaxAllowedScale = dto.MaxAllowedScale ?? Default.SpellVisualEffectName.MaxAllowedScale,
                MinAllowedScale = dto.MinAllowedScale ?? Default.SpellVisualEffectName.MinAllowedScale,
                ModelFileDataId = dto.ModelFileDataId ?? Default.SpellVisualEffectName.ModelFileDataId,
                ModelPosition = dto.ModelPosition ?? Default.SpellVisualEffectName.ModelPosition,
                Scale = dto.Scale ?? Default.SpellVisualEffectName.Scale,
                TextureFileDataId = dto.TextureFileDataId ?? Default.SpellVisualEffectName.TextureFileDataId,
                Type = dto.Type ?? Default.SpellVisualEffectName.Type,

                BaseMissileSpeed = Default.SpellVisualEffectName.BaseMissileSpeed,
                DissolveEffectId = Default.SpellVisualEffectName.DissolveEffectId,
                EffectRadius = Default.SpellVisualEffectName.EffectRadius,
                Field_9_1_0_38549_014 = Default.SpellVisualEffectName.Field_9_1_0_38549_014,
                Flags = Default.SpellVisualEffectName.Flags,
                RibbonQualityId = Default.SpellVisualEffectName.RibbonQualityId
            };
        }
    }
}
