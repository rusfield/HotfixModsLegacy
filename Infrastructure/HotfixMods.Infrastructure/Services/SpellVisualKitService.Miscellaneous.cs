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
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.Id, dto.IsUpdate);
            var spellVisualKitid = await GetIdByConditionsAsync<SpellVisualKit>(dto.SpellVisualKit.Id, dto.IsUpdate);

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


            dto.HotfixModsEntity.Id = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordId = spellVisualKitid;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.SpellVisualKit.Id = spellVisualKitid;
            dto.SpellVisualKit.VerifiedBuild = VerifiedBuild;

            foreach (var effectGroup in dto.EffectGroups)
            {
                effectGroup.SpellVisualKitEffect.Id = nextSpellVisualKitEffectId++;
                effectGroup.SpellVisualKitEffect.ParentSpellVisualKitId = (int)spellVisualKitid;
                effectGroup.SpellVisualScreenEffect.VerifiedBuild = VerifiedBuild;

                if (Enum.IsDefined(typeof(SpellVisualEffectEffectType), effectGroup.SpellVisualKitEffect.EffectType))
                {
                    var type = (SpellVisualEffectEffectType)effectGroup.SpellVisualKitEffect.EffectType;
                    if (type == SpellVisualEffectEffectType.SPELL_PROCEDURAL_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextSpellProceduralEffectId;
                        effectGroup.SpellProceduralEffect.Id = nextSpellProceduralEffectId++;
                        effectGroup.SpellProceduralEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.SPELL_VISUAL_KIT_MODEL_ATTACH)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextSpellVisualKitModelAttachId;
                        effectGroup.SpellVisualKitModelAttach.SpellVisualEffectNameId = (int)nextSpellVisualEffectNameId;

                        effectGroup.SpellVisualEffectName.Id = nextSpellVisualEffectNameId++;
                        effectGroup.SpellVisualEffectName.VerifiedBuild = VerifiedBuild;

                        effectGroup.SpellVisualKitModelAttach.Id = nextSpellVisualKitModelAttachId++;
                        effectGroup.SpellVisualKitModelAttach.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.CAMERA_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextCameraEffectId;
                        effectGroup.CameraEffect.Id = nextCameraEffectId++;
                        effectGroup.CameraEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.CAMERA_EFFECT_2)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextCameraEffectId;
                        effectGroup.CameraEffect2.Id = nextCameraEffectId++;
                        effectGroup.CameraEffect2.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.SOUND_KIT)
                    {
                        // Nothing to do here
                    }
                    else if (type == SpellVisualEffectEffectType.SPELL_VISUAL_ANIM)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextSpellVisualAnimId;
                        effectGroup.SpellVisualAnim.Id = nextSpellVisualAnimId++;
                        effectGroup.SpellVisualAnim.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.SHADOWY_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextShadowyEffectId;
                        effectGroup.ShadowyEffect.Id = nextShadowyEffectId++;
                        effectGroup.ShadowyEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.SPELL_EFFECT_EMISSION)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextSpellEffectEmissionId;
                        effectGroup.SpellEffectEmission.Id = nextSpellEffectEmissionId++;
                        effectGroup.SpellEffectEmission.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.OUTLINE_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextOutlineEffectId;
                        effectGroup.OutlineEffect.Id = nextOutlineEffectId++;
                        effectGroup.OutlineEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.UNIT_SOUND_TYPE)
                    {
                        // Nothing to do here
                    }
                    else if (type == SpellVisualEffectEffectType.DISSOLVE_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextDissolveEffectId;
                        effectGroup.DissolveEffect.Id = nextDissolveEffectId++;
                        effectGroup.DissolveEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.EDGE_GLOW_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextEdgeGlowEffectId;
                        effectGroup.EdgeGlowEffect.Id = nextEdgeGlowEffectId++;
                        effectGroup.EdgeGlowEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.BEAM_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextBeamEffectId;
                        effectGroup.BeamEffect.Id = nextBeamEffectId++;
                        effectGroup.BeamEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.CLIENT_SCENE_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextClientSceneEffectId;
                        effectGroup.ClientSceneEffect.Id = nextClientSceneEffectId++;
                        effectGroup.ClientSceneEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.CLONE_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextCloneEffectId;
                        effectGroup.CloneEffect.Id = nextCloneEffectId++;
                        effectGroup.CloneEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.GRADIENT_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextGradientEffectId;
                        effectGroup.GradientEffect.Id = nextGradientEffectId++;
                        effectGroup.GradientEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.BARRAGE_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextBarrageEffectId;
                        effectGroup.BarrageEffect.SpellVisualEffectNameId = (int)nextSpellVisualEffectNameId;

                        effectGroup.SpellVisualEffectName.Id = nextSpellVisualEffectNameId++;
                        effectGroup.SpellVisualEffectName.VerifiedBuild = VerifiedBuild;

                        effectGroup.BarrageEffect.Id = nextBarrageEffectId++;
                        effectGroup.BarrageEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.ROPE_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextRopeEffectId;
                        effectGroup.RopeEffect.Id = nextRopeEffectId++;
                        effectGroup.RopeEffect.VerifiedBuild = VerifiedBuild;
                    }
                    else if (type == SpellVisualEffectEffectType.SPELL_VISUAL_SCREEN_EFFECT)
                    {
                        effectGroup.SpellVisualKitEffect.Effect = (int)nextSpellVisualScreenEffectId;
                        effectGroup.SpellVisualScreenEffect.Id = nextSpellVisualScreenEffectId++;
                        effectGroup.SpellVisualScreenEffect.VerifiedBuild = VerifiedBuild;
                    }
                }
            }
        }
    }
}
