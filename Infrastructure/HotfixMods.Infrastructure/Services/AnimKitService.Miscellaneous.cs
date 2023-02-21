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
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            var animKitId = await GetIdByConditionsAsync<AnimKit>(dto.AnimKit.ID, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextAnimKitSegmentId = await GetNextIdAsync<AnimKitSegment>();
            var nextAnimKitConfigId = await GetNextIdAsync<AnimKitConfig>();
            var nextAnimKitConfigBoneSetId = await GetNextIdAsync<AnimKitConfigBoneSet>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = animKitId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.AnimKit.ID = animKitId;
            dto.AnimKit.VerifiedBuild = VerifiedBuild;

            byte orderIndex = 0;
            dto.SegmentGroups.ForEach(s =>
            {
                s.AnimKitSegment.ParentAnimKitID = (ushort)animKitId;
                s.AnimKitSegment.ID = nextAnimKitSegmentId;
                s.AnimKitSegment.AnimKitConfigID = (ushort)nextAnimKitConfigId;
                s.AnimKitConfig.ID = nextAnimKitConfigId;
                s.AnimKitSegment.OrderIndex = orderIndex;


                s.AnimKitSegment.VerifiedBuild = VerifiedBuild;
                s.AnimKitConfig.VerifiedBuild = VerifiedBuild;

                foreach(var boneSet in s.AnimKitConfigBoneSet)
                {
                    boneSet.ID = nextAnimKitConfigBoneSetId++;
                    boneSet.ParentAnimKitConfigID = (int)nextAnimKitConfigId;
                    boneSet.VerifiedBuild = VerifiedBuild;
                }

                nextAnimKitSegmentId++;
                nextAnimKitConfigId++;
                orderIndex++;
            });

        }
    }
}
