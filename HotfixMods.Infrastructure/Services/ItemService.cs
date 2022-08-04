using HotfixMods.Core.Models;
using HotfixMods.Core.Providers;
using HotfixMods.Core.Enums;
using HotfixMods.Infrastructure.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService : Service
    {
        public ItemService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider) { }
        public async Task SaveItemAsync(ItemDto item)
        {
            var hotfixId = await GetNextHotfixIdAsync();
            item.InitHotfixes(hotfixId, VerifiedBuild);

            if (item.IsUpdate)
            {
                // TODO
                throw new NotImplementedException();
            }
            else
            {

                await _mySql.AddAsync(BuildItem(item));
                await _mySql.AddAsync(BuildItemAppearance(item));
                await _mySql.AddAsync(BuildItemDisplayInfo(item));
                await _mySql.AddAsync(BuildItemModifiedAppearance(item));
                await _mySql.AddAsync(BuildItemSearchName(item));
                await _mySql.AddAsync(BuildItemSparse(item));
                await _mySql.AddManyAsync(BuildItemDisplayInfoMaterialRes(item));
            }

            await _mySql.AddManyAsync(item.GetHotfixes());
        }

        public async Task<List<ItemDto>> GetItemsById(int itemId, Action<string, string, int>? progressCallback = null)
        {
            if (progressCallback == null)
                progressCallback = ConsoleProgressCallback;

            var result = new List<ItemDto>();

            progressCallback("Item", "Loading from ItemSparse", 10);
            var itemSparse = await _db2.GetAsync<ItemSparse>(i => i.Id == itemId);
            if(null == itemSparse)
            {
                progressCallback("Item", "ItemSparse not found", 100);
                return result;
            }

            progressCallback("Item", "Loading from Item", 25);
            var item = await _db2.GetAsync<Item>(i => i.Id == itemId);
            if(null == item)
            {
                progressCallback("Item", "Item not found", 100);
                return result;
            }

            progressCallback("Item", "Loading from ItemModifiedAppearance", 40);
            var itemModifiedAppearances = (await _db2.GetManyAsync<ItemModifiedAppearance>(i => i.ItemId == itemId)).ToList();
            if(!itemModifiedAppearances.Any())
            {
                progressCallback("Item", "ItemModifiedAppearance not found", 100);
                return result;
            }

            progressCallback("Item", "Setting Id", 50);
            var id = itemId;
            var isUpdate = false;
            if(id >= IdRangeFrom && id < IdRangeTo)
            {
                isUpdate = true;
            }
            else
            {
                id = await GetNextIdAsync();
            }

            foreach(var (itemModifiedAppearance, index) in itemModifiedAppearances.Select((value, idx) => (value, idx)))
            {
                progressCallback($"Appearance {index + 1} of {itemModifiedAppearances.Count}", "Loading from ItemAppearance", 50 + (int)(45.0 / itemModifiedAppearances.Count * index));
                var itemAppearance = await _db2.GetAsync<ItemAppearance>(i => i.Id == itemModifiedAppearance.ItemAppearanceId);
                if(null == itemAppearance)
                {
                    progressCallback($"Appearance {index + 1} of {itemModifiedAppearances.Count}", "ItemAppearance not found", 50 + (int)(45.0 / itemModifiedAppearances.Count * index));
                    continue;
                }

                progressCallback($"Appearance {index + 1} of {itemModifiedAppearances.Count}", "Loading from ItemDisplayInfo", 50 + (int)(45.0 / itemModifiedAppearances.Count * index));
                var itemDisplayInfo = await _db2.GetAsync<ItemDisplayInfo>(i => i.Id == itemAppearance.ItemDisplayInfoId);
                if(null == itemDisplayInfo)
                {
                    progressCallback($"Appearance {index + 1} of {itemModifiedAppearances.Count}", "ItemDisplayInfo not found", 50 + (int)(45.0 / itemModifiedAppearances.Count * index));
                    continue;
                }

                progressCallback($"Appearance {index + 1} of {itemModifiedAppearances.Count}", "Loading from ItemDisplayInfoMaterialRes", 50 + (int)(45.0 / itemModifiedAppearances.Count * index));
                var itemDisplayInfoMaterialResources = await _db2.GetManyAsync<ItemDisplayInfoMaterialRes>(i => i.ItemDisplayInfoId == itemDisplayInfo.Id);

                // Check for raid drop or artifact
                string appearanceName = "None";
                if(itemModifiedAppearances.Count() == 4)
                {
                    appearanceName = (itemModifiedAppearance.OrderIndex) switch
                    {
                        0 => "LFR",
                        1 => "Normal",
                        2 => "Heroic",
                        3 => "Mythic",
                        _ => "Unknown"
                    };
                }
                else if(itemModifiedAppearances.Count() == 24)
                {
                    appearanceName = "Artifact"; // TODO
                }

                var itemDto = new ItemDto()
                {
                    Id = id,
                    ItemDisplayInfoId = id,
                    GeosetGroup0 = itemDisplayInfo.GeosetGroup0,
                    GeosetGroup1 = itemDisplayInfo.GeosetGroup1,
                    GeosetGroup2 = itemDisplayInfo.GeosetGroup2,
                    GeosetGroup3 = itemDisplayInfo.GeosetGroup3,
                    GeosetGroup4 = itemDisplayInfo.GeosetGroup4,
                    GeosetGroup5 = itemDisplayInfo.GeosetGroup5,
                    GeosetGroupAttachment0 = itemDisplayInfo.AttachmentGeosetGroup0,
                    GeosetGroupAttachment1 = itemDisplayInfo.AttachmentGeosetGroup1,
                    GeosetGroupAttachment2 = itemDisplayInfo.AttachmentGeosetGroup2,
                    GeosetGroupAttachment3 = itemDisplayInfo.AttachmentGeosetGroup3,
                    GeosetGroupAttachment4 = itemDisplayInfo.AttachmentGeosetGroup4,
                    GeosetGroupAttachment5 = itemDisplayInfo.AttachmentGeosetGroup5,
                    HelmetGeosetVis0 = itemDisplayInfo.HelmetGeosetVis0,
                    HelmetGeosetVis1 = itemDisplayInfo.HelmetGeosetVis1,
                    ModelMaterialResourceId0 = itemDisplayInfo.ModelMaterialResourcesId0,
                    ModelMaterialResourceId1 = itemDisplayInfo.ModelMaterialResourcesId1,
                    ModelResourceId0 = itemDisplayInfo.ModelResourcesId0,
                    ModelResourceId1 = itemDisplayInfo.ModelResourcesId1,
                    ModelType0 = itemDisplayInfo.ModelType0,
                    ModelType1 = itemDisplayInfo.ModelType1,
                    Bonding = itemSparse.Bonding,
                    Flags0 = itemSparse.Flags0,
                    Flags1 = itemSparse.Flags1,
                    Flags2 = itemSparse.Flags2,
                    Flags3 = itemSparse.Flags3,
                    AllowableClasses = itemSparse.AllowableClass,
                    AllowableRaces = itemSparse.AllowableRace,
                    RequiredLevel = itemSparse.RequiredLevel,
                    ItemLevel = itemSparse.ItemLevel,
                    OverallQuality = itemSparse.OverallQualityId,
                    Material = itemSparse.Material,
                    ItemClass = item.ClassId,
                    Name = itemSparse.Display,
                    ItemSubClass = item.SubClassId,
                    ItemDisplayInfoFlags = itemDisplayInfo.Flags,
                    IconId = itemAppearance.DefaultIconFileDataId,
                    Description = itemSparse.Description,
                    InventoryType = item.InventoryType,
                    DisplayType = itemAppearance.DisplayType,
                    
                    IsUpdate = isUpdate,
                    AppearanceName = appearanceName
                };

                foreach(var materialRes in itemDisplayInfoMaterialResources)
                {
                    switch (materialRes.ComponentSection)
                    {
                        case ComponentSections.ARM_UPPER:
                            itemDto.ComponentArmUpper = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.ARM_LOWER:
                            itemDto.ComponentArmLower = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.HAND:
                            itemDto.ComponentHand = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.TORSO_UPPER:
                            itemDto.ComponentTorsoUpper = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.TORSO_LOWER:
                            itemDto.ComponentTorsoLower = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.LEG_UPPER:
                            itemDto.ComponentLegUpper = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.LEG_LOWER:
                            itemDto.ComponentLegLower = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.FOOT:
                            itemDto.ComponentFoot = materialRes.MaterialResourcesId;
                            break;
                        case ComponentSections.ACCESSORY:
                            itemDto.ComponentAccessory = materialRes.MaterialResourcesId;
                            break;
                        default:
                            break;
                    }
                }
                result.Add(itemDto);
            }
            progressCallback($"Result", $"Returning {result.Count} {(result.Count == 1 ? "item" : "items")}", 100);
            return result;
        }
    }
}
