using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService
    {
        protected async Task SetIdAndVerifiedBuild(AnimKitDto animKitDto)
        {
            if (!animKitDto.IsUpdate)
            {
                var newAnimKitId = await GetNextIdAsync<AnimKit>();

                animKitDto.Entity.Id = await GetNextIdAsync<HotfixModsEntity>();
                animKitDto.Entity.RecordId = newAnimKitId;
                animKitDto.AnimKit.Id = newAnimKitId;

                var newAnimKitSegmentId = await GetNextIdAsync<AnimKitSegment>();
                var newAnimKitConfigId = await GetNextIdAsync<AnimKitConfig>();
                var newAnimKitConfigBoneSetId = await GetNextIdAsync<AnimKitConfigBoneSet>();

                foreach (var segmentGroup in animKitDto.SegmentGroups)
                {
                    segmentGroup.AnimKitSegment.ParentAnimKitId = (ushort)newAnimKitId; // TODO: maybe add a validation to ensure the ID is not greater than ushort
                    segmentGroup.AnimKitSegment.Id = newAnimKitSegmentId;
                    segmentGroup.AnimKitSegment.AnimKitConfigId = (ushort)newAnimKitConfigId;

                    segmentGroup.AnimKitConfig.Id = newAnimKitConfigId;
                    segmentGroup.AnimKitConfigBoneSet.Id = newAnimKitConfigBoneSetId;
                    segmentGroup.AnimKitConfigBoneSet.ParentAnimKitConfigId = newAnimKitConfigId;

                    newAnimKitSegmentId++;
                    newAnimKitConfigId++;
                    newAnimKitConfigBoneSetId++;
                }
            }

            animKitDto.Entity.VerifiedBuild = VerifiedBuild;
            animKitDto.AnimKit.VerifiedBuild = VerifiedBuild;
            animKitDto.SegmentGroups.ForEach(s =>
            {
                s.AnimKitConfig.VerifiedBuild = VerifiedBuild;
                s.AnimKitConfigBoneSet.VerifiedBuild = VerifiedBuild;
                s.AnimKitSegment.VerifiedBuild = VerifiedBuild;
            });
        }
    }
}
