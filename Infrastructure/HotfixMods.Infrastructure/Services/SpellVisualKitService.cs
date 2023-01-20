using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public class SpellVisualKitService : ServiceBase
    {
        public SpellVisualKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }
        public async Task<SpellVisualKitDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var result = new SpellVisualKitDto();
            result.HotfixModsEntity.RecordId = await GetNextIdAsync();

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
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(spellVisualKit.Id)
            };
        }

        public async Task<bool> SaveAsync(SpellVisualKitDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            await SaveAsync(dto.SpellVisualKit);
            await SaveAsync(dto.HotfixModsEntity);

            dto.IsUpdate = true;
            return true;
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
