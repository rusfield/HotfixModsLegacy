using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SoundKitService
    {
        async Task SetIdAndVerifiedBuild(SoundKitDto dto)
        {
            // Step 1: Init IDs of single entities
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            var soundKitId = await GetIdByConditionsAsync<SoundKit>((ulong)dto.SoundKit.ID, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextSoundKitEntryId = await GetNextIdAsync<SoundKitEntry>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = soundKitId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.SoundKit.ID = (int)soundKitId;

            dto.EntryGroups.ForEach(g =>
            {
                g.SoundKitEntry.ID = (int)nextSoundKitEntryId++;
                g.SoundKitEntry.SoundKitID = (uint)soundKitId;
                g.SoundKitEntry.VerifiedBuild = VerifiedBuild;
            });
        }
    }
}
