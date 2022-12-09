using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public class SpellVisualKitService : Service
    {
        public SpellVisualKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }
        public async Task<SpellVisualKitDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var result = new SpellVisualKitDto();
            result.Entity.RecordId = await GetNextIdAsync();

            return result;
        }

        public async Task<SpellVisualKitDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var spellVisualKit = await GetSingleByIdAsync<SpellVisualKit>(id);
            if (null == spellVisualKit)
            {
                return null;
            }
            return new SpellVisualKitDto()
            {
                SpellVisualKit = spellVisualKit,
                Entity = await GetHotfixModsEntity(spellVisualKit.Id)
            };
        }

        public async Task SaveAsync(SpellVisualKitDto spellVisualKitDto)
        {
            await SaveAsync(spellVisualKitDto.SpellVisualKit);
            await SaveAsync(spellVisualKitDto.Entity);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteAsync<SpellVisualKit>(new DbParameter(nameof(AnimKit.Id), id));
            // TODO: HotfixMods
        }

        public async Task<int> GetNextIdAsync()
        {
            return await GetNextIdAsync<SpellVisualKit>();
        }
    }
}
