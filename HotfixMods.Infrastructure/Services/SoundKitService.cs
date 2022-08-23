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

        public async Task<List<DashboardModel>> GetSoundKitDashboardAsync()
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

        public async Task<List<SoundKitDto>> GetSoundKitById(int soundKitId, Action<string, string, int>? progressCallback = null)
        {
            var soundKit = await _db2.GetAsync<SoundKit>(s => s.Id == soundKitId);
            if (null == soundKit)
            {
                return new ();
            }
            var soundKitEntries = await _db2.GetManyAsync<SoundKitEntry>(s => s.SoundKitId == soundKitId);
            if (!soundKitEntries.Any())
            {
                return new();
            }

            var result = new SoundKitDto()
            {
                Id = await GetNextIdAsync(),
                FileDataIds = new(),
                PitchAdjust = soundKit.PitchAdjust,
                PitchVariation = soundKit.PitchVariationPlus,
                VolumeAdjust = soundKit.VolumeFloat,
                VolumeVariation = soundKit.VolumeVariationPlus,
                SoundType = soundKit.SoundType
            };
            foreach(var soundKitEntry in soundKitEntries)
            {
                result.FileDataIds.Add(soundKitEntry.FileDataId);
            }
            return new List<SoundKitDto>() { result };
        }

        public async Task DeleteSoundKitAsync(int id)
        {
            await DeleteFromHotfixes(id);
        }

        public async Task SaveSoundKitAsync(SoundKitDto soundKit, Action<string, string, int>? progressCallback = null)
        {
            if (soundKit.FileDataIds.Count > 20)
            {
                /*
                 * Adding more than 10 SoundKitEntries will cause conflicts with the next SoundKit.
                 * If this number is increased, you need to make appropriate changes everywhere in code.
                 */
                throw new Exception($"SoundKit should not have more than 20 SoundKitEntries (aka FileDataIds).");
            }

            var hotfixId = await GetNextHotfixIdAsync();
            soundKit.InitHotfixes(hotfixId, VerifiedBuild);

            if (soundKit.IsUpdate)
            {
                await _mySql.UpdateAsync(BuildHotfixModsData(soundKit));
                await _mySql.UpdateAsync(BuildSoundKit(soundKit));

                var entries = await _mySql.GetManyAsync<SoundKitEntry>(c => c.SoundKitId == soundKit.Id);
                if (entries.Any())
                    await _mySql.DeleteManyAsync(entries);
                await _mySql.AddManyAsync(BuildSoundKitEntry(soundKit));
            }
            else
            {
                await _mySql.AddAsync(BuildHotfixModsData(soundKit));
                await _mySql.AddAsync(BuildSoundKit(soundKit));
                await _mySql.AddManyAsync(BuildSoundKitEntry(soundKit));
            }

            await _mySql.AddManyAsync(soundKit.GetHotfixes());
        }

        async Task DeleteFromHotfixes(int id)
        {
            var soundKit = await _mySql.GetAsync<SoundKit>(s => s.Id == id);
            var soundKitEntries = await _mySql.GetManyAsync<SoundKitEntry>(s => s.SoundKitId == id);
            var hotfixModsData = await _mySql.GetAsync<HotfixModsData>(h => h.Id == id);

            if (null != soundKit)
                await _mySql.DeleteAsync(soundKit);

            if (soundKitEntries.Any())
                await _mySql.DeleteManyAsync(soundKitEntries);

            var hotfixData = await _mySql.GetManyAsync<HotfixData>(h => h.UniqueId == id);
            if (hotfixData != null && hotfixData.Count() > 0)
            {
                foreach (var hotfix in hotfixData)
                {
                    hotfix.Status = HotfixStatuses.INVALID;
                }
                await _mySql.UpdateManyAsync(hotfixData);
            }

            if (null != hotfixModsData)
                await _mySql.DeleteAsync(hotfixModsData);
        }
    }
}
