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
using HotfixMods.Infrastructure.DefaultModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService
    {
        ItemDisplayInfoMaterialRes[] BuildItemDisplayInfoMaterialRes(ItemDto item)
        {
            var result = new List<ItemDisplayInfoMaterialRes>();

            if (!item.ComponentArmLower.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.ARM_LOWER,
                    ComponentSection = ComponentSections.ARM_LOWER,
                    MaterialResourcesId = (int)item.ComponentArmLower,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = VerifiedBuild
                });
                item.AddHotfix(item.Id + (int)ComponentSections.ARM_LOWER, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentArmUpper.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.ARM_UPPER,
                    ComponentSection = ComponentSections.ARM_UPPER,
                    MaterialResourcesId = (int)item.ComponentArmUpper,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = VerifiedBuild
                });
                item.AddHotfix(item.Id + (int)ComponentSections.ARM_UPPER, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentTorsoLower.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.TORSO_LOWER,
                    ComponentSection = ComponentSections.TORSO_LOWER,
                    MaterialResourcesId = (int)item.ComponentTorsoLower,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = VerifiedBuild
                });
                item.AddHotfix(item.Id + (int)ComponentSections.TORSO_LOWER, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentTorsoUpper.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.TORSO_UPPER,
                    ComponentSection = ComponentSections.TORSO_UPPER,
                    MaterialResourcesId = (int)item.ComponentTorsoUpper,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = VerifiedBuild
                });
                item.AddHotfix(item.Id + (int)ComponentSections.TORSO_UPPER, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentLegLower.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.LEG_LOWER,
                    ComponentSection = ComponentSections.LEG_LOWER,
                    MaterialResourcesId = (int)item.ComponentLegLower,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = VerifiedBuild
                });
                item.AddHotfix(item.Id + (int)ComponentSections.LEG_LOWER, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentLegUpper.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.LEG_UPPER,
                    ComponentSection = ComponentSections.LEG_UPPER,
                    MaterialResourcesId = (int)item.ComponentLegUpper,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = VerifiedBuild
                });
                item.AddHotfix(item.Id + (int)ComponentSections.LEG_UPPER, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentHand.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.HAND,
                    ComponentSection = ComponentSections.HAND,
                    MaterialResourcesId = (int)item.ComponentHand,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = VerifiedBuild
                });
                item.AddHotfix(item.Id + (int)ComponentSections.HAND, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentFoot.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.FOOT,
                    ComponentSection = ComponentSections.FOOT,
                    MaterialResourcesId = (int)item.ComponentFoot,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = VerifiedBuild
                });
                item.AddHotfix(item.Id + (int)ComponentSections.FOOT, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            if (!item.ComponentAccessory.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = item.Id + (int)ComponentSections.ACCESSORY,
                    ComponentSection = ComponentSections.ACCESSORY,
                    MaterialResourcesId = (int)item.ComponentAccessory,
                    ItemDisplayInfoId = item.Id,
                    VerifiedBuild = VerifiedBuild
                });
                item.AddHotfix(item.Id + (int)ComponentSections.ACCESSORY, TableHashes.ItemDisplayInfoMaterialRes, HotfixStatuses.VALID);

            }

            return result.ToArray();
        }

        ItemAppearance BuildItemAppearance(ItemDto item)
        {
            var result = new ItemAppearance()
            {
                Id = item.Id,
                DefaultIconFileDataId = item.IconId ?? Default.ItemAppearance.DefaultIconFileDataId,
                DisplayType = item.DisplayType,
                ItemDisplayInfoId = item.Id,
                UiOrder = item.Id, // CHECK IF OK -- Should be id x 100 according to DB2, but this may result in a very big number
                VerifiedBuild = VerifiedBuild
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
                VerifiedBuild = VerifiedBuild,

                ItemAppearanceModifierId = Default.ItemModifiedAppearance.ItemAppearanceModifierId,
                OrderIndex = Default.ItemModifiedAppearance.OrderIndex
            };

            item.AddHotfix(item.Id, TableHashes.ItemModifiedAppearance, HotfixStatuses.VALID);

            return result;
        }

        ItemSearchName BuildItemSearchName(ItemDto item)
        {
            var result = new ItemSearchName()
            {
                Id = item.Id,
                Display = item.Name ?? Default.ItemSearchName.Display,
                Flags0 = item.Flags0 ?? Default.ItemSearchName.Flags0,
                Flags1 = item.Flags1 ?? Default.ItemSearchName.Flags1,
                Flags2 = item.Flags2 ?? Default.ItemSearchName.Flags2,
                Flags3 = item.Flags3 ?? Default.ItemSearchName.Flags3,
                ItemLevel = item.ItemLevel ?? Default.ItemSearchName.ItemLevel,
                OverallQualityId = item.OverallQuality ?? Default.ItemSearchName.OverallQualityId,
                RequiredLevel = item.RequiredLevel ?? Default.ItemSearchName.RequiredLevel,
                AllowableClass = item.AllowableClasses ?? Default.ItemSearchName.AllowableClass,
                AllowableRace = item.AllowableRaces ?? Default.ItemSearchName.AllowableRace,
                VerifiedBuild = VerifiedBuild
            };

            item.AddHotfix(item.Id, TableHashes.ItemSearchName, HotfixStatuses.VALID);

            return result;
        }

        ItemSparse BuildItemSparse(ItemDto item)
        {
            var result = new ItemSparse()
            {
                Id = item.Id,
                VerifiedBuild = VerifiedBuild,
                InventoryType = item.InventoryType,

                Bonding = item.Bonding ?? Default.ItemSparse.Bonding,
                Display = item.Name ?? Default.ItemSparse.Display,
                Flags0 = item.Flags0 ?? Default.ItemSparse.Flags0,
                Flags1 = item.Flags1 ?? Default.ItemSparse.Flags1,
                Flags2 = item.Flags2 ?? Default.ItemSparse.Flags2,
                Flags3 = item.Flags3 ?? Default.ItemSparse.Flags3,
                ItemLevel = item.ItemLevel ?? Default.ItemSparse.ItemLevel,
                Material = item.Material ?? Default.ItemSparse.Material,
                OverallQualityId = item.OverallQuality ?? Default.ItemSparse.OverallQualityId,
                RequiredLevel = item.RequiredLevel ?? Default.ItemSparse.RequiredLevel,
                AllowableClass = item.AllowableClasses ?? Default.ItemSparse.AllowableClass,
                AllowableRace = item.AllowableRaces ?? Default.ItemSparse.AllowableRace,
                Description = item.Description ?? Default.ItemSparse.Description,
                StatModifierBonusStat0 = item.StatModifierBonusStat0 ?? Default.ItemSparse.StatModifierBonusStat0,
                StatModifierBonusStat1 = item.StatModifierBonusStat1 ?? Default.ItemSparse.StatModifierBonusStat1,
                StatModifierBonusStat2 = item.StatModifierBonusStat2 ?? Default.ItemSparse.StatModifierBonusStat2,
                StatModifierBonusStat3 = item.StatModifierBonusStat3 ?? Default.ItemSparse.StatModifierBonusStat3,
                StatModifierBonusStat4 = item.StatModifierBonusStat4 ?? Default.ItemSparse.StatModifierBonusStat4,
                StatModifierBonusStat5 = item.StatModifierBonusStat5 ?? Default.ItemSparse.StatModifierBonusStat5,
                StatModifierBonusStat6 = item.StatModifierBonusStat6 ?? Default.ItemSparse.StatModifierBonusStat6,
                StatModifierBonusStat7 = item.StatModifierBonusStat7 ?? Default.ItemSparse.StatModifierBonusStat7,
                StatModifierBonusStat8 = item.StatModifierBonusStat8 ?? Default.ItemSparse.StatModifierBonusStat8,
                StatModifierBonusStat9 = item.StatModifierBonusStat9 ?? Default.ItemSparse.StatModifierBonusStat9,
                StatPercentEditor0 = item.StatPercentEditor0 ?? Default.ItemSparse.StatPercentEditor0,
                StatPercentEditor1 = item.StatPercentEditor1 ?? Default.ItemSparse.StatPercentEditor1,
                StatPercentEditor2 = item.StatPercentEditor2 ?? Default.ItemSparse.StatPercentEditor2,
                StatPercentEditor3 = item.StatPercentEditor3 ?? Default.ItemSparse.StatPercentEditor3,
                StatPercentEditor4 = item.StatPercentEditor4 ?? Default.ItemSparse.StatPercentEditor4,
                StatPercentEditor5 = item.StatPercentEditor5 ?? Default.ItemSparse.StatPercentEditor5,
                StatPercentEditor6 = item.StatPercentEditor6 ?? Default.ItemSparse.StatPercentEditor6,
                StatPercentEditor7 = item.StatPercentEditor7 ?? Default.ItemSparse.StatPercentEditor7,
                StatPercentEditor8 = item.StatPercentEditor8 ?? Default.ItemSparse.StatPercentEditor8,
                StatPercentEditor9 = item.StatPercentEditor9 ?? Default.ItemSparse.StatPercentEditor9,

                PriceRandomValue = Default.ItemSparse.PriceRandomValue,
                Stackable = Default.ItemSparse.Stackable,
                VendorStackCount = Default.ItemSparse.VendorStackCount,
                Display1 = Default.ItemSparse.Display1,
                Display2 = Default.ItemSparse.Display2,
                Display3 = Default.ItemSparse.Display3
            };

            item.AddHotfix(item.Id, TableHashes.ItemSparse, HotfixStatuses.VALID);

            return result;
        }

        Item BuildItem(ItemDto item)
        {
            var result = new Item()
            {
                Id = item.Id,
                VerifiedBuild = VerifiedBuild,
                SubClassId = item.ItemSubClass,
                ItemGroupSoundsId = item.ItemGroupSoundsId ?? Default.Item.ItemGroupSoundsId,
                ClassId = item.ItemClass,
                InventoryType = item.InventoryType,

                IconFileDataId = item.IconId ?? Default.Item.IconFileDataId,
                Material = item.Material ?? Default.Item.Material,

                SoundOverrideSubClassId = Default.Item.SoundOverrideSubClassId
            };
            item.AddHotfix(item.Id, TableHashes.Item, HotfixStatuses.VALID);

            return result;
        }

        ItemDisplayInfo BuildItemDisplayInfo(ItemDto item)
        {
            var result = new ItemDisplayInfo()
            {
                Id = item.Id,
                VerifiedBuild = VerifiedBuild,

                Flags = item.ItemDisplayInfoFlags ?? Default.ItemDisplayInfo.Flags,
                ModelMaterialResourcesId0 = item.ModelMaterialResourceId0 ?? Default.ItemDisplayInfo.ModelMaterialResourcesId0,
                ModelMaterialResourcesId1 = item.ModelMaterialResourceId1 ?? Default.ItemDisplayInfo.ModelMaterialResourcesId1,
                ModelResourcesId0 = item.ModelResourceId0 ?? Default.ItemDisplayInfo.ModelResourcesId0,
                ModelResourcesId1 = item.ModelResourceId1 ?? Default.ItemDisplayInfo.ModelResourcesId1,
                AttachmentGeosetGroup0 = item.GeosetGroupAttachment0 ?? Default.ItemDisplayInfo.AttachmentGeosetGroup0,
                AttachmentGeosetGroup1 = item.GeosetGroupAttachment1 ?? Default.ItemDisplayInfo.AttachmentGeosetGroup1,
                AttachmentGeosetGroup2 = item.GeosetGroupAttachment2 ?? Default.ItemDisplayInfo.AttachmentGeosetGroup2,
                AttachmentGeosetGroup3 = item.GeosetGroupAttachment3 ?? Default.ItemDisplayInfo.AttachmentGeosetGroup3,
                AttachmentGeosetGroup4 = item.GeosetGroupAttachment4 ?? Default.ItemDisplayInfo.AttachmentGeosetGroup4,
                AttachmentGeosetGroup5 = item.GeosetGroupAttachment5 ?? Default.ItemDisplayInfo.AttachmentGeosetGroup5,
                GeosetGroup0 = item.GeosetGroup0 ?? Default.ItemDisplayInfo.GeosetGroup0,
                GeosetGroup1 = item.GeosetGroup1 ?? Default.ItemDisplayInfo.GeosetGroup1,
                GeosetGroup2 = item.GeosetGroup2 ?? Default.ItemDisplayInfo.GeosetGroup2,
                GeosetGroup3 = item.GeosetGroup3 ?? Default.ItemDisplayInfo.GeosetGroup3,
                GeosetGroup4 = item.GeosetGroup4 ?? Default.ItemDisplayInfo.GeosetGroup4,
                GeosetGroup5 = item.GeosetGroup5 ?? Default.ItemDisplayInfo.GeosetGroup5,
                HelmetGeosetVis0 = item.HelmetGeosetVis0 ?? Default.ItemDisplayInfo.HelmetGeosetVis0,
                HelmetGeosetVis1 = item.HelmetGeosetVis1 ?? Default.ItemDisplayInfo.HelmetGeosetVis1,
                ModelType0 = item.ModelType0 ?? Default.ItemDisplayInfo.ModelType0, 
                ModelType1 = item.ModelType1 ?? Default.ItemDisplayInfo.ModelType1,

                ItemRangedDisplayInfoID = Default.ItemDisplayInfo.ItemRangedDisplayInfoID,
                ItemVisual = Default.ItemDisplayInfo.ItemVisual,
                OverrideSwooshSoundKitID = Default.ItemDisplayInfo.OverrideSwooshSoundKitID,
                ParticleColorID = Default.ItemDisplayInfo.ParticleColorID,
                SheathedSpellVisualKitID = Default.ItemDisplayInfo.SheathedSpellVisualKitID,
                SheatheTransformMatrixID = Default.ItemDisplayInfo.SheatheTransformMatrixID,
                StateSpellVisualKitID = Default.ItemDisplayInfo.StateSpellVisualKitID,
                UnsheathedSpellVisualKitID = Default.ItemDisplayInfo.UnsheathedSpellVisualKitID
            };

            item.AddHotfix(item.Id, TableHashes.ItemDisplayInfo, HotfixStatuses.VALID);
            
            return result;
        }
    }
}
