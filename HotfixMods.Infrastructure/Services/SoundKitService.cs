using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Core.Providers;
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
        public SoundKitService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider)  {   }

        public async Task SaveSoundAsync(SoundKitDto soundKit)
        {
            if(soundKit.FileDataIds.Count > 10)
            {
                /*
                 * Adding more than 10 SoundKitEntries will cause conflicts with the next SoundKit.
                 * If this number is increased, you need to make appropriate changes everywhere in code.
                 */
                throw new Exception($"SoundKit should not have more than 10 SoundKitEntries (aka FileDataIds).");
            }

            HotfixModsData hotfixModsData;
            var hotfixId = await GetNextHotfixIdAsync();
            soundKit.InitHotfixes(hotfixId, VerifiedBuild);

            if (soundKit.IsUpdate)
            {
                await DeleteFromHotfixes(soundKit.Id);
                await _mySql.UpdateAsync(BuildHotfixModsData(soundKit));
            }
            else
            {
                await _mySql.AddAsync(BuildHotfixModsData(soundKit));
            }

            await _mySql.AddAsync(BuildSoundKit(soundKit));
            await _mySql.AddManyAsync(BuildSoundKitEntry(soundKit));

            await _mySql.AddManyAsync(soundKit.GetHotfixes());
        }

        async Task DeleteFromHotfixes(int id)
        {
            var soundKit = await _mySql.GetAsync<SoundKit>(s => s.Id == id);
            var soundKitEntries = await _mySql.GetManyAsync<SoundKitEntry>(s => s.SoundKitId == id);

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
        }
    }
}
