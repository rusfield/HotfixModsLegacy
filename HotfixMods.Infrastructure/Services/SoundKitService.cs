using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Core.Providers;
using HotfixMods.Infrastructure.Dashboard;
using HotfixMods.Infrastructure.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SoundKitService : Service
    {
        public SoundKitService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider) { }

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

        public async Task<SoundKitDto> GetNewSoundKit(Action<string, string, int>? progressCallback = null)
        {
            return new SoundKitDto()
            {
                Id = await GetNextIdAsync(),
                FileDataIds = new()
            };
        }

        public async Task<SoundKitDto> GetSoundKitById(int soundKitId, Action<string, string, int>? progressCallback = null)
        {
            var soundKit = await _mySql.GetSingleAsync<SoundKit>(s => s.Id == soundKitId) ?? await _db2.GetAsync<SoundKit>(s => s.Id == soundKitId);
            if (null == soundKit)
            {
                return new();
            }
            var soundKitEntries = await _mySql.GetAsync<SoundKitEntry>(s => s.SoundKitId == soundKitId);
            if (!soundKitEntries.Any())
                soundKitEntries = await _db2.GetManyAsync<SoundKitEntry>(s => s.SoundKitId == soundKitId);

            if (!soundKitEntries.Any())
            {
                return new();
            }

            var hmData = await _mySql.GetSingleAsync<HotfixModsData>(h => h.RecordId == soundKitId);

            var result = new SoundKitDto()
            {
                Id = hmData != null ? hmData.RecordId : await GetNextIdAsync(),
                FileDataIds = new(),
                PitchAdjust = soundKit.PitchAdjust,
                PitchVariation = soundKit.PitchVariationPlus,
                VolumeAdjust = soundKit.VolumeFloat,
                VolumeVariation = soundKit.VolumeVariationPlus,
                SoundType = soundKit.SoundType,
                HotfixModsComment = hmData?.Comment,
                HotfixModsName = hmData?.Name,
                IsUpdate = hmData != null
            };
            foreach (var soundKitEntry in soundKitEntries)
            {
                result.FileDataIds.Add(soundKitEntry.FileDataId);
            }
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteFromHotfixesAsync(id);
        }

        public async Task SaveAsync(SoundKitDto soundKit, Action<string, string, int>? progressCallback = null)
        {
            if (soundKit.FileDataIds.Count > 20)
            {
                throw new Exception($"SoundKit should not have more than 20 SoundKitEntries (aka FileDataIds).");
            }

            var hotfixId = await GetNextHotfixIdAsync();
            soundKit.InitHotfixes(hotfixId, VerifiedBuild);

            await _mySql.AddOrUpdateAsync(BuildHotfixModsData(soundKit));
            await _mySql.AddOrUpdateAsync(BuildSoundKit(soundKit));

            var entries = await _mySql.GetAsync<SoundKitEntry>(c => c.SoundKitId == soundKit.Id);
            if (entries.Any())
                await _mySql.DeleteAsync(entries.ToArray());
            await _mySql.AddOrUpdateAsync(BuildSoundKitEntry(soundKit));

            await _mySql.AddOrUpdateAsync(soundKit.GetHotfixes().ToArray());
        }

        async Task DeleteFromHotfixesAsync(int id)
        {
            var soundKit = await _mySql.GetSingleAsync<SoundKit>(s => s.Id == id);
            var soundKitEntries = await _mySql.GetAsync<SoundKitEntry>(s => s.SoundKitId == id);
            var hotfixModsData = await _mySql.GetSingleAsync<HotfixModsData>(h => h.Id == id);

            if (null != soundKit)
                await _mySql.DeleteAsync(soundKit);

            if (soundKitEntries.Any())
                await _mySql.DeleteAsync(soundKitEntries.ToArray());

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
