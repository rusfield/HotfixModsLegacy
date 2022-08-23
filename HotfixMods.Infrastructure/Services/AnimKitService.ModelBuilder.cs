using HotfixMods.Core.Constants;
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

                OneShotDuration = animKitDto.OneShotDuration ?? AnimKitDefaults.OneShotDuration,
                OneShotStopAnimKitId = animKitDto.OneShotStopAnimKitId ?? AnimKitDefaults.OneShotStopAnimKitId
            };
        } 

        List<AnimKitSegment> BuildAnimKitSegment(AnimKitDto animKitDto)
        {
            var result = new List<AnimKitSegment>();
            foreach(var segment in animKitDto.Segments)
            {
                int id = animKitDto.Id + (segment.OrderIndex ?? AnimKitDefaults.OrderIndex);
                animKitDto.AddHotfix(id, TableHashes.AnimKitSegment, HotfixStatuses.VALID);
                result.Add(new AnimKitSegment()
                {
                    Id = id,
                    ParentAnimKitId = animKitDto.Id,
                    VerifiedBuild = VerifiedBuild,
                    
                    AnimId = segment.AnimId ?? AnimKitDefaults.AnimId,
                    AnimKitConfigId = segment.AnimKitConfigId ?? AnimKitDefaults.AnimKitConfigId,
                    AnimStartTime = segment.AnimStartTime ?? AnimKitDefaults.AnimStartTime,
                    BlendInTimeMs = segment.BlendInTimeMs ?? AnimKitDefaults.BlendInTimeMs,
                    BlendOutTimeMs = segment.BlendOutTimeMs ?? AnimKitDefaults.BlendOutTimeMs,
                    EndCondition = segment.EndCondition ?? AnimKitDefaults.EndCondition,
                    EndConditionDelay = segment.EndConditionDelay ?? AnimKitDefaults.EndConditionDelay,
                    EndConditionParam = segment.EndConditionParam ?? AnimKitDefaults.EndConditionParam,
                    OrderIndex = segment.OrderIndex ?? AnimKitDefaults.OrderIndex,
                    OverrideConfigFlags = segment.OverrideConfigFlags ?? AnimKitDefaults.OverrideConfigFlags,
                    SegmentFlags = segment.OverrideConfigFlags ?? AnimKitDefaults.OverrideConfigFlags,
                    Speed = segment.Speed ?? AnimKitDefaults.Speed,
                    StartCondition = segment.StartCondition ?? AnimKitDefaults.StartCondition,
                    StartConditionDelay = segment.StartConditionDelay ?? AnimKitDefaults.StartConditionDelay,
                    StartConditionParam = segment.StartConditionParam ?? AnimKitDefaults.StartConditionParam,

                    ForcedVariation = AnimKitDefaults.ForcedVariation,
                    LoopToSegmentIndex = AnimKitDefaults.LoopToSegmentIndex
                });
            }
            return result;
        }
    }
}
