using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Helpers;
using System.Text.Json;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService : ServiceBase
    {
        public AnimKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public AnimKitDto GetNew(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            callback.Invoke(LoadingHelper.Loading, "Returning new template", 100);
            return new()
            {
                HotfixModsEntity = new()
                {
                    Name = "New Anim Kit"
                }
            };
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            var dtos = await GetAsync<HotfixModsEntity>(new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
            var results = new List<DashboardModel>();
            foreach (var dto in dtos)
            {
                results.Add(new()
                {
                    Id = dto.RecordId,
                    Name = dto.Name,
                    AvatarUrl = null
                });
            }
            return results;
        }

        public async Task<AnimKitDto?> GetByIdAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(5);

            var animKit = await GetSingleAsync<AnimKit>(callback, progress, new DbParameter(nameof(AnimKit.Id), id));
            if (null == animKit)
            {
                callback.Invoke(LoadingHelper.Loading, $"{nameof(AnimKit)} not found", 100);
                return null;
            }

            var hotfixModsEntity = await GetExistingOrNewHotfixModsEntity(callback, progress, animKit.Id);
            var result = new AnimKitDto()
            {
                AnimKit = animKit,
                SegmentGroups = new(),
                HotfixModsEntity = hotfixModsEntity,
                IsUpdate = true
            };

            var segments = await GetAsync<AnimKitSegment>(callback, progress, new DbParameter(nameof(AnimKitSegment.ParentAnimKitId), id));
            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(AnimKitConfig)} and {nameof(AnimKitConfigBoneSet)}", progress());
            foreach (var segment in segments)
            {
                var animKitConfig = await GetSingleAsync<AnimKitConfig>(new DbParameter(nameof(AnimKitConfig.Id), segment.AnimKitConfigId)) ?? new();
                var animKitConfigBoneSets = await GetAsync<AnimKitConfigBoneSet>(new DbParameter(nameof(AnimKitConfigBoneSet.ParentAnimKitConfigId), animKitConfig.Id));
                if (animKitConfigBoneSets.Count == 0)
                    animKitConfigBoneSets.Add(new());

                foreach(var animKitConfigBoneSet in animKitConfigBoneSets)
                {
                    // Make new copies and references 
                    var newAnimKitSegment = JsonSerializer.Deserialize<AnimKitSegment>(JsonSerializer.Serialize(segment))!;
                    var newAnimKitConfig = JsonSerializer.Deserialize<AnimKitConfig>(JsonSerializer.Serialize(animKitConfig))!;

                    result.SegmentGroups.Add(new()
                    {
                        AnimKitSegment = newAnimKitSegment, 
                        AnimKitConfig = newAnimKitConfig,
                        AnimKitConfigBoneSet = animKitConfigBoneSet
                    });
                }
            }
            callback.Invoke(LoadingHelper.Loading, $"Loading successful", 100);
            return result;
        }

        public async Task<bool> SaveAsync(AnimKitDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(7);

            callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
            await SetIdAndVerifiedBuild(dto);

            await SaveAsync(callback, progress, dto.HotfixModsEntity);
            await SaveAsync(callback, progress, dto.AnimKit);
            await SaveAsync(callback, progress, dto.SegmentGroups.Select(s => s.AnimKitSegment).ToList());
            await SaveAsync(callback, progress, dto.SegmentGroups.Select(s => s.AnimKitConfig).ToList());
            await SaveAsync(callback, progress, dto.SegmentGroups.Select(s => s.AnimKitConfigBoneSet).ToList());

            dto.IsUpdate = true;
            callback.Invoke(LoadingHelper.Saving, $"Saving successful", 100);
            return true;
        }

        public async Task<bool> DeleteAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(4);

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
    }
}
