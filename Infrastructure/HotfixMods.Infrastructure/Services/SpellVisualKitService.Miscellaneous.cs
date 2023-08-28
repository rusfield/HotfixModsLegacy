using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellVisualKitService
    {
        async Task SetIdAndVerifiedBuild(SpellVisualKitDto dto)
        {
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            var spellVisualKitId = await GetIdByConditionsAsync<SpellVisualKit>((ulong)dto.SpellVisualKit.ID, dto.IsUpdate);

            var nextSpellVisualKitEffectId = await GetNextIdAsync<SpellVisualKitEffect>();
            var nextSpellVisualEffectNameId = await GetNextIdAsync<SpellVisualEffectName>();

            var nextSpellProceduralEffectId = await GetNextIdAsync<SpellProceduralEffect>();
            var nextSpellVisualKitModelAttachId = await GetNextIdAsync<SpellVisualKitModelAttach>();
            var nextCameraEffectId = await GetNextIdAsync<CameraEffect>();
            var nextSpellVisualAnimId = await GetNextIdAsync<SpellVisualAnim>();
            var nextShadowyEffectId = await GetNextIdAsync<ShadowyEffect>();
            var nextSpellEffectEmissionId = await GetNextIdAsync<SpellEffectEmission>();
            var nextOutlineEffectId = await GetNextIdAsync<OutlineEffect>();
            var nextDissolveEffectId = await GetNextIdAsync<DissolveEffect>();
            var nextEdgeGlowEffectId = await GetNextIdAsync<EdgeGlowEffect>();
            var nextBeamEffectId = await GetNextIdAsync<BeamEffect>();
            var nextClientSceneEffectId = await GetNextIdAsync<ClientSceneEffect>();
            var nextCloneEffectId = await GetNextIdAsync<CloneEffect>();
            var nextGradientEffectId = await GetNextIdAsync<GradientEffect>();
            var nextBarrageEffectId = await GetNextIdAsync<BarrageEffect>();
            var nextRopeEffectId = await GetNextIdAsync<RopeEffect>();
            var nextSpellVisualScreenEffectId = await GetNextIdAsync<SpellVisualScreenEffect>();


            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = spellVisualKitId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.SpellVisualKit.ID = (int)spellVisualKitId;
            dto.SpellVisualKit.VerifiedBuild = VerifiedBuild;

            foreach (var effectGroup in dto.EffectGroups)
            {
                effectGroup.SpellVisualKitEffect.ID = (int)nextSpellVisualKitEffectId++;
                effectGroup.SpellVisualKitEffect.ParentSpellVisualKitID = (int)spellVisualKitId;
                effectGroup.SpellVisualScreenEffect.VerifiedBuild = VerifiedBuild;

                if (Enum.IsDefined(typeof(SpellVisualKitEffect_EffectType), effectGroup.SpellVisualKitEffect.EffectType))
                {
                    var type = (SpellVisualKitEffect_EffectType)effectGroup.SpellVisualKitEffect.EffectType;
                    if (type == SpellVisualKitEffect_EffectType.SPELL_PROCEDURAL_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextSpellProceduralEffectId;
                        effectGroup.SpellProceduralEffect.ID = (int)nextSpellProceduralEffectId++;
                        effectGroup.SpellProceduralEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.SPELL_VISUAL_KIT_MODEL_ATTACH)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextSpellVisualKitModelAttachId;
                        effectGroup.SpellVisualKitModelAttach.SpellVisualEffectNameID = (int)nextSpellVisualEffectNameId;

                        effectGroup.SpellVisualEffectName.ID = (int)nextSpellVisualEffectNameId++;
                        effectGroup.SpellVisualEffectName.VerifiedBuild = VerifiedBuild;

                        effectGroup.SpellVisualKitModelAttach.ID = (int)nextSpellVisualKitModelAttachId++;
                        effectGroup.SpellVisualKitModelAttach.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.CAMERA_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextCameraEffectId;
                        effectGroup.CameraEffect.ID = (int)nextCameraEffectId++;
                        effectGroup.CameraEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.CAMERA_EFFECT_2)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextCameraEffectId;
                        effectGroup.CameraEffect2.ID = (int)nextCameraEffectId++;
                        effectGroup.CameraEffect2.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.SOUND_KIT)
                    {
                        // Nothing to do here
                    }
                    else if (type == SpellVisualKitEffect_EffectType.SPELL_VISUAL_ANIM)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextSpellVisualAnimId;
                        effectGroup.SpellVisualAnim.ID = (int)nextSpellVisualAnimId++;
                        effectGroup.SpellVisualAnim.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.SHADOWY_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextShadowyEffectId;
                        effectGroup.ShadowyEffect.ID = (int)nextShadowyEffectId++;
                        effectGroup.ShadowyEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.SPELL_EFFECT_EMISSION)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextSpellEffectEmissionId;
                        effectGroup.SpellEffectEmission.ID = (int)nextSpellEffectEmissionId++;
                        effectGroup.SpellEffectEmission.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.OUTLINE_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextOutlineEffectId;
                        effectGroup.OutlineEffect.ID = (int)nextOutlineEffectId++;
                        effectGroup.OutlineEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.UNIT_SOUND_TYPE)
                    {
                        // Nothing to do here
                    }
                    else if (type == SpellVisualKitEffect_EffectType.DISSOLVE_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextDissolveEffectId;
                        effectGroup.DissolveEffect.ID = (int)nextDissolveEffectId++;
                        effectGroup.DissolveEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.EDGE_GLOW_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextEdgeGlowEffectId;
                        effectGroup.EdgeGlowEffect.ID = (int)nextEdgeGlowEffectId++;
                        effectGroup.EdgeGlowEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.BEAM_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextBeamEffectId;
                        effectGroup.BeamEffect.ID = (int)nextBeamEffectId++;
                        effectGroup.BeamEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.CLIENT_SCENE_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextClientSceneEffectId;
                        effectGroup.ClientSceneEffect.ID = (int)nextClientSceneEffectId++;
                        effectGroup.ClientSceneEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.CLONE_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextCloneEffectId;
                        effectGroup.CloneEffect.ID = (int)nextCloneEffectId++;
                        effectGroup.CloneEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.GRADIENT_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextGradientEffectId;
                        effectGroup.GradientEffect.ID = (int)nextGradientEffectId++;
                        effectGroup.GradientEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.BARRAGE_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextBarrageEffectId;
                        effectGroup.BarrageEffect.SpellVisualEffectNameID = (int)nextSpellVisualEffectNameId;

                        effectGroup.SpellVisualEffectName.ID = (int)nextSpellVisualEffectNameId++;
                        effectGroup.SpellVisualEffectName.VerifiedBuild = VerifiedBuild;

                        effectGroup.BarrageEffect.ID = (int)nextBarrageEffectId++;
                        effectGroup.BarrageEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.ROPE_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextRopeEffectId;
                        effectGroup.RopeEffect.ID = (int)nextRopeEffectId++;
                        effectGroup.RopeEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualKitEffect_EffectType.SPELL_VISUAL_SCREEN_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextSpellVisualScreenEffectId;
                        effectGroup.SpellVisualScreenEffect.ID = (int)nextSpellVisualScreenEffectId++;
                        effectGroup.SpellVisualScreenEffect.VerifiedBuild = VerifiedBuild;
                    }
                }
            }
        }
    }
}
