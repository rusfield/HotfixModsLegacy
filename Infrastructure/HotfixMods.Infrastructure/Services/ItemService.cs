using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using System.Text.Json;

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
            int currentInvoke = 1;
            Func<int> increaseProgress = () => currentInvoke++ * 100 / 10; // divide by total invokes

            callback.Invoke("Loading", $"Loading {nameof(Item)}", increaseProgress());
            var item = await GetSingleByIdAsync<Item>(id);
            if (null == item)
            {
                return null;
            }

            var result = new ItemDto()
            {
                Item = item,
            };

            callback.Invoke("Loading", $"Loading {nameof(HotfixModsEntity)}", increaseProgress());
            result.HotfixModsEntity = await GetSingleAsync<HotfixModsEntity>(new DbParameter(nameof(HotfixModsEntity.RecordId), id), new DbParameter(nameof(HotfixModsEntity.VerifiedBuild), VerifiedBuild)) ?? new();

            callback.Invoke("Loading", $"Loading {nameof(ItemSparse)}", increaseProgress());
            result.ItemSparse = await GetSingleAsync<ItemSparse>(new DbParameter(nameof(ItemSparse.Id), id));

            callback.Invoke("Loading", $"Loading {nameof(ItemModifiedAppearance)}", increaseProgress());
            result.ItemModifiedAppearance= await GetSingleAsync<ItemModifiedAppearance>(new DbParameter(nameof(ItemModifiedAppearance.ItemId), id));

            callback.Invoke("Loading", $"Loading {nameof(ItemXItemEffect)}", increaseProgress());
            var itemXItemEffect = await GetAsync<ItemXItemEffect>(new DbParameter(nameof(ItemSparse.Id), id));

            callback.Invoke("Loading", $"Loading {nameof(ItemEffect)}", increaseProgress());
            itemXItemEffect.ForEach(async i =>
            {
                var itemEffect = await GetSingleAsync<ItemEffect>(new DbParameter(nameof(ItemEffect.Id), i.ItemEffectId));
                if (itemEffect != null)
                    result.EffectGroups.Add(new()
                    {
                        ItemEffect = itemEffect
                    });
            });

            if (result.ItemModifiedAppearance != null)
            {
                callback.Invoke("Loading", $"Loading {nameof(ItemAppearance)}", increaseProgress());
                result.ItemAppearance = await GetSingleAsync<ItemAppearance>(new DbParameter(nameof(ItemAppearance.Id), result.ItemModifiedAppearance.ItemAppearanceId));
                if (result.ItemAppearance != null)
                {
                    callback.Invoke("Loading", $"Loading {nameof(ItemDisplayInfo)}", increaseProgress());
                    result.ItemDisplayInfo= await GetSingleAsync<ItemDisplayInfo>(new DbParameter(nameof(ItemDisplayInfo.Id), result.ItemAppearance.ItemDisplayInfoId));
                    if (result.ItemDisplayInfo != null)
                    {
                        callback.Invoke("Loading", $"Loading {nameof(ItemDisplayInfoMaterialRes)}", increaseProgress());
                        var itemDisplayInfoMaterialRes = await GetAsync<ItemDisplayInfoMaterialRes>(new DbParameter(nameof(ItemDisplayInfoMaterialRes.ItemDisplayInfoId), result.ItemDisplayInfo));
                        result.ItemDisplayInfoMaterialRes = itemDisplayInfoMaterialRes.Count > 0 ? result.ItemDisplayInfoMaterialRes : null;
                    }
                }
            }

            callback.Invoke("Loading", "Loading successful.", 100);
            return result;
        }

        public async Task<bool> SaveAsync(ItemDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            int currentInvoke = 1;
            Func<int> increaseProgress = () => currentInvoke++ * 100 / 12; // divide by total invokes

            // Copy values from ItemSparse
            var itemSearchName = dto.ItemSparse != null ? JsonSerializer.Deserialize<ItemSearchName>(JsonSerializer.Serialize(dto.ItemSparse)) : null;

            var itemXItemEffects = await GetAsync<ItemXItemEffect>(new DbParameter(nameof(ItemXItemEffect.ItemId), dto.Item.Id));

            try
            {
                callback.Invoke("Saving", "Deleting existing data", increaseProgress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync(dto.Item.Id);
                }

                callback.Invoke("Saving", "Preparing ID", increaseProgress());
                await SetIdAndVerifiedBuild(dto, itemXItemEffects, itemSearchName);

                callback.Invoke("Saving", $"Saving to {nameof(Item)}", increaseProgress());
                await SaveAsync(dto.Item);


                if (dto.ItemSparse != null)
                {
                    await SaveAsync(dto.ItemSparse);
                }

                if (itemSearchName != null)
                {
                    await SaveAsync(itemSearchName);
                }

                if (dto.ItemModifiedAppearance != null)
                {
                    await SaveAsync(dto.ItemModifiedAppearance);

                    if (dto.ItemAppearance != null)
                    {
                        await SaveAsync(dto.ItemAppearance);

                        if (dto.ItemDisplayInfo != null)
                        {
                            await SaveAsync(dto.ItemDisplayInfo);

                            if (dto.ItemDisplayInfoMaterialRes?.Any() ?? false)
                                await SaveAsync(dto.ItemDisplayInfoMaterialRes);
                        }
                    }
                }

                if (dto.EffectGroups.Any())
                {
                    await SaveAsync(dto.EffectGroups.Select(s => s.ItemEffect));
                    await SaveAsync(itemXItemEffects);
                }

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
            var dto = await GetByIdAsync(id);
            var itemSearchName = await GetSingleAsync<ItemSearchName>(new DbParameter(nameof(ItemSearchName.Id), dto.Item.Id));
            var itemXItemEffects = await GetAsync<ItemXItemEffect>(new DbParameter(nameof(ItemXItemEffect.ItemId), dto.Item.Id));

            await DeleteAsync(itemSearchName);
            await DeleteAsync(itemXItemEffects);
            await DeleteAsync(dto.ItemDisplayInfoMaterialRes);
            await DeleteAsync(dto.ItemDisplayInfo);
            await DeleteAsync(dto.ItemAppearance);
            await DeleteAsync(dto.ItemModifiedAppearance);
            await DeleteAsync(dto.ItemSparse);
            await DeleteAsync(dto.Item);
            await DeleteAsync(dto.HotfixModsEntity);

        }

        public async Task<int> GetNextIdAsync()
        {
            return await GetNextIdAsync<Item>();
        }
    }
}
