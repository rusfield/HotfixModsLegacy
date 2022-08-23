using HotfixMods.Core.Models;
using HotfixMods.Core.Providers;
using HotfixMods.Infrastructure.Dashboard;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.DtoModels.AnimKits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService : Service
    {
        public AnimKitService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider) { }

        public async Task<List<DashboardModel>> GetDashboardAsync()
        {
            var hotfixModsData = await _mySql.GetManyAsync<HotfixModsData>(c => c.VerifiedBuild == VerifiedBuild);
            var result = new List<DashboardModel>();
            foreach (var data in hotfixModsData)
            {
                result.Add(new DashboardModel()
                {
                    Id = data.RecordId,
                    Name = data.Name,
                    Comment = data.Comment,
                    AvatarUrl = "TODO"
                });
            }
            // Newest on top
            result.Reverse();
            return result;
        }

        public async Task DeleteAsync(int id)
        {

        }

        public async Task SaveAsync(AnimKitDto animKitDto)
        {

        }

        public async Task<List<AnimKitDto>> GetAnimKitsByIdAsync(int animKitId, Action<string, string, int>? progressCallback = null)
        {
            var animKit = await _db2.GetAsync<AnimKit>(a => a.Id == animKitId);
            if(animKit == null)
            {
                return new();
            }
            var animKitSegments = await _db2.GetManyAsync<AnimKitSegment>(a => a.ParentAnimKitId == animKitId);
            if (!animKitSegments.Any())
            {
                return new();
            }
            
            var result = new AnimKitDto()
            {
                Id = await GetNextIdAsync(),
                OneShotStopAnimKitId = animKit.OneShotStopAnimKitId,
                OneShotDuration = animKit.OneShotDuration,
                Segments = new()
            };

            foreach(var segment in animKitSegments)
            {
                result.Segments.Add(new AnimKitSegmentDto()
                {
                    AnimId = segment.AnimId,
                    AnimKitConfigId = segment.AnimKitConfigId,
                    AnimStartTime = segment.AnimStartTime,
                    BlendInTimeMs = segment.BlendInTimeMs,
                    BlendOutTimeMs = segment.BlendOutTimeMs,
                    EndCondition = segment.EndCondition,
                    EndConditionDelay = segment.EndConditionDelay,
                    EndConditionParam = segment.EndConditionParam,
                    OrderIndex = segment.OrderIndex,
                    OverrideConfigFlags = segment.OverrideConfigFlags,
                    SegmentFlags = segment.SegmentFlags,
                    Speed = segment.Speed,
                    StartCondition = segment.StartCondition,
                    StartConditionDelay = segment.StartConditionDelay,
                    StartConditionParam = segment.StartConditionParam
                });
            }

            return new() { result };
        }
    }
}
