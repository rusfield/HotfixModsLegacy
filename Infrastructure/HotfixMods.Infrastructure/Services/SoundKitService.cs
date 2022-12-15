using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public class SoundKitService : ServiceBase
    {
        public SoundKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }
        public async Task<SoundKitDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var result = new SoundKitDto();
            result.Entity.RecordId = await GetNextIdAsync();

            return result;
        }

        public async Task<SoundKitDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var soundKit = await GetSingleByIdAsync<SoundKit>(id);
            if (null == soundKit)
            {
                return null;
            }
            return new SoundKitDto()
            {
                SoundKit = soundKit,
                EntryGroups = new(),
                Entity = await GetHotfixModsEntity(soundKit.Id)
            };
        }

        public async Task SaveAsync(SoundKitDto soundKitDto)
        {
            await SaveAsync(soundKitDto.SoundKit);
            //await SaveAsync(soundKitDto.SoundKitEntries.ToArray()); // TODO: Fix
            await SaveAsync(soundKitDto.Entity);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteAsync<SoundKit>(new DbParameter(nameof(AnimKit.Id), id));
            await DeleteAsync<SoundKitEntry>(new DbParameter(nameof(SoundKitEntry.SoundKitId), id));
            // TODO: HotfixMods
        }

        public async Task<int> GetNextIdAsync()
        {
            return await GetNextIdAsync<SoundKit>();
        }
    }
}
