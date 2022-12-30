using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SoundKitService
    {
        async Task SetIdAndVerifiedBuild(SoundKitDto dto)
        {
            if (!dto.IsUpdate)
            {
                var newSoundKitId = await GetNextIdAsync<SoundKit>();
                var newSoundKitEntryId = await GetNextIdAsync<SoundKitEntry>();

                dto.HotfixModsEntity.Id = await GetNextIdAsync<HotfixModsEntity>();
                dto.HotfixModsEntity.RecordId = newSoundKitId;
                dto.SoundKit.Id = newSoundKitId;

                dto.EntryGroups.ForEach(s =>
                {
                    s.SoundKitEntry.SoundKitId = (uint)newSoundKitId;
                    s.SoundKitEntry.Id = newSoundKitEntryId;

                    newSoundKitEntryId++;
                });
            }

            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;
            dto.SoundKit.VerifiedBuild = VerifiedBuild;
            dto.EntryGroups.ForEach(s =>
            {
                s.SoundKitEntry.VerifiedBuild = VerifiedBuild;
            });
        }
    }
}
