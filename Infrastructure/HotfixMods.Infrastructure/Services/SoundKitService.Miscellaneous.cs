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
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.Id, dto.IsUpdate);
            var soundKitId = await GetIdByConditionsAsync<SoundKit>(dto.SoundKit.Id, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextSoundKitEntryId = await GetNextIdAsync<SoundKitEntry>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.Id = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordId = soundKitId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.SoundKit.Id = soundKitId;

            dto.EntryGroups.ForEach(g =>
            {
                g.SoundKitEntry.Id = nextSoundKitEntryId++;
                g.SoundKitEntry.SoundKitId = soundKitId;
                g.SoundKitEntry.VerifiedBuild = VerifiedBuild;
            });
        }
    }
}
