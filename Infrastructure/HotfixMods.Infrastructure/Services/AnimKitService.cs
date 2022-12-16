using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using static HotfixMods.Infrastructure.DtoModels.AnimKitDto;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService : ServiceBase
    {
        public AnimKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public async Task<AnimKitDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            return new AnimKitDto();
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
                Entity = await GetExistingOrNewHotfixModsEntity(animKit.Id),
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

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var animKitDto = await GetByIdAsync(id);
            if (null == animKitDto)
            {
                return false;
            }


            animKitDto.SegmentGroups.ForEach(async s =>
            {
                await DeleteAsync(s.AnimKitSegment);
                await DeleteAsync(s.AnimKitConfig);
                await DeleteAsync(s.AnimKitConfigBoneSet);
            });
            
            await DeleteAsync(animKitDto.AnimKit);
            await DeleteAsync(animKitDto.Entity);

            return true;
        }
    }
}
