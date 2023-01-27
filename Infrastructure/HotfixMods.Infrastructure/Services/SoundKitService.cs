using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SoundKitService : ServiceBase
    {
        public SoundKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }
        public async Task<SoundKitDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            return new SoundKitDto();
        }

        public async Task<SoundKitDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var soundKit = await GetSingleByIdAsync<SoundKit>(id);
            if (null == soundKit)
            {
                return null;
            }

            var result = new SoundKitDto()
            {
                SoundKit = soundKit,
                EntryGroups = new(),
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(soundKit.Id),
                IsUpdate = true
            };

            var soundKitEntries = await GetAsync<SoundKitEntry>(new DbParameter(nameof(SoundKitEntry.SoundKitId), id));
            foreach (var soundKitEntry in soundKitEntries)
            {
                result.EntryGroups.Add(new()
                {
                    SoundKitEntry = soundKitEntry
                });
            }

            return result;
        }

        public async Task<bool> SaveAsync(SoundKitDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            await SetIdAndVerifiedBuild(dto);

            await SaveAsync(dto.HotfixModsEntity);
            await SaveAsync(dto.SoundKit);
            await SaveAsync(dto.EntryGroups.Select(s => s.SoundKitEntry));

            dto.IsUpdate = true;
            return true;
        }

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var dto = await GetByIdAsync(id);
            if(null == dto)
            {
                return false;
            }

            await dto.EntryGroups.ForEachAsync(async s =>
            {
                await DeleteAsync(s);
            });
            await DeleteAsync(dto.SoundKit);
            await DeleteAsync(dto.HotfixModsEntity);

            return true;
        }
    }
}
