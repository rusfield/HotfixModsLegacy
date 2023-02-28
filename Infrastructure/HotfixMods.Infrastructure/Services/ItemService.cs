using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;
using System.Text.Json;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService : ServiceBase
    {
        public ItemService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IExceptionHandler exceptionHandler, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.ItemSettings.FromId;
            ToId = appConfig.ItemSettings.ToId;
            VerifiedBuild = appConfig.ItemSettings.VerifiedBuild;
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            try
            {
                var dtos = await GetAsync<HotfixModsEntity>(new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
                var results = new List<DashboardModel>();
                foreach (var dto in dtos)
                {
                    results.Add(new()
                    {
                        ID = dto.RecordID,
                        Name = dto.Name,
                        AvatarUrl = null
                    });
                }
                return results.OrderByDescending(d => d.ID).ToList();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return new();
        }

        public async Task<ItemDto?> GetByItemDisplayInfoId(uint itemDisplayInfoId, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(9);

            try
            {
                var itemDisplayInfo = await GetSingleAsync<ItemDisplayInfo>(callback, progress, new DbParameter(nameof(ItemDisplayInfo.ID), itemDisplayInfoId));
                if (null == itemDisplayInfo)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(ItemDisplayInfo)} not found", 100);
                    return null;
                }

                var result = new ItemDto()
                {
                    ItemDisplayInfo = itemDisplayInfo,
                    ItemAppearance = new(),
                    ItemModifiedAppearance = new(),
                    Item = new()
                };

                var itemAppearance = await GetSingleAsync<ItemAppearance>(callback, progress, new DbParameter(nameof(ItemAppearance.ItemDisplayInfoID), itemDisplayInfoId));
                if (itemAppearance != null)
                {
                    result.ItemAppearance = itemAppearance;
                    var itemModifiedAppearance = await GetSingleAsync<ItemModifiedAppearance>(callback, progress, new DbParameter(nameof(ItemModifiedAppearance.ItemAppearanceID), result.ItemAppearance.ID));
                    if (itemModifiedAppearance != null)
                    {
                        result.ItemModifiedAppearance = itemModifiedAppearance;
                        var item = await GetSingleAsync<Item>(callback, progress, new DbParameter(nameof(Item.ID), result.ItemModifiedAppearance.ItemID));
                        if (item != null)
                        {
                            result.Item = item;
                            result.ItemSparse = await GetSingleAsync<ItemSparse>(callback, progress, new DbParameter(nameof(ItemSparse.ID), result.Item.ID));
                            result.IsUpdate = true;

                            var itemXItemEffect = await GetAsync<ItemXItemEffect>(callback, progress, new DbParameter(nameof(ItemXItemEffect.ItemID), result.Item.ID));

                            callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(ItemEffect)}", progress());
                            await itemXItemEffect.ForEachAsync(async i =>
                            {
                                var itemEffect = await GetSingleAsync<ItemEffect>(new DbParameter(nameof(ItemEffect.ID), i.ItemEffectID));
                                if (itemEffect != null)
                                {
                                    result.EffectGroups.Add(new()
                                    {
                                        ItemEffect = itemEffect
                                    });
                                }
                            });
                        }
                    }
                }

                var itemDisplayInfoMaterialRes = await GetAsync<ItemDisplayInfoMaterialRes>(callback, progress, new DbParameter(nameof(ItemDisplayInfoMaterialRes.ItemDisplayInfoID), result.ItemDisplayInfo.ID));
                result.ItemDisplayInfoMaterialRes = itemDisplayInfoMaterialRes.Count > 0 ? itemDisplayInfoMaterialRes : null;

                // Load texture from new db2 if exist
                var itemDisplayInfoModelMatRes = await GetAsync<ItemDisplayInfoModelMatRes>(callback, progress, new DbParameter(nameof(ItemDisplayInfoModelMatRes.ItemDisplayInfoID), result.ItemDisplayInfo.ID));
                result.ItemDisplayInfo.ModelMaterialResourcesID0 = itemDisplayInfoModelMatRes.Where(i => i.ModelIndex == 0).FirstOrDefault()?.MaterialResourcesID ?? result.ItemDisplayInfo.ModelMaterialResourcesID0;
                result.ItemDisplayInfo.ModelMaterialResourcesID1 = itemDisplayInfoModelMatRes.Where(i => i.ModelIndex == 1).FirstOrDefault()?.MaterialResourcesID ?? result.ItemDisplayInfo.ModelMaterialResourcesID1;

                callback.Invoke(LoadingHelper.Loading, "Loading successful", 100);
                return result;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return null;
        }

        public async Task<ItemDto?> GetByIdAsync(uint id, int modifiedAppearanceOrderIndex = 0, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(11);

            try
            {
                var item = await GetSingleAsync<Item>(callback, progress, new DbParameter(nameof(Item.ID), id));
                if (null == item)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(Item)} not found", 100);
                    return null;
                }

                var result = new ItemDto()
                {
                    Item = item,
                    IsUpdate = true
                };

                result.HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(callback, progress, item.ID);
                result.ItemSparse = await GetSingleAsync<ItemSparse>(callback, progress, new DbParameter(nameof(ItemSparse.ID), id));
                result.ItemModifiedAppearance = await GetSingleAsync<ItemModifiedAppearance>(callback, progress, new DbParameter(nameof(ItemModifiedAppearance.ItemID), id), new DbParameter(nameof(ItemModifiedAppearance.OrderIndex), modifiedAppearanceOrderIndex));

                var itemXItemEffect = await GetAsync<ItemXItemEffect>(callback, progress, new DbParameter(nameof(ItemXItemEffect.ItemID), id));

                callback.Invoke(LoadingHelper.Loading, $"Loading {nameof(ItemEffect)}", progress());
                await itemXItemEffect.ForEachAsync(async i =>
                {
                    var itemEffect = await GetSingleAsync<ItemEffect>(new DbParameter(nameof(ItemEffect.ID), i.ItemEffectID));
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
                    result.ItemAppearance = await GetSingleAsync<ItemAppearance>(callback, progress, new DbParameter(nameof(ItemAppearance.ID), result.ItemModifiedAppearance.ItemAppearanceID));
                    if (result.ItemAppearance != null)
                    {
                        result.ItemDisplayInfo = await GetSingleAsync<ItemDisplayInfo>(callback, progress, new DbParameter(nameof(ItemDisplayInfo.ID), result.ItemAppearance.ItemDisplayInfoID));
                        if (result.ItemDisplayInfo != null)
                        {
                            var itemDisplayInfoMaterialRes = await GetAsync<ItemDisplayInfoMaterialRes>(callback, progress, new DbParameter(nameof(ItemDisplayInfoMaterialRes.ItemDisplayInfoID), result.ItemDisplayInfo.ID));
                            result.ItemDisplayInfoMaterialRes = itemDisplayInfoMaterialRes.Count > 0 ? itemDisplayInfoMaterialRes : null;

                            // Load texture from new db2 if exist
                            var itemDisplayInfoModelMatRes = await GetAsync<ItemDisplayInfoModelMatRes>(callback, progress, new DbParameter(nameof(ItemDisplayInfoModelMatRes.ItemDisplayInfoID), result.ItemDisplayInfo.ID));
                            result.ItemDisplayInfo.ModelMaterialResourcesID0 = itemDisplayInfoModelMatRes.Where(i => i.ModelIndex == 0).FirstOrDefault()?.MaterialResourcesID ?? result.ItemDisplayInfo.ModelMaterialResourcesID0;
                            result.ItemDisplayInfo.ModelMaterialResourcesID1 = itemDisplayInfoModelMatRes.Where(i => i.ModelIndex == 1).FirstOrDefault()?.MaterialResourcesID ?? result.ItemDisplayInfo.ModelMaterialResourcesID1;
                        }
                    }
                }

                callback.Invoke(LoadingHelper.Loading, "Loading successful", 100);
                return result;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return null;
        }

        public async Task<bool> SaveAsync(ItemDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(14);

            try
            {
                // Copy values from ItemSparse
                callback.Invoke(LoadingHelper.Saving, "Preparing ID", progress());
                var itemSearchName = dto.ItemSparse != null ? JsonSerializer.Deserialize<ItemSearchName>(JsonSerializer.Serialize(dto.ItemSparse)) : null;
                var itemXItemEffects = await GetAsync<ItemXItemEffect>(new DbParameter(nameof(ItemXItemEffect.ItemID), dto.Item.ID));
                var itemDisplayInfoModelMatRes = new List<ItemDisplayInfoModelMatRes>();
                if (dto.ItemDisplayInfo != null)
                    itemDisplayInfoModelMatRes = await GetAsync<ItemDisplayInfoModelMatRes>(new DbParameter(nameof(ItemDisplayInfoModelMatRes.ItemDisplayInfoID), dto.ItemDisplayInfo.ID));

                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync(dto.Item.ID);
                }

                callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
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
                            await SaveAsync(callback, progress, dto.ItemDisplayInfoMaterialRes?.ToList() ?? new());
                            await SaveAsync(callback, progress, itemDisplayInfoModelMatRes);
                        }
                    }
                }

                callback.Invoke(LoadingHelper.Saving, $"Saving to {nameof(ItemEffect)} and {nameof(ItemXItemEffect)}", progress());
                if (dto.EffectGroups.Any())
                {
                    await SaveAsync(dto.EffectGroups.Select(s => s.ItemEffect).ToList());
                    await SaveAsync(itemXItemEffects);
                }

                callback.Invoke(LoadingHelper.Saving, "Saving successful", 100);
                dto.IsUpdate = true;

                return true;

            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return false;
        }

        public async Task<bool> DeleteAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(11);

            try
            {
                var dto = await GetByIdAsync(id);
                if (null == dto)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }
                var itemSearchName = await GetSingleAsync<ItemSearchName>(new DbParameter(nameof(ItemSearchName.ID), dto.Item.ID));
                var itemXItemEffects = await GetAsync<ItemXItemEffect>(new DbParameter(nameof(ItemXItemEffect.ItemID), dto.Item.ID));

                if (dto.ItemDisplayInfo != null)
                {
                    var itemDisplayInfoModelMatRes = await GetAsync<ItemDisplayInfoModelMatRes>(new DbParameter(nameof(ItemDisplayInfoModelMatRes.ItemDisplayInfoID), dto.ItemDisplayInfo.ID));
                    await DeleteAsync(callback, progress, itemDisplayInfoModelMatRes);
                }

                await DeleteAsync(callback, progress, itemSearchName);
                await DeleteAsync(callback, progress, itemXItemEffects);
                await DeleteAsync(callback, progress, dto.ItemDisplayInfoMaterialRes ?? new());
                await DeleteAsync(callback, progress, dto.ItemDisplayInfo);
                await DeleteAsync(callback, progress, dto.ItemAppearance);
                await DeleteAsync(callback, progress, dto.ItemModifiedAppearance);
                await DeleteAsync(callback, progress, dto.ItemSparse);
                await DeleteAsync(callback, progress, dto.Item);
                await DeleteAsync(callback, progress, dto.HotfixModsEntity);

                callback.Invoke(LoadingHelper.Deleting, "Delete successful", 100);
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return false;
        }

     
    }
}
