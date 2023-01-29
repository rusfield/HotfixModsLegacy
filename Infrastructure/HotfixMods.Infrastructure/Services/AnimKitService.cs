using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using System.Text.Json;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService : ServiceBase
    {
        public AnimKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public AnimKitDto GetNew(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            return new AnimKitDto();
        }

        public async Task<AnimKitDto?> GetByIdAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var animKit = await GetSingleAsync<AnimKit>(new DbParameter(nameof(AnimKit.Id), id));
            if (null == animKit)
            {
                return null;
            }

            var result = new AnimKitDto()
            {
                AnimKit = animKit,
                SegmentGroups = new(),
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(animKit.Id),
                IsUpdate = true
            };

            var segments = await GetAsync<AnimKitSegment>(new DbParameter(nameof(AnimKitSegment.ParentAnimKitId), id));
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

            return result;
        }

        public async Task<bool> SaveAsync(AnimKitDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            await SetIdAndVerifiedBuild(dto);

            await SaveAsync(dto.HotfixModsEntity);
            await SaveAsync(dto.AnimKit);
            await SaveAsync(dto.SegmentGroups.Select(s => s.AnimKitSegment).ToList());
            await SaveAsync(dto.SegmentGroups.Select(s => s.AnimKitConfig).ToList());
            await SaveAsync(dto.SegmentGroups.Select(s => s.AnimKitConfigBoneSet).ToList());

            dto.IsUpdate = true;
            return true;
        }

        public async Task<bool> DeleteAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var dto = await GetByIdAsync(id);
            if (null == dto)
            {
                return false;
            }


            await dto.SegmentGroups.ForEachAsync(async s =>
            {
                await DeleteAsync(s.AnimKitSegment);
                await DeleteAsync(s.AnimKitConfig);
                await DeleteAsync(s.AnimKitConfigBoneSet);
            });
            
            await DeleteAsync(dto.AnimKit);
            await DeleteAsync(dto.HotfixModsEntity);

            return true;
        }
    }
}
