using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService
    {
        void NormalizeBeforeSave(AnimKitDto dto)
        {
            foreach (var segmentGroup in dto.SegmentGroups)
            {
                segmentGroup.AnimKitConfigBoneSet = segmentGroup.AnimKitConfigBoneSet
                    .Where(boneSet => boneSet != null && boneSet.AnimKitPriorityID != 0)
                    .ToList();
            }

            // The editor can leave behind an untouched starter group when the user deletes/rebuilds
            // a kit. Keep a single default group if it is the only group, but drop obvious placeholder
            // rows once the kit contains real segment data as well.
            if (dto.SegmentGroups.Count > 1)
            {
                dto.SegmentGroups = dto.SegmentGroups
                    .Where(segmentGroup => !IsPlaceholderSegmentGroup(segmentGroup))
                    .ToList();
            }
        }

        static bool IsPlaceholderSegmentGroup(AnimKitDto.SegmentGroup segmentGroup)
        {
            return IsDefaultSegment(segmentGroup.AnimKitSegment)
                && IsDefaultConfig(segmentGroup.AnimKitConfig)
                && segmentGroup.AnimKitConfigBoneSet.Count == 1
                && IsDefaultBoneSet(segmentGroup.AnimKitConfigBoneSet[0]);
        }

        static bool IsDefaultSegment(AnimKitSegment segment)
        {
            return segment.ID == 0
                && segment.ParentAnimKitID == 0
                && segment.OrderIndex == 0
                && segment.AnimID == 0
                && segment.AnimStartTime == 0
                && segment.AnimKitConfigID == 1
                && segment.StartCondition == 0
                && segment.StartConditionParam == 0
                && segment.StartConditionDelay == 0
                && segment.EndCondition == 0
                && segment.EndConditionParam == 0
                && segment.EndConditionDelay == 0
                && segment.Speed == 1
                && segment.SegmentFlags == 0
                && segment.ForcedVariation == 0
                && segment.OverrideConfigFlags == 0
                && segment.LoopToSegmentIndex == 0
                && segment.BlendInTimeMs == 0
                && segment.BlendOutTimeMs == 0
                && segment.Field_9_0_1_34278_018 == 1;
        }

        static bool IsDefaultConfig(AnimKitConfig config)
        {
            return config.ID == 0
                && config.ConfigFlags == 0;
        }

        static bool IsDefaultBoneSet(AnimKitConfigBoneSet boneSet)
        {
            return boneSet.ID == 0
                && boneSet.AnimKitBoneSetID == 0
                && boneSet.AnimKitPriorityID == 1
                && boneSet.ParentAnimKitConfigID == 0;
        }

        async Task SetIdAndVerifiedBuild(AnimKitDto dto)
        {
            // Step 1: Init IDs of single entities
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            var animKitId = await GetIdByConditionsAsync<AnimKit>((int)dto.AnimKit.ID, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextAnimKitSegmentId = await GetNextIdAsync<AnimKitSegment>();
            var nextAnimKitConfigId = await GetNextIdAsync<AnimKitConfig>();
            var nextAnimKitConfigBoneSetId = await GetNextIdAsync<AnimKitConfigBoneSet>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = animKitId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.AnimKit.ID = (uint)animKitId;
            dto.AnimKit.VerifiedBuild = VerifiedBuild;

            int orderIndex = 0;
            dto.SegmentGroups.ForEach(s =>
            {
                s.AnimKitSegment.ParentAnimKitID = (uint)animKitId;
                s.AnimKitSegment.ID = (uint)nextAnimKitSegmentId;
                s.AnimKitSegment.AnimKitConfigID = (uint)nextAnimKitConfigId;
                s.AnimKitConfig.ID = (uint)nextAnimKitConfigId;
                s.AnimKitSegment.OrderIndex = orderIndex;


                s.AnimKitSegment.VerifiedBuild = VerifiedBuild;
                s.AnimKitConfig.VerifiedBuild = VerifiedBuild;

                foreach(var boneSet in s.AnimKitConfigBoneSet)
                {
                    boneSet.ID = (uint)nextAnimKitConfigBoneSetId++;
                    boneSet.ParentAnimKitConfigID = (uint)nextAnimKitConfigId;
                    boneSet.VerifiedBuild = VerifiedBuild;
                }

                nextAnimKitSegmentId++;
                nextAnimKitConfigId++;
                orderIndex++;
            });

        }
    }
}
