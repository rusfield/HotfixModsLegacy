using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SoundKitService
    {
        async Task SetIdAndVerifiedBuild(SoundKitDto soundKitDto)
        {
            if (!soundKitDto.IsUpdate)
            {
                var newSoundKitId = await GetNextIdAsync<SoundKit>();
                var newSoundKitEntryId = await GetNextIdAsync<SoundKitEntry>();

                soundKitDto.Entity.Id = await GetNextIdAsync<HotfixModsEntity>();
                soundKitDto.Entity.RecordId = newSoundKitId;
                soundKitDto.SoundKit.Id = newSoundKitId;

                soundKitDto.EntryGroups.ForEach(s =>
                {
                    s.SoundKitEntry.SoundKitId = (uint)newSoundKitId;
                    s.SoundKitEntry.Id = newSoundKitEntryId;

                    newSoundKitEntryId++;
                });
            }

            soundKitDto.Entity.VerifiedBuild = VerifiedBuild;
            soundKitDto.SoundKit.VerifiedBuild = VerifiedBuild;
            soundKitDto.EntryGroups.ForEach(s =>
            {
                s.SoundKitEntry.VerifiedBuild = VerifiedBuild;
            });
        }
    }
}
