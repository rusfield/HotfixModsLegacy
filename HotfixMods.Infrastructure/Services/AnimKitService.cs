using HotfixMods.Core.Enums;
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
            var hotfixModsData = await _mySql.GetAsync<HotfixModsData>(c => c.VerifiedBuild == VerifiedBuild);
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
            await DeleteFromHotfixesAsync(id);
        }

        public async Task SaveAsync(AnimKitDto animKitDto)
        {
            if (animKitDto.Segments.Count > 20)
            {
                throw new Exception($"AnimKit should not have more than 50 Segments.");
            }


            var hotfixId = await GetNextHotfixIdAsync();
            animKitDto.InitHotfixes(hotfixId, VerifiedBuild);

            if (animKitDto.IsUpdate)
            {
                await _mySql.AddOrUpdateAsync(BuildHotfixModsData(animKitDto));
                await _mySql.AddOrUpdateAsync(BuildAnimKit(animKitDto));

                var segments = await _mySql.GetAsync<AnimKitSegment>(c => c.ParentAnimKitId == animKitDto.Id);
                if (segments.Any())
                    await _mySql.DeleteAsync(segments.ToArray());
                await _mySql.AddOrUpdateAsync(BuildAnimKitSegment(animKitDto));
            }
            else
            {
                await _mySql.AddOrUpdateAsync(BuildHotfixModsData(animKitDto));
                await _mySql.AddOrUpdateAsync(BuildAnimKit(animKitDto));
                await _mySql.AddOrUpdateAsync(BuildAnimKitSegment(animKitDto));
            }

            await AddHotfixes(animKitDto.GetHotfixes());
        }

        public async Task<AnimKitDto> GetNewAnimKitAsync(Action<string, string, int>? progressCallback = null)
        {
            if (progressCallback == null)
                progressCallback = ConsoleProgressCallback;

            progressCallback("Done", "Returning new Anim Kit", 100);
            return new AnimKitDto()
            {
                Id = await GetNextIdAsync(),
                Segments = new() { new() { OrderIndex = 0 } }
            };
        }

        public async Task<AnimKitDto?> GetAnimKitsByIdAsync(int animKitId, Action<string, string, int>? progressCallback = null)
        {
            if (progressCallback == null)
                progressCallback = ConsoleProgressCallback;

            progressCallback("Loading", "Loading Anim Kit", 15);
            var animKit = await _db2.GetSingleAsync<AnimKit>(a => a.Id == animKitId);
            if(animKit == null)
            {
                progressCallback("Error", "Anim Kit not found", 100);
                return null;
            }
            var animKitSegments = await _db2.GetAsync<AnimKitSegment>(a => a.ParentAnimKitId == animKitId);
            if (!animKitSegments.Any())
            {
                progressCallback("Error", "Anim Kit Segments not found", 100);
                return null;
            }

            progressCallback("Loading", "Loading Hotfix Mods Data", 50);
            var hmData = await _mySql.GetSingleAsync<HotfixModsData>(c => c.RecordId == animKitId && c.VerifiedBuild == VerifiedBuild);

            progressCallback("Loading", "Building Anim Kit", 90);
            var result = new AnimKitDto()
            {
                Id = await GetNextIdAsync(),
                OneShotStopAnimKitId = animKit.OneShotStopAnimKitId,
                OneShotDuration = animKit.OneShotDuration,
                HotfixModsName = hmData?.Name,
                HotfixModsComment = hmData?.Comment,
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

            progressCallback("Done", "Returning Anim Kit", 100);
            return result;
        }

        async Task DeleteFromHotfixesAsync(int id)
        {
            var animKit = await _mySql.GetSingleAsync<AnimKit>(s => s.Id == id);
            var animKitSegments = await _mySql.GetAsync<AnimKitSegment>(s => s.ParentAnimKitId == id);
            var hotfixModsData = await _mySql.GetSingleAsync<HotfixModsData>(h => h.Id == id && h.VerifiedBuild == VerifiedBuild);

            if (null != animKit)
                await _mySql.DeleteAsync(animKit);

            if (animKitSegments.Any())
                await _mySql.DeleteAsync(animKitSegments.ToArray());

            var hotfixData = await _mySql.GetAsync<HotfixData>(h => h.UniqueId == id);
            if (hotfixData != null && hotfixData.Count() > 0)
            {
                foreach (var hotfix in hotfixData)
                {
                    hotfix.Status = HotfixStatuses.INVALID;
                }
                await _mySql.AddOrUpdateAsync(hotfixData.ToArray());
            }

            if (null != hotfixModsData)
                await _mySql.DeleteAsync(hotfixModsData);
        }
    }
}
