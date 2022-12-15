using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService : ServiceBase
    {
        public AnimKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public async Task<AnimKitDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var result = new AnimKitDto();

            return result;
        }

        public async Task<AnimKitDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var animKit = await GetSingleByIdAsync<AnimKit>(id);
            if (null == animKit)
            {
                return null;
            }

            var result = new AnimKitDto()
            {
                AnimKit = animKit,
                SegmentGroups = new(),
                Entity = await GetHotfixModsEntity(animKit.Id)
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
                    result.SegmentGroups.Add(new()
                    {
                        AnimKitSegment = segment,
                        AnimKitConfig = animKitConfig,
                        AnimKitConfigBoneSet = animKitConfigBoneSet
                    });
                }
            }

            return result;
        }

        public async Task SaveAsync(AnimKitDto animKitDto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            await SetIdAndVerifiedBuild(animKitDto);

            await SaveAsync(animKitDto.Entity);
            await SaveAsync(animKitDto.AnimKit);
            await SaveAsync(animKitDto.SegmentGroups.Select(s => s.AnimKitSegment));
            await SaveAsync(animKitDto.SegmentGroups.Select(s => s.AnimKitConfig));
            await SaveAsync(animKitDto.SegmentGroups.Select(s => s.AnimKitConfigBoneSet));
        }

        public async Task DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var animKitDto = await GetByIdAsync(id);
            if (null == animKitDto)
            {
                return;
            }
                

            foreach(var segmentGroup in animKitDto.SegmentGroups)
            {
                await DeleteAsync<AnimKitSegment>(new DbParameter(nameof(AnimKitSegment.Id), segmentGroup.AnimKitSegment.Id));
                await DeleteAsync<AnimKitConfig>(new DbParameter(nameof(AnimKitConfig.Id), segmentGroup.AnimKitConfig.Id));
                await DeleteAsync<AnimKitConfigBoneSet>(new DbParameter(nameof(AnimKitConfigBoneSet.Id), segmentGroup.AnimKitConfigBoneSet.Id));
            }
            await DeleteAsync<AnimKit>(new DbParameter(nameof(AnimKit.Id), id));
            await DeleteAsync<HotfixModsEntity>(new DbParameter(nameof(HotfixModsEntity.Id), animKitDto.Entity.Id));
        }
    }
}
