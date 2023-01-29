using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Helpers;
using System.Text.Json;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService : ServiceBase
    {
        public ItemService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public ItemDto GetNew(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            callback.Invoke(LoadingHelper.Loading, "Returning new template", 100);
            return new();
        }

        public async Task<ItemDto?> GetByIdAsync(uint id, int modifiedAppearanceOrderIndex = 0, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var increaseProgress = LoadingHelper.GetLoaderFunc(9);

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(Item)}", increaseProgress());
            var item = await GetSingleAsync<Item>(new DbParameter(nameof(Item.Id), id));
            if (null == item)
            {
                callback.Invoke(LoadingHelper.Loading, "Not found", 100);
                return null;
            }

            var result = new ItemDto()
            {
                Item = item,
            };

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(HotfixModsEntity)}", increaseProgress());
            result.HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(item.Id);

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(ItemSparse)}", increaseProgress());
            result.ItemSparse = await GetSingleAsync<ItemSparse>(new DbParameter(nameof(ItemSparse.Id), id));

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(ItemModifiedAppearance)}", increaseProgress());
            result.ItemModifiedAppearance = await GetSingleAsync<ItemModifiedAppearance>(new DbParameter(nameof(ItemModifiedAppearance.ItemId), id), new DbParameter(nameof(ItemModifiedAppearance.OrderIndex), modifiedAppearanceOrderIndex));

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(ItemXItemEffect)}", increaseProgress());
            var itemXItemEffect = await GetAsync<ItemXItemEffect>(new DbParameter(nameof(ItemXItemEffect.ItemId), id));

            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(ItemEffect)}", increaseProgress());
            await itemXItemEffect.ForEachAsync(async i =>
            {
                var itemEffect = await GetSingleAsync<ItemEffect>(new DbParameter(nameof(ItemEffect.Id), i.ItemEffectId));
                if (itemEffect != null)
                {
                    result.EffectGroups.Add(new()
                    {
                        ItemEffect = itemEffect
                    });
                }
            });

            if (result.ItemModifiedAppearance != null)
            {
                callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(ItemAppearance)}", increaseProgress());
                result.ItemAppearance = await GetSingleAsync<ItemAppearance>(new DbParameter(nameof(ItemAppearance.Id), result.ItemModifiedAppearance.ItemAppearanceId));
                if (result.ItemAppearance != null)
                {
                    callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(ItemDisplayInfo)}", increaseProgress());
                    result.ItemDisplayInfo = await GetSingleAsync<ItemDisplayInfo>(new DbParameter(nameof(ItemDisplayInfo.Id), result.ItemAppearance.ItemDisplayInfoId));
                    if (result.ItemDisplayInfo != null)
                    {
                        callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(ItemDisplayInfoMaterialRes)}", increaseProgress());
                        var itemDisplayInfoMaterialRes = await GetAsync<ItemDisplayInfoMaterialRes>(new DbParameter(nameof(ItemDisplayInfoMaterialRes.ItemDisplayInfoId), result.ItemDisplayInfo.Id));
                        result.ItemDisplayInfoMaterialRes = itemDisplayInfoMaterialRes.Count > 0 ? itemDisplayInfoMaterialRes : null;

                        // Load texture from new db2 if exist
                        var itemDisplayInfoModelMatRes = await GetAsync<ItemDisplayInfoModelMatRes>(new DbParameter(nameof(ItemDisplayInfoModelMatRes.ItemDisplayInfoId), result.ItemDisplayInfo.Id));
                        result.ItemDisplayInfo.ModelMaterialResourcesID1 = itemDisplayInfoModelMatRes.Where(i => i.ModelIndex == 0).FirstOrDefault()?.MaterialResourcesId ?? result.ItemDisplayInfo.ModelMaterialResourcesID1;
                        result.ItemDisplayInfo.ModelMaterialResourcesID2 = itemDisplayInfoModelMatRes.Where(i => i.ModelIndex == 1).FirstOrDefault()?.MaterialResourcesId ?? result.ItemDisplayInfo.ModelMaterialResourcesID2;
                    }
                }
            }

            callback.Invoke(LoadingHelper.Loading, "Loading successful.", 100);
            return result;
        }

        public async Task<bool> SaveAsync(ItemDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(3);

            try
            {
                // Copy values from ItemSparse
                callback.Invoke(LoadingHelper.Saving, "Preparing ID", progress());
                var itemSearchName = dto.ItemSparse != null ? JsonSerializer.Deserialize<ItemSearchName>(JsonSerializer.Serialize(dto.ItemSparse)) : null;
                var itemXItemEffects = await GetAsync<ItemXItemEffect>(new DbParameter(nameof(ItemXItemEffect.ItemId), dto.Item.Id));
                var itemDisplayInfoModelMatRes = new List<ItemDisplayInfoModelMatRes>();
                if (dto.ItemDisplayInfo != null)
                    itemDisplayInfoModelMatRes = await GetAsync<ItemDisplayInfoModelMatRes>(new DbParameter(nameof(ItemDisplayInfoModelMatRes.ItemDisplayInfoId), dto.ItemDisplayInfo.Id));

                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync(dto.Item.Id);
                }

                callback.Invoke(LoadingHelper.Saving, "Preparing ID", progress());
                await SetIdAndVerifiedBuild(dto, itemXItemEffects, itemSearchName, itemDisplayInfoModelMatRes);

                await SaveAsync(callback, progress, dto.HotfixModsEntity);
                await SaveAsync(callback, progress, dto.Item);
                await SaveAsync(callback, progress, dto.ItemSparse);
                await SaveAsync(callback, progress, itemSearchName);

                if (dto.ItemModifiedAppearance != null)
                {
                    await SaveAsync(callback, progress, dto.ItemModifiedAppearance);
                    if (dto.ItemAppearance != null)
                    {
                        await SaveAsync(callback, progress, dto.ItemAppearance);
                        if (dto.ItemDisplayInfo != null)
                        {
                            await SaveAsync(callback, progress, dto.ItemDisplayInfo);
                            if (dto.ItemDisplayInfoMaterialRes?.Any() ?? false)
                                await SaveAsync(callback, progress, dto.ItemDisplayInfoMaterialRes);

                            if (itemDisplayInfoModelMatRes.Any())
                                await SaveAsync(callback, progress, itemDisplayInfoModelMatRes);
                        }
                    }
                }

                callback.Invoke(LoadingHelper.Saving, $"Saving to {nameof(ItemEffect)} and {nameof(ItemXItemEffect)}", progress());
                if (dto.EffectGroups.Any())
                {
                    await SaveAsync(callback, progress, dto.EffectGroups.Select(s => s.ItemEffect).ToList());
                    await SaveAsync(callback, progress, itemXItemEffects);
                }

            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                return false;
            }

            callback.Invoke(LoadingHelper.Saving, "Saving successful", 100);
            dto.IsUpdate = true;

            return true;
        }

        public async Task DeleteAsync(uint id)
        {
            var dto = await GetByIdAsync(id);
            var itemSearchName = await GetSingleAsync<ItemSearchName>(new DbParameter(nameof(ItemSearchName.Id), dto.Item.Id));
            var itemXItemEffects = await GetAsync<ItemXItemEffect>(new DbParameter(nameof(ItemXItemEffect.ItemId), dto.Item.Id));

            if (dto.ItemDisplayInfo != null)
            {
                var itemDisplayInfoModelMatRes = await GetAsync<ItemDisplayInfoModelMatRes>(new DbParameter(nameof(ItemDisplayInfoModelMatRes.ItemDisplayInfoId), dto.ItemDisplayInfo.Id));
                await DeleteAsync(itemDisplayInfoModelMatRes);
            }

            await DeleteAsync(itemSearchName);
            await DeleteAsync(itemXItemEffects);
            await DeleteAsync(dto.ItemDisplayInfoMaterialRes ?? new());
            await DeleteAsync(dto.ItemDisplayInfo);
            await DeleteAsync(dto.ItemAppearance);
            await DeleteAsync(dto.ItemModifiedAppearance);
            await DeleteAsync(dto.ItemSparse);
            await DeleteAsync(dto.Item);
            await DeleteAsync(dto.HotfixModsEntity);
        }

        public async Task<uint> GetNextIdAsync()
        {
            return await GetNextIdAsync<Item>();
        }
    }
}
