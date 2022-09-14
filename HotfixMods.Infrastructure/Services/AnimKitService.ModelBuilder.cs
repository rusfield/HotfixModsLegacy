using HotfixMods.Core.Constants;
using HotfixMods.Infrastructure.DefaultModels;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService
    {
        AnimKit BuildAnimKit(AnimKitDto animKitDto)
        {
            animKitDto.AddHotfix(animKitDto.Id, TableHashes.AnimKit, HotfixStatuses.VALID);
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
                animKitDto.AddHotfix(id, TableHashes.AnimKitSegment, HotfixStatuses.VALID);
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
                    SegmentFlags = segment.OverrideConfigFlags ?? Default.AnimKitSegment.OverrideConfigFlags,
                    Speed = segment.Speed ?? Default.AnimKitSegment.Speed,
                    StartCondition = segment.StartCondition ?? Default.AnimKitSegment.StartCondition,
                    StartConditionDelay = segment.StartConditionDelay ?? Default.AnimKitSegment.StartConditionDelay,
                    StartConditionParam = segment.StartConditionParam ?? Default.AnimKitSegment.StartConditionParam,

                    ForcedVariation = Default.AnimKitSegment.ForcedVariation,
                    LoopToSegmentIndex = Default.AnimKitSegment.LoopToSegmentIndex
                });
            }
            return result.ToArray();
        }
    }
}
