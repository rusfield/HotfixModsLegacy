using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService
    {
        async Task SetIdAndVerifiedBuild(AnimKitDto dto)
        {
            // Step 1: Init IDs of single entities
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.Id, dto.IsUpdate);
            var animKitId = await GetIdByConditionsAsync<AnimKit>(dto.AnimKit.Id, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextAnimKitSegmentId = await GetNextIdAsync<AnimKitSegment>();
            var nextAnimKitConfigId = await GetNextIdAsync<AnimKitConfig>();
            var nextAnimKitConfigBoneSetId = await GetNextIdAsync<AnimKitConfigBoneSet>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.Id = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordId = animKitId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;
            dto.AnimKit.Id = animKitId;
            dto.AnimKit.VerifiedBuild = VerifiedBuild;


            dto.SegmentGroups.ForEach(s =>
            {
                s.AnimKitSegment.ParentAnimKitId = (ushort)animKitId;
                s.AnimKitSegment.Id = nextAnimKitSegmentId;
                s.AnimKitSegment.AnimKitConfigId = (ushort)nextAnimKitConfigId;
                s.AnimKitConfig.Id = nextAnimKitConfigId;
                s.AnimKitConfigBoneSet.Id = nextAnimKitConfigBoneSetId;
                s.AnimKitConfigBoneSet.ParentAnimKitConfigId = (int)nextAnimKitConfigId;

                s.AnimKitSegment.VerifiedBuild = VerifiedBuild;
                s.AnimKitConfig.VerifiedBuild = VerifiedBuild;
                s.AnimKitConfigBoneSet.VerifiedBuild = VerifiedBuild;

                nextAnimKitSegmentId++;
                nextAnimKitConfigId++;
                nextAnimKitConfigBoneSetId++;
            });

        }
    }
}
