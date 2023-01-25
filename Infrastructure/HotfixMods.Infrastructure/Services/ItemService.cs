using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService : ServiceBase
    {
        public ItemService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public async Task<ItemDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var result = new ItemDto();
            result.HotfixModsEntity.RecordId = await GetNextIdAsync();

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

        public async Task<bool> SaveAsync(ItemDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            int currentInvoke = 1;
            Func<int> increaseProgress = () => currentInvoke++ * 100 / 12; // divide by total invokes

            try
            {
                /*
                callback.Invoke("Saving", "Preparing ID", increaseProgress());
                await SetIdAndVerifiedBuild(dto);

                callback.Invoke("Saving", $"Saving {nameof(HotfixModsEntity)}", increaseProgress());
                await SaveAsync(dto.HotfixModsEntity);

                callback.Invoke("Saving", $"Saving {nameof(CreatureTemplate)}", increaseProgress());
                await SaveAsync(dto.CreatureTemplate);

                callback.Invoke("Saving", $"Saving {nameof(CreatureDisplayInfo)}", increaseProgress());
                await SaveAsync(dto.CreatureDisplayInfo);

                callback.Invoke("Saving", $"Saving {nameof(CreatureEquipTemplate)}", increaseProgress());
                await SaveAsync(dto.CreatureEquipTemplate);

                callback.Invoke("Saving", $"Saving {nameof(CreatureTemplateAddon)}", increaseProgress());
                await SaveAsync(dto.CreatureTemplateAddon);

                callback.Invoke("Saving", $"Saving {nameof(CreatureModelInfo)}", increaseProgress());
                await SaveAsync(dto.CreatureModelInfo);

                callback.Invoke("Saving", $"Saving {nameof(CreatureDisplayInfoExtra)}", increaseProgress());
                await SaveAsync(dto.CreatureDisplayInfoExtra);

                callback.Invoke("Saving", $"Saving {nameof(CreatureTemplateModel)}", increaseProgress());
                await SaveAsync(dto.CreatureTemplateModel);

                callback.Invoke("Saving", $"Saving {nameof(CreatureDisplayInfoOption)}", increaseProgress());
                await SaveAsync(dto.CreatureDisplayInfoOption.ToArray());

                callback.Invoke("Saving", $"Saving {nameof(NpcModelItemSlotDisplayInfo)}", increaseProgress());
                await SaveAsync(dto.NpcModelItemSlotDisplayInfo.ToArray());
                */
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                return false;
            }

            callback.Invoke("Saving", "Saving successful.", 100);
            dto.IsUpdate = true;


            return true;
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
