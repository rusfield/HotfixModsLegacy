using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Core.Constants;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService
    {
        List<ItemDisplayInfoMaterialRes> BuildItemDisplayInfoMaterialRes(ItemDto item)
        {
            var result = new List<ItemDisplayInfoMaterialRes>();

            if (!item.ComponentArmLower.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.ARM_LOWER,
                    ComponentSection = ComponentSections.ARM_LOWER,
                    MaterialResourceId = (int)item.ComponentArmLower,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = item.GetVerifiedBuild()
                });
                item.AddHotfix(item.Id + (int)ComponentSections.ARM_LOWER, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentArmUpper.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.ARM_UPPER,
                    ComponentSection = ComponentSections.ARM_UPPER,
                    MaterialResourceId = (int)item.ComponentArmUpper,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = item.GetVerifiedBuild()
                });
                item.AddHotfix(item.Id + (int)ComponentSections.ARM_UPPER, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentTorsoLower.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.TORSO_LOWER,
                    ComponentSection = ComponentSections.TORSO_LOWER,
                    MaterialResourceId = (int)item.ComponentTorsoLower,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = item.GetVerifiedBuild()
                });
                item.AddHotfix(item.Id + (int)ComponentSections.TORSO_LOWER, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentTorsoUpper.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.TORSO_UPPER,
                    ComponentSection = ComponentSections.TORSO_UPPER,
                    MaterialResourceId = (int)item.ComponentTorsoUpper,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = item.GetVerifiedBuild()
                });
                item.AddHotfix(item.Id + (int)ComponentSections.TORSO_UPPER, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentLegLower.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.LEG_LOWER,
                    ComponentSection = ComponentSections.LEG_LOWER,
                    MaterialResourceId = (int)item.ComponentLegLower,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = item.GetVerifiedBuild()
                });
                item.AddHotfix(item.Id + (int)ComponentSections.LEG_LOWER, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentLegUpper.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.LEG_UPPER,
                    ComponentSection = ComponentSections.LEG_UPPER,
                    MaterialResourceId = (int)item.ComponentLegUpper,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = item.GetVerifiedBuild()
                });
                item.AddHotfix(item.Id + (int)ComponentSections.LEG_UPPER, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentHand.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.HAND,
                    ComponentSection = ComponentSections.HAND,
                    MaterialResourceId = (int)item.ComponentHand,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = item.GetVerifiedBuild()
                });
                item.AddHotfix(item.Id + (int)ComponentSections.HAND, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentFoot.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.FOOT,
                    ComponentSection = ComponentSections.FOOT,
                    MaterialResourceId = (int)item.ComponentFoot,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = item.GetVerifiedBuild()
                });
                item.AddHotfix(item.Id + (int)ComponentSections.FOOT, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentAccessory.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.ACCESSORY,
                    ComponentSection = ComponentSections.ACCESSORY,
                    MaterialResourceId = (int)item.ComponentAccessory,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = item.GetVerifiedBuild()
                });
                item.AddHotfix(item.Id + (int)ComponentSections.ACCESSORY, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            return result;
        }

        ItemAppearance BuildItemAppearance(ItemDto item)
        {
            var result = new ItemAppearance()
            {
                Id = item.Id,
                DefaultIconFileDataId = item.IconId ?? ItemDefaults.IconId,
                DisplayType = GetDisplayType(),
                ItemDisplayInfoId = item.Id,
                UiOrder = item.Id, // CHECK IF OK -- Should be id x 100 according to DB2, but this may result in a very big number
                VerifiedBuild = item.GetVerifiedBuild()
            };

            item.AddHotfix(item.Id, TableHashes.ItemAppearance, HotfixStatuses.VALID);

            return result;
        }

        ItemModifiedAppearance BuildItemModifiedAppearance(ItemDto item)
        {
            var result = new ItemModifiedAppearance()
            {
                Id = item.Id,
                ItemAppearanceId = item.Id,
                ItemId = item.Id,
                ItemAppearanceModifierId = 0,
                OrderIndex = 0,
                VerifiedBuild = item.GetVerifiedBuild()
            };

            item.AddHotfix(item.Id, TableHashes.ItemModifiedAppearance, HotfixStatuses.VALID);


            return result;
        }

        ItemSearchName BuildItemSearchName(ItemDto item)
        {
            var result = new ItemSearchName()
            {
                Id = item.Id,
                Display = item.Name ?? ItemDefaults.Name,
                Flags0 = item.Flags0 ?? ItemDefaults.Flags0,
                Flags1 = item.Flags1 ?? ItemDefaults.Flags1,
                Flags2 = item.Flags2 ?? ItemDefaults.Flags2,
                Flags3 = item.Flags3 ?? ItemDefaults.Flags3,
                ItemLevel = ItemDefaults.ItemLevel,
                OverallQualityId = item.OverallQuality ?? ItemDefaults.OverallQuality,
                RequiredLevel = item.RequiredLevel ?? ItemDefaults.RequiredLevel,
                AllowableClass = item.AllowableClasses ?? ItemDefaults.AllowableClasses,
                AllowableRace = item.AllowableRaces ?? ItemDefaults.AllowableRaces,
                VerifiedBuild = item.GetVerifiedBuild()
            };

            item.AddHotfix(item.Id, TableHashes.ItemSearchName, HotfixStatuses.VALID);

            return result;
        }

        ItemSparse BuildItemSparse(ItemDto item)
        {
            var result = new ItemSparse()
            {
                Id = item.Id,
                Bonding = item.Bonding ?? ItemDefaults.Bonding,
                Display = item.Name ?? ItemDefaults.Name,
                Flags0 = item.Flags0 ?? ItemDefaults.Flags0,
                Flags1 = item.Flags1 ?? ItemDefaults.Flags1,
                Flags2 = item.Flags2 ?? ItemDefaults.Flags2,
                Flags3 = item.Flags3 ?? ItemDefaults.Flags3,
                InventoryType = GetInventoryType(),
                ItemLevel = item.ItemLevel ?? ItemDefaults.ItemLevel,
                Material = item.Material ?? ItemDefaults.Material,
                OverallQualityId = item.OverallQuality ?? ItemDefaults.OverallQuality,
                RequiredLevel = item.RequiredLevel ?? ItemDefaults.RequiredLevel,
                VerifiedBuild = item.GetVerifiedBuild(),
                AllowableClass = item.AllowableClasses ?? ItemDefaults.AllowableClasses,
                AllowableRace = item.AllowableRaces ?? ItemDefaults.AllowableRaces,

                PriceRandomValue = 1,
                Stackable = 1,
                VendorStackCount = 1,
                Display1 = "",
                Display2 = "",
                Display3 = "",
                Description = ""
            };

            item.AddHotfix(item.Id, TableHashes.ItemSparse, HotfixStatuses.VALID);

            return result;
        }

        Item BuildItem(ItemDto item)
        {
            var result = new Item()
            {
                Id = item.Id,
                IconFileDataId = item.IconId ?? ItemDefaults.IconId,
                ClassId = GetClass(),
                InventoryType = GetInventoryType(),
                Material = item.Material ?? ItemDefaults.Material,
                SubClassId = GetSubclassId(),
                VerifiedBuild = item.GetVerifiedBuild()
            };
            item.AddHotfix(item.Id, TableHashes.Item, HotfixStatuses.VALID);

            return result;
        }

        ItemDisplayInfo BuildItemDisplayInfo(ItemDto item)
        {
            var result = new ItemDisplayInfo()
            {
                Id = item.Id,
                Flags = item.ItemDisplayInfoFlags ?? ItemDefaults.DisplayInfoFlag,
                ModelMaterialResourcesId0 = item.ModelMaterialResourceId0 ?? ItemDefaults.ModelMaterialResourcesId,
                ModelMaterialResourcesId1 = item.ModelMaterialResourceId1 ?? ItemDefaults.ModelMaterialResourcesId,
                ModelResourcesId0 = item.ModelResourceId0 ?? ItemDefaults.ModelResourceId,
                ModelResourcesId1 = item.ModelResourceId1 ?? ItemDefaults.ModelResourceId,
                VerifiedBuild = item.GetVerifiedBuild()
            };

            item.AddHotfix(item.Id, TableHashes.ItemDisplayInfo, HotfixStatuses.VALID);
            
            return result;
        }
    }
}
