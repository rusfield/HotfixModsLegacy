using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public class ItemService : ServiceBase
    {
        public ItemService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public async Task<ItemDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var result = new ItemDto();
            result.Entity.RecordId = await GetNextIdAsync();

            return result;
        }

        public async Task<ItemDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var item = await GetSingleByIdAsync<Item>(id);
            if (null == item)
            {
                return null;
            }

            // TODO
            return null;
        }

        public async Task SaveAsync(AnimKitDto animKitDto)
        {
            // TODO
        }

        public async Task DeleteAsync(int id)
        {
            // TODO
        }

        public async Task<int> GetNextIdAsync()
        {
            return await GetNextIdAsync<Item>();
        }
    }
}
