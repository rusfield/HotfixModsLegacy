using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService
    {
        async Task SetIdAndVerifiedBuild(AnimKitDto animKitDto)
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

                animKitDto.SegmentGroups.ForEach(s =>
                {
                    s.AnimKitSegment.ParentAnimKitId = (ushort)newAnimKitId;
                    s.AnimKitSegment.Id = newAnimKitSegmentId;
                    s.AnimKitSegment.AnimKitConfigId = (ushort)newAnimKitConfigId;
                    s.AnimKitConfig.Id = newAnimKitConfigId;
                    s.AnimKitConfigBoneSet.Id = newAnimKitConfigBoneSetId;
                    s.AnimKitConfigBoneSet.ParentAnimKitConfigId = newAnimKitConfigId;

                    newAnimKitSegmentId++;
                    newAnimKitConfigId++;
                    newAnimKitConfigBoneSetId++;
                });
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
