using HotfixMods.Infrastructure.DefaultModels;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService
    {
        AnimKit BuildAnimKit(AnimKitDto animKitDto)
        {
            animKitDto.AddHotfix(animKitDto.Id, TableHashes.ANIM_KIT, HotfixStatuses.VALID);
            return new AnimKit()
            {
                Id = animKitDto.Id,
                VerifiedBuild = VerifiedBuild,

                OneShotDuration = animKitDto.OneShotDuration ?? Default.AnimKit.OneShotDuration,
                OneShotStopAnimKitId = animKitDto.OneShotStopAnimKitId ?? Default.AnimKit.OneShotStopAnimKitId
            };
        } 

        AnimKitSegment[] BuildAnimKitSegment(AnimKitDto animKitDto)
        {
            var result = new List<AnimKitSegment>();
            foreach(var segment in animKitDto.Segments)
            {
                int id = animKitDto.Id + (segment.OrderIndex ?? Default.AnimKitSegment.OrderIndex);
                animKitDto.AddHotfix(id, TableHashes.ANIM_KIT_SEGMENT, HotfixStatuses.VALID);
                result.Add(new AnimKitSegment()
                {
                    Id = id,
                    ParentAnimKitId = animKitDto.Id,
                    VerifiedBuild = VerifiedBuild,
                    
                    AnimId = segment.AnimId ?? Default.AnimKitSegment.AnimId,
                    AnimKitConfigId = segment.AnimKitConfigId ?? Default.AnimKitSegment.AnimKitConfigId,
                    AnimStartTime = segment.AnimStartTime ?? Default.AnimKitSegment.AnimStartTime,
                    BlendInTimeMs = segment.BlendInTimeMs ?? Default.AnimKitSegment.BlendInTimeMs,
                    BlendOutTimeMs = segment.BlendOutTimeMs ?? Default.AnimKitSegment.BlendOutTimeMs,
                    EndCondition = segment.EndCondition ?? Default.AnimKitSegment.EndCondition,
                    EndConditionDelay = segment.EndConditionDelay ?? Default.AnimKitSegment.EndConditionDelay,
                    EndConditionParam = segment.EndConditionParam ?? Default.AnimKitSegment.EndConditionParam,
                    OrderIndex = segment.OrderIndex ?? Default.AnimKitSegment.OrderIndex,
                    OverrideConfigFlags = segment.OverrideConfigFlags ?? Default.AnimKitSegment.OverrideConfigFlags,
                    SegmentFlags = segment.SegmentFlags ?? Default.AnimKitSegment.SegmentFlags,
                    Speed = segment.Speed ?? Default.AnimKitSegment.Speed,
                    StartCondition = segment.StartCondition ?? Default.AnimKitSegment.StartCondition,
                    StartConditionDelay = segment.StartConditionDelay ?? Default.AnimKitSegment.StartConditionDelay,
                    StartConditionParam = segment.StartConditionParam ?? Default.AnimKitSegment.StartConditionParam,
                    LoopToSegmentIndex = segment.LoopToSegmentIndex ?? Default.AnimKitSegment.LoopToSegmentIndex,
                    ForcedVariation = segment.ForcedVariation ?? Default.AnimKitSegment.ForcedVariation
                });
            }
            return result.ToArray();
        }
    }
}
