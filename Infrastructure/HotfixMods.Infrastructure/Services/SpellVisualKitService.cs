﻿using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.AggregateModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellVisualKitService : ServiceBase
    {
        public SpellVisualKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IServerEnumProvider serverEnumProvider, IListfileProvider listfileProvider, IExceptionHandler exceptionHandler, AppConfig appConfig)
            : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, serverEnumProvider, listfileProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.SpellVisualKitSettings.FromId;
            ToId = appConfig.SpellVisualKitSettings.ToId;
            VerifiedBuild = appConfig.SpellVisualKitSettings.VerifiedBuild;
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            try
            {
                var dtos = await GetAsync<HotfixModsEntity>(DefaultCallback, DefaultProgress, true, false, new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
                var results = new List<DashboardModel>();
                foreach (var dto in dtos)
                {
                    results.Add(new()
                    {
                        ID = dto.RecordID,
                        Name = dto.Name,
                        AvatarUrl = null
                    });
                }
                return results.OrderByDescending(d => d.ID).ToList();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return new();
        }

        public async Task<SpellVisualKitDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(4);

            try
            {
                var spellVisualKit = await  GetSingleAsync<SpellVisualKit>(callback, progress, new DbParameter(nameof(SpellVisualKit.ID), id));
                if (null == spellVisualKit)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(SpellVisualKit)} not found", 100);
                    return null;
                }

                var result = new SpellVisualKitDto()
                {
                    SpellVisualKit = spellVisualKit,
                    HotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(spellVisualKit.ID),
                };

                var spellVisualKitEffects = await GetAsync<SpellVisualKitEffect>(callback, progress, new DbParameter(nameof(SpellVisualKitEffect.ParentSpellVisualKitID), id));

                callback.Invoke(LoadingHelper.Loading, "Loading effects", progress());
                await spellVisualKitEffects.ForEachAsync(async spellVisualKitEffect =>
                {
                    if (Enum.IsDefined(typeof(SpellVisualKitEffect_EffectType), spellVisualKitEffect.EffectType))
                    {
                        var group = new SpellVisualKitDto.EffectGroup();
                        group.SpellVisualKitEffect = spellVisualKitEffect;
                        var type = (SpellVisualKitEffect_EffectType)spellVisualKitEffect.EffectType;
                        if (type == SpellVisualKitEffect_EffectType.SPELL_PROCEDURAL_EFFECT)
                        {
                            group.SpellProceduralEffect = await GetSingleAsync<SpellProceduralEffect>(new DbParameter(nameof(SpellProceduralEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SPELL_VISUAL_KIT_MODEL_ATTACH)
                        {
                            group.SpellVisualKitModelAttach = await GetSingleAsync<SpellVisualKitModelAttach>(new DbParameter(nameof(SpellVisualKitModelAttach.ID), spellVisualKitEffect.Effect)) ?? new();
                            if (group.SpellVisualKitModelAttach != null)
                                group.SpellVisualEffectName = await GetSingleAsync<SpellVisualEffectName>(new DbParameter(nameof(SpellVisualEffectName.ID), group.SpellVisualKitModelAttach.SpellVisualEffectNameID)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.CAMERA_EFFECT)
                        {
                            group.CameraEffect = await GetSingleAsync<CameraEffect>(new DbParameter(nameof(CameraEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.CAMERA_EFFECT_2)
                        {
                            group.CameraEffect = await GetSingleAsync<CameraEffect>(new DbParameter(nameof(CameraEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SOUND_KIT)
                        {
                            // Nothing to do here
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SPELL_VISUAL_ANIM)
                        {
                            group.SpellVisualAnim = await GetSingleAsync<SpellVisualAnim>(new DbParameter(nameof(SpellVisualAnim.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SHADOWY_EFFECT)
                        {
                            group.ShadowyEffect = await GetSingleAsync<ShadowyEffect>(new DbParameter(nameof(ShadowyEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SPELL_EFFECT_EMISSION)
                        {
                            group.SpellEffectEmission = await GetSingleAsync<SpellEffectEmission>(new DbParameter(nameof(SpellEffectEmission.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.OUTLINE_EFFECT)
                        {
                            group.OutlineEffect = await GetSingleAsync<OutlineEffect>(new DbParameter(nameof(OutlineEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.UNIT_SOUND_TYPE)
                        {
                            // Nothing to do here
                        }
                        else if (type == SpellVisualKitEffect_EffectType.DISSOLVE_EFFECT)
                        {
                            group.DissolveEffect = await GetSingleAsync<DissolveEffect>(new DbParameter(nameof(DissolveEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.EDGE_GLOW_EFFECT)
                        {
                            group.EdgeGlowEffect = await GetSingleAsync<EdgeGlowEffect>(new DbParameter(nameof(EdgeGlowEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.BEAM_EFFECT)
                        {
                            group.BeamEffect = await GetSingleAsync<BeamEffect>(new DbParameter(nameof(BeamEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.CLIENT_SCENE_EFFECT)
                        {
                            group.ClientSceneEffect = await GetSingleAsync<ClientSceneEffect>(new DbParameter(nameof(ClientSceneEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.CLONE_EFFECT)
                        {
                            group.CloneEffect = await GetSingleAsync<CloneEffect>(new DbParameter(nameof(CloneEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.GRADIENT_EFFECT)
                        {
                            group.GradientEffect = await GetSingleAsync<GradientEffect>(new DbParameter(nameof(GradientEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.BARRAGE_EFFECT)
                        {
                            group.BarrageEffect = await GetSingleAsync<BarrageEffect>(new DbParameter(nameof(BarrageEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                            if (group.BarrageEffect != null)
                                group.SpellVisualEffectName = await GetSingleAsync<SpellVisualEffectName>(new DbParameter(nameof(SpellVisualEffectName.ID), group.BarrageEffect.SpellVisualEffectNameID)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.ROPE_EFFECT)
                        {
                            group.RopeEffect = await GetSingleAsync<RopeEffect>(new DbParameter(nameof(RopeEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SPELL_VISUAL_SCREEN_EFFECT)
                        {
                            group.SpellVisualScreenEffect = await GetSingleAsync<SpellVisualScreenEffect>(new DbParameter(nameof(SpellVisualScreenEffect.ID), spellVisualKitEffect.Effect)) ?? new();
                        }

                        result.EffectGroups.Add(group);
                    }
                });



                callback.Invoke(LoadingHelper.Loading, $"Loading successful", 100);
                result.IsUpdate= true;
                return result;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return null;
        }

        public async Task<bool> SaveAsync(SpellVisualKitDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(6);

            try
            {
                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync(dto.SpellVisualKit.ID);
                }

                callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
                await SetIdAndVerifiedBuild(dto);

                await SaveAsync(callback, progress, dto.HotfixModsEntity);
                await SaveAsync(callback, progress, dto.SpellVisualKit);

                callback.Invoke(LoadingHelper.Saving, "Saving effects", progress());
                await dto.EffectGroups.ForEachAsync(async effectGroup =>
                {
                    await SaveAsync(effectGroup.SpellVisualKitEffect);
                    if (Enum.IsDefined(typeof(SpellVisualKitEffect_EffectType), effectGroup.SpellVisualKitEffect.EffectType))
                    {
                        var type = (SpellVisualKitEffect_EffectType)effectGroup.SpellVisualKitEffect.EffectType;
                        if (type == SpellVisualKitEffect_EffectType.SPELL_PROCEDURAL_EFFECT)
                        {
                            await SaveAsync(effectGroup.SpellProceduralEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SPELL_VISUAL_KIT_MODEL_ATTACH)
                        {
                            await SaveAsync(effectGroup.SpellVisualKitModelAttach);
                            await SaveAsync(effectGroup.SpellVisualEffectName);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.CAMERA_EFFECT)
                        {
                            await SaveAsync(effectGroup.CameraEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.CAMERA_EFFECT_2)
                        {
                            await SaveAsync(effectGroup.CameraEffect2);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SOUND_KIT)
                        {
                            // Nothing to do here
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SPELL_VISUAL_ANIM)
                        {
                            await SaveAsync(effectGroup.SpellVisualAnim);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SHADOWY_EFFECT)
                        {
                            await SaveAsync(effectGroup.ShadowyEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SPELL_EFFECT_EMISSION)
                        {
                            await SaveAsync(effectGroup.SpellEffectEmission);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.OUTLINE_EFFECT)
                        {
                            await SaveAsync(effectGroup.OutlineEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.UNIT_SOUND_TYPE)
                        {
                            // Nothing to do here
                        }
                        else if (type == SpellVisualKitEffect_EffectType.DISSOLVE_EFFECT)
                        {
                            await SaveAsync(effectGroup.DissolveEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.EDGE_GLOW_EFFECT)
                        {
                            await SaveAsync(effectGroup.EdgeGlowEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.BEAM_EFFECT)
                        {
                            await SaveAsync(effectGroup.BeamEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.CLIENT_SCENE_EFFECT)
                        {
                            await SaveAsync(effectGroup.ClientSceneEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.CLONE_EFFECT)
                        {
                            await SaveAsync(effectGroup.CloneEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.GRADIENT_EFFECT)
                        {
                            await SaveAsync(effectGroup.GradientEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.BARRAGE_EFFECT)
                        {
                            await SaveAsync(effectGroup.BarrageEffect);
                            await SaveAsync(effectGroup.SpellVisualEffectName);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.ROPE_EFFECT)
                        {
                            await SaveAsync(effectGroup.RopeEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SPELL_VISUAL_SCREEN_EFFECT)
                        {
                            await SaveAsync(effectGroup.SpellVisualScreenEffect);
                        }
                    }
                });

                callback.Invoke(LoadingHelper.Saving, "Saving successful", 100);
                dto.IsUpdate = true;
                return true;

            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(4);

            try
            {
                var dto = await GetByIdAsync(id);
                if (null == dto)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }

                callback.Invoke(LoadingHelper.Saving, "Deleting effects", progress());
                await dto.EffectGroups.ForEachAsync(async effectGroup =>
                {
                    await DeleteAsync(effectGroup.SpellVisualKitEffect);
                    if (Enum.IsDefined(typeof(SpellVisualKitEffect_EffectType), effectGroup.SpellVisualKitEffect.EffectType))
                    {
                        var type = (SpellVisualKitEffect_EffectType)effectGroup.SpellVisualKitEffect.EffectType;
                        if (type == SpellVisualKitEffect_EffectType.SPELL_PROCEDURAL_EFFECT)
                        {
                            await DeleteAsync(effectGroup.SpellProceduralEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SPELL_VISUAL_KIT_MODEL_ATTACH)
                        {
                            await DeleteAsync(effectGroup.SpellVisualKitModelAttach);
                            await DeleteAsync(effectGroup.SpellVisualEffectName);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.CAMERA_EFFECT)
                        {
                            await DeleteAsync(effectGroup.CameraEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.CAMERA_EFFECT_2)
                        {
                            await DeleteAsync(effectGroup.CameraEffect2);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SOUND_KIT)
                        {
                            // Nothing to do here
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SPELL_VISUAL_ANIM)
                        {
                            await DeleteAsync(effectGroup.SpellVisualAnim);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SHADOWY_EFFECT)
                        {
                            await DeleteAsync(effectGroup.ShadowyEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SPELL_EFFECT_EMISSION)
                        {
                            await DeleteAsync(effectGroup.SpellEffectEmission);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.OUTLINE_EFFECT)
                        {
                            await DeleteAsync(effectGroup.OutlineEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.UNIT_SOUND_TYPE)
                        {
                            // Nothing to do here
                        }
                        else if (type == SpellVisualKitEffect_EffectType.DISSOLVE_EFFECT)
                        {
                            await DeleteAsync(effectGroup.DissolveEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.EDGE_GLOW_EFFECT)
                        {
                            await DeleteAsync(effectGroup.EdgeGlowEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.BEAM_EFFECT)
                        {
                            await DeleteAsync(effectGroup.BeamEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.CLIENT_SCENE_EFFECT)
                        {
                            await DeleteAsync(effectGroup.ClientSceneEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.CLONE_EFFECT)
                        {
                            await DeleteAsync(effectGroup.CloneEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.GRADIENT_EFFECT)
                        {
                            await DeleteAsync(effectGroup.GradientEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.BARRAGE_EFFECT)
                        {
                            await DeleteAsync(effectGroup.BarrageEffect);
                            await DeleteAsync(effectGroup.SpellVisualEffectName);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.ROPE_EFFECT)
                        {
                            await DeleteAsync(effectGroup.RopeEffect);
                        }
                        else if (type == SpellVisualKitEffect_EffectType.SPELL_VISUAL_SCREEN_EFFECT)
                        {
                            await DeleteAsync(effectGroup.SpellVisualScreenEffect);
                        }
                    }
                });

                await DeleteAsync(callback, progress, dto.SpellVisualKit);
                await DeleteAsync(callback, progress, dto.HotfixModsEntity);

                callback.Invoke(LoadingHelper.Deleting, "Delete successful", 100);
                return true;

            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return false;
        }
    }
}
