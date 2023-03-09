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
using System.Text.Json;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService : ServiceBase
    {
        public AnimKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IExceptionHandler exceptionHandler, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.AnimKitSettings.FromId;
            ToId = appConfig.AnimKitSettings.ToId;
            VerifiedBuild = appConfig.AnimKitSettings.VerifiedBuild;
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            try
            {
                var dtos = await GetAsync<HotfixModsEntity>(false, new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
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

        public async Task<AnimKitDto?> GetByIdAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(5);

            try
            {
                var animKit = await GetSingleAsync<AnimKit>(callback, progress, new DbParameter(nameof(AnimKit.ID), id));
                if (null == animKit)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(AnimKit)} not found", 100);
                    return null;
                }

                var hotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(callback, progress, animKit.ID);
                var result = new AnimKitDto()
                {
                    AnimKit = animKit,
                    SegmentGroups = new(),
                    HotfixModsEntity = hotfixModsEntity,
                    IsUpdate = true
                };

                var segments = await GetAsync<AnimKitSegment>(callback, progress, false, new DbParameter(nameof(AnimKitSegment.ParentAnimKitID), id));
                callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(AnimKitConfig)} and {nameof(AnimKitConfigBoneSet)}", progress());
                foreach (var segment in segments)
                {
                    var animKitConfig = await GetSingleAsync<AnimKitConfig>(new DbParameter(nameof(AnimKitConfig.ID), segment.AnimKitConfigID)) ?? new();
                    var animKitConfigBoneSets = await GetAsync<AnimKitConfigBoneSet>(false, new DbParameter(nameof(AnimKitConfigBoneSet.ParentAnimKitConfigID), animKitConfig.ID));
                    if (animKitConfigBoneSets.Count == 0)
                        animKitConfigBoneSets.Add(new());

                    result.SegmentGroups.Add(new()
                    {
                        AnimKitSegment = segment,
                        AnimKitConfig = animKitConfig,
                        AnimKitConfigBoneSet = animKitConfigBoneSets
                    });
                }
                callback.Invoke(LoadingHelper.Loading, $"Loading successful", 100);
                return result;

            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return null;
        }

        public async Task<bool> SaveAsync(AnimKitDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(8);

            try
            {
                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync(dto.AnimKit.ID);
                }

                callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
                await SetIdAndVerifiedBuild(dto);

                await SaveAsync(callback, progress, dto.HotfixModsEntity);
                await SaveAsync(callback, progress, dto.AnimKit);
                await SaveAsync(callback, progress, dto.SegmentGroups.Select(s => s.AnimKitSegment).ToList());
                await SaveAsync(callback, progress, dto.SegmentGroups.Select(s => s.AnimKitConfig).ToList());
                await SaveAsync(callback, progress, dto.SegmentGroups.SelectMany(s => s.AnimKitConfigBoneSet).ToList());

                dto.IsUpdate = true;
                callback.Invoke(LoadingHelper.Saving, $"Saving successful", 100);
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return false;
        }

        public async Task<bool> DeleteAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(4);

            try
            {
                var dto = await GetByIdAsync(id);
                if (null == dto)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }

                callback.Invoke(LoadingHelper.Deleting, $"Deleting {nameof(AnimKitSegment)}, {nameof(AnimKitConfig)} and {nameof(AnimKitConfigBoneSet)}", 100);
                await dto.SegmentGroups.ForEachAsync(async s =>
                {
                    await DeleteAsync(s.AnimKitSegment);
                    await DeleteAsync(s.AnimKitConfig);
                    await DeleteAsync(s.AnimKitConfigBoneSet);
                });

                await DeleteAsync(callback, progress, dto.AnimKit);
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
