using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService
    {
        async Task SetIdAndVerifiedBuild(AnimKitDto dto)
        {
            if (!dto.IsUpdate)
            {
                var newAnimKitId = await GetNextIdAsync<AnimKit>();

                dto.HotfixModsEntity.Id = await GetNextIdAsync<HotfixModsEntity>();
                dto.HotfixModsEntity.RecordId = newAnimKitId;
                dto.AnimKit.Id = newAnimKitId;

                var newAnimKitSegmentId = await GetNextIdAsync<AnimKitSegment>();
                var newAnimKitConfigId = await GetNextIdAsync<AnimKitConfig>();
                var newAnimKitConfigBoneSetId = await GetNextIdAsync<AnimKitConfigBoneSet>();

                dto.SegmentGroups.ForEach(s =>
                {
                    s.AnimKitSegment.ParentAnimKitId = (ushort)newAnimKitId;
                    s.AnimKitSegment.Id = newAnimKitSegmentId;
                    s.AnimKitSegment.AnimKitConfigId = (ushort)newAnimKitConfigId;
                    s.AnimKitConfig.Id = newAnimKitConfigId;
                    s.AnimKitConfigBoneSet.Id = newAnimKitConfigBoneSetId;
                    s.AnimKitConfigBoneSet.ParentAnimKitConfigId = (int)newAnimKitConfigId;

                    newAnimKitSegmentId++;
                    newAnimKitConfigId++;
                    newAnimKitConfigBoneSetId++;
                });
            }

            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;
            dto.AnimKit.VerifiedBuild = VerifiedBuild;
            dto.SegmentGroups.ForEach(s =>
            {
                s.AnimKitConfig.VerifiedBuild = VerifiedBuild;
                s.AnimKitConfigBoneSet.VerifiedBuild = VerifiedBuild;
                s.AnimKitSegment.VerifiedBuild = VerifiedBuild;
            });
        }
    }
}
