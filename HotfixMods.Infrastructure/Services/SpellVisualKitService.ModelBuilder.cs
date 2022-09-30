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
                
                ClutterLevel = dto.ClutterLevel ?? Default.SpellVisualKit.ClutterLevel,
                FallbackSpellVisualKitId = dto.FallbackSpellVisualKitId ?? Default.SpellVisualKit.FallbackSpellVisualKitId,
                DelayMin = dto.DelayMin ?? Default.SpellVisualKit.DelayMin,
                DelayMax = dto.DelayMax ?? Default.SpellVisualKit.DelayMax,
                Flags0 = dto.Flags0 ?? Default.SpellVisualKit.Flags0,
                Flags1 = dto.Flags1 ?? Default.SpellVisualKit.Flags1
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

                AttachmentId = dto.AttachmentId ?? Default.SpellVisualKitModelAttach.AttachmentId,
                AnimId = dto.AnimId ?? Default.SpellVisualKitModelAttach.AnimId,
                AnimKitId = dto.AnimKitId ?? Default.SpellVisualKitModelAttach.AnimKitId,
                EndAnimId = dto.EndAnimId ?? Default.SpellVisualKitModelAttach.EndAnimId,
                Flags = dto.ModelAttachFlags ?? Default.SpellVisualKitModelAttach.Flags,
                Offset0 = dto.Offset0 ?? Default.SpellVisualKitModelAttach.Offset0,
                Offset1 = dto.Offset1 ?? Default.SpellVisualKitModelAttach.Offset1,
                Offset2 = dto.Offset2 ?? Default.SpellVisualKitModelAttach.Offset2,
                Pitch = dto.Pitch ?? Default.SpellVisualKitModelAttach.Pitch,
                PositionerId = dto.PositionerId ?? Default.SpellVisualKitModelAttach.PositionerId,
                Roll = dto.Roll ?? Default.SpellVisualKitModelAttach.Roll,
                StartAnimId = dto.StartAnimId ?? Default.SpellVisualKitModelAttach.StartAnimId,
                StartDelay = dto.StartDelay ?? Default.SpellVisualKitModelAttach.StartDelay,
                Yaw = dto.Yaw ?? Default.SpellVisualKitModelAttach.Yaw,

                LowDefModelAttachId = Default.SpellVisualKitModelAttach.LowDefModelAttachId,
                Field_9_0_1_33978_021 = Default.SpellVisualKitModelAttach.Field_9_0_1_33978_021,
                OffsetVariation0 = Default.SpellVisualKitModelAttach.OffsetVariation0,
                OffsetVariation1 = Default.SpellVisualKitModelAttach.OffsetVariation1,
                OffsetVariation2 = Default.SpellVisualKitModelAttach.OffsetVariation2,
                PitchVariation = Default.SpellVisualKitModelAttach.PitchVariation,
                RollVariation = Default.SpellVisualKitModelAttach.RollVariation,
                ScaleVariation = Default.SpellVisualKitModelAttach.ScaleVariation,
                YawVariation = Default.SpellVisualKitModelAttach.YawVariation,
                Scale = Default.SpellVisualKitModelAttach.Scale // Scale is set on SpellVisualEffectName
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
                MaxAllowedScale = dto.Scale != null ? (decimal)dto.Scale * 100 : Default.SpellVisualEffectName.MaxAllowedScale,
                MinAllowedScale = dto.Scale != null ? (decimal)dto.Scale / 100 : Default.SpellVisualEffectName.MinAllowedScale,
                ModelFileDataId = dto.ModelFileDataId ?? Default.SpellVisualEffectName.ModelFileDataId,
                Scale = dto.Scale ?? Default.SpellVisualEffectName.Scale,
                TextureFileDataId = dto.TextureFileDataId ?? Default.SpellVisualEffectName.TextureFileDataId,
                Type = dto.Type ?? Default.SpellVisualEffectName.Type,
                BaseMissileSpeed = dto.BaseMissileSpeed ?? Default.SpellVisualEffectName.BaseMissileSpeed,
                DissolveEffectId = dto.DissolveEffectId ?? Default.SpellVisualEffectName.DissolveEffectId,
                EffectRadius = dto.EffectRadius ?? Default.SpellVisualEffectName.EffectRadius,
                Flags = dto.SpellVisualEffectNameFlags ?? Default.SpellVisualEffectName.Flags,
                RibbonQualityId = dto.RibbonQualityId ?? Default.SpellVisualEffectName.RibbonQualityId,

                ModelPosition = Default.SpellVisualEffectName.ModelPosition,
                Field_9_1_0_38549_014 = Default.SpellVisualEffectName.Field_9_1_0_38549_014
            };
        }
    }
}
