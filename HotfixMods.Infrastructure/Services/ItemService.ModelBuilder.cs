using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.DefaultModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService
    {
        ItemDisplayInfoMaterialRes[] BuildItemDisplayInfoMaterialRes(ItemDto itemDto)
        {
            var result = new List<ItemDisplayInfoMaterialRes>();

            if (!itemDto.ComponentArmLower.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = itemDto.Id + (int)ComponentSections.ARM_LOWER,
                    ComponentSection = ComponentSections.ARM_LOWER,
                    MaterialResourcesId = (int)itemDto.ComponentArmLower,
                    ItemDisplayInfoId = itemDto.Id,
                    VerifiedBuild = VerifiedBuild
                });
                itemDto.AddHotfix(itemDto.Id + (int)ComponentSections.ARM_LOWER, TableHashes.ITEM_DISPLAY_INFO_MATERIAL_RES, HotfixStatuses.VALID);

            }

            if (!itemDto.ComponentArmUpper.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = itemDto.Id + (int)ComponentSections.ARM_UPPER,
                    ComponentSection = ComponentSections.ARM_UPPER,
                    MaterialResourcesId = (int)itemDto.ComponentArmUpper,
                    ItemDisplayInfoId = itemDto.Id,
                    VerifiedBuild = VerifiedBuild
                });
                itemDto.AddHotfix(itemDto.Id + (int)ComponentSections.ARM_UPPER, TableHashes.ITEM_DISPLAY_INFO_MATERIAL_RES, HotfixStatuses.VALID);

            }

            if (!itemDto.ComponentTorsoLower.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = itemDto.Id + (int)ComponentSections.TORSO_LOWER,
                    ComponentSection = ComponentSections.TORSO_LOWER,
                    MaterialResourcesId = (int)itemDto.ComponentTorsoLower,
                    ItemDisplayInfoId = itemDto.Id,
                    VerifiedBuild = VerifiedBuild
                });
                itemDto.AddHotfix(itemDto.Id + (int)ComponentSections.TORSO_LOWER, TableHashes.ITEM_DISPLAY_INFO_MATERIAL_RES, HotfixStatuses.VALID);

            }

            if (!itemDto.ComponentTorsoUpper.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = itemDto.Id + (int)ComponentSections.TORSO_UPPER,
                    ComponentSection = ComponentSections.TORSO_UPPER,
                    MaterialResourcesId = (int)itemDto.ComponentTorsoUpper,
                    ItemDisplayInfoId = itemDto.Id,
                    VerifiedBuild = VerifiedBuild
                });
                itemDto.AddHotfix(itemDto.Id + (int)ComponentSections.TORSO_UPPER, TableHashes.ITEM_DISPLAY_INFO_MATERIAL_RES, HotfixStatuses.VALID);

            }

            if (!itemDto.ComponentLegLower.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = itemDto.Id + (int)ComponentSections.LEG_LOWER,
                    ComponentSection = ComponentSections.LEG_LOWER,
                    MaterialResourcesId = (int)itemDto.ComponentLegLower,
                    ItemDisplayInfoId = itemDto.Id,
                    VerifiedBuild = VerifiedBuild
                });
                itemDto.AddHotfix(itemDto.Id + (int)ComponentSections.LEG_LOWER, TableHashes.ITEM_DISPLAY_INFO_MATERIAL_RES, HotfixStatuses.VALID);

            }

            if (!itemDto.ComponentLegUpper.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = itemDto.Id + (int)ComponentSections.LEG_UPPER,
                    ComponentSection = ComponentSections.LEG_UPPER,
                    MaterialResourcesId = (int)itemDto.ComponentLegUpper,
                    ItemDisplayInfoId = itemDto.Id,
                    VerifiedBuild = VerifiedBuild
                });
                itemDto.AddHotfix(itemDto.Id + (int)ComponentSections.LEG_UPPER, TableHashes.ITEM_DISPLAY_INFO_MATERIAL_RES, HotfixStatuses.VALID);

            }

            if (!itemDto.ComponentHand.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = itemDto.Id + (int)ComponentSections.HAND,
                    ComponentSection = ComponentSections.HAND,
                    MaterialResourcesId = (int)itemDto.ComponentHand,
                    ItemDisplayInfoId = itemDto.Id,
                    VerifiedBuild = VerifiedBuild
                });
                itemDto.AddHotfix(itemDto.Id + (int)ComponentSections.HAND, TableHashes.ITEM_DISPLAY_INFO_MATERIAL_RES, HotfixStatuses.VALID);

            }

            if (!itemDto.ComponentFoot.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = itemDto.Id + (int)ComponentSections.FOOT,
                    ComponentSection = ComponentSections.FOOT,
                    MaterialResourcesId = (int)itemDto.ComponentFoot,
                    ItemDisplayInfoId = itemDto.Id,
                    VerifiedBuild = VerifiedBuild
                });
                itemDto.AddHotfix(itemDto.Id + (int)ComponentSections.FOOT, TableHashes.ITEM_DISPLAY_INFO_MATERIAL_RES, HotfixStatuses.VALID);

            }

            if (!itemDto.ComponentAccessory.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = itemDto.Id + (int)ComponentSections.ACCESSORY,
                    ComponentSection = ComponentSections.ACCESSORY,
                    MaterialResourcesId = (int)itemDto.ComponentAccessory,
                    ItemDisplayInfoId = itemDto.Id,
                    VerifiedBuild = VerifiedBuild
                });
                itemDto.AddHotfix(itemDto.Id + (int)ComponentSections.ACCESSORY, TableHashes.ITEM_DISPLAY_INFO_MATERIAL_RES, HotfixStatuses.VALID);

            }

            return result.ToArray();
        }

        ItemAppearance BuildItemAppearance(ItemDto itemDto)
        {
            var result = new ItemAppearance()
            {
                Id = itemDto.Id,
                DefaultIconFileDataId = itemDto.IconId ?? Default.ItemAppearance.DefaultIconFileDataId,
                DisplayType = itemDto.DisplayType,
                ItemDisplayInfoId = itemDto.Id,
                UiOrder = itemDto.Id, // CHECK IF OK -- Should be id x 100 according to DB2, but this may result in a very big number
                VerifiedBuild = VerifiedBuild
            };

            itemDto.AddHotfix(itemDto.Id, TableHashes.ITEM_APPEARANCE, HotfixStatuses.VALID);

            return result;
        }

        ItemModifiedAppearance BuildItemModifiedAppearance(ItemDto itemDto)
        {
            var result = new ItemModifiedAppearance()
            {
                Id = itemDto.Id,
                ItemAppearanceId = itemDto.Id,
                ItemId = itemDto.Id,
                VerifiedBuild = VerifiedBuild,

                ItemAppearanceModifierId = Default.ItemModifiedAppearance.ItemAppearanceModifierId,
                OrderIndex = Default.ItemModifiedAppearance.OrderIndex
            };

            itemDto.AddHotfix(itemDto.Id, TableHashes.ITEM_MODIFIED_APPEARANCE, HotfixStatuses.VALID);

            return result;
        }

        ItemSearchName BuildItemSearchName(ItemDto itemDto)
        {
            var result = new ItemSearchName()
            {
                Id = itemDto.Id,
                Display = itemDto.Name ?? Default.ItemSearchName.Display,
                Flags0 = itemDto.Flags0 ?? Default.ItemSearchName.Flags0,
                Flags1 = itemDto.Flags1 ?? Default.ItemSearchName.Flags1,
                Flags2 = itemDto.Flags2 ?? Default.ItemSearchName.Flags2,
                Flags3 = itemDto.Flags3 ?? Default.ItemSearchName.Flags3,
                ItemLevel = itemDto.ItemLevel ?? Default.ItemSearchName.ItemLevel,
                OverallQualityId = itemDto.OverallQuality ?? Default.ItemSearchName.OverallQualityId,
                RequiredLevel = itemDto.RequiredLevel ?? Default.ItemSearchName.RequiredLevel,
                AllowableClass = itemDto.AllowableClasses ?? Default.ItemSearchName.AllowableClass,
                AllowableRace = itemDto.AllowableRaces ?? Default.ItemSearchName.AllowableRace,
                VerifiedBuild = VerifiedBuild
            };

            itemDto.AddHotfix(itemDto.Id, TableHashes.ITEM_SEARCH_NAME, HotfixStatuses.VALID);

            return result;
        }

        ItemSparse BuildItemSparse(ItemDto itemDto)
        {
            var result = new ItemSparse()
            {
                Id = itemDto.Id,
                VerifiedBuild = VerifiedBuild,
                InventoryType = itemDto.InventoryType,

                Bonding = itemDto.Bonding ?? Default.ItemSparse.Bonding,
                Display = itemDto.Name ?? Default.ItemSparse.Display,
                Flags0 = itemDto.Flags0 ?? Default.ItemSparse.Flags0,
                Flags1 = itemDto.Flags1 ?? Default.ItemSparse.Flags1,
                Flags2 = itemDto.Flags2 ?? Default.ItemSparse.Flags2,
                Flags3 = itemDto.Flags3 ?? Default.ItemSparse.Flags3,
                ItemLevel = itemDto.ItemLevel ?? Default.ItemSparse.ItemLevel,
                Material = itemDto.Material ?? Default.ItemSparse.Material,
                OverallQualityId = itemDto.OverallQuality ?? Default.ItemSparse.OverallQualityId,
                RequiredLevel = itemDto.RequiredLevel ?? Default.ItemSparse.RequiredLevel,
                AllowableClass = itemDto.AllowableClasses ?? Default.ItemSparse.AllowableClass,
                AllowableRace = itemDto.AllowableRaces ?? Default.ItemSparse.AllowableRace,
                Description = itemDto.Description ?? Default.ItemSparse.Description,
                StatModifierBonusStat0 = itemDto.StatModifierBonusStat0 ?? Default.ItemSparse.StatModifierBonusStat0,
                StatModifierBonusStat1 = itemDto.StatModifierBonusStat1 ?? Default.ItemSparse.StatModifierBonusStat1,
                StatModifierBonusStat2 = itemDto.StatModifierBonusStat2 ?? Default.ItemSparse.StatModifierBonusStat2,
                StatModifierBonusStat3 = itemDto.StatModifierBonusStat3 ?? Default.ItemSparse.StatModifierBonusStat3,
                StatModifierBonusStat4 = itemDto.StatModifierBonusStat4 ?? Default.ItemSparse.StatModifierBonusStat4,
                StatModifierBonusStat5 = itemDto.StatModifierBonusStat5 ?? Default.ItemSparse.StatModifierBonusStat5,
                StatModifierBonusStat6 = itemDto.StatModifierBonusStat6 ?? Default.ItemSparse.StatModifierBonusStat6,
                StatModifierBonusStat7 = itemDto.StatModifierBonusStat7 ?? Default.ItemSparse.StatModifierBonusStat7,
                StatModifierBonusStat8 = itemDto.StatModifierBonusStat8 ?? Default.ItemSparse.StatModifierBonusStat8,
                StatModifierBonusStat9 = itemDto.StatModifierBonusStat9 ?? Default.ItemSparse.StatModifierBonusStat9,
                StatPercentEditor0 = itemDto.StatPercentEditor0 ?? Default.ItemSparse.StatPercentEditor0,
                StatPercentEditor1 = itemDto.StatPercentEditor1 ?? Default.ItemSparse.StatPercentEditor1,
                StatPercentEditor2 = itemDto.StatPercentEditor2 ?? Default.ItemSparse.StatPercentEditor2,
                StatPercentEditor3 = itemDto.StatPercentEditor3 ?? Default.ItemSparse.StatPercentEditor3,
                StatPercentEditor4 = itemDto.StatPercentEditor4 ?? Default.ItemSparse.StatPercentEditor4,
                StatPercentEditor5 = itemDto.StatPercentEditor5 ?? Default.ItemSparse.StatPercentEditor5,
                StatPercentEditor6 = itemDto.StatPercentEditor6 ?? Default.ItemSparse.StatPercentEditor6,
                StatPercentEditor7 = itemDto.StatPercentEditor7 ?? Default.ItemSparse.StatPercentEditor7,
                StatPercentEditor8 = itemDto.StatPercentEditor8 ?? Default.ItemSparse.StatPercentEditor8,
                StatPercentEditor9 = itemDto.StatPercentEditor9 ?? Default.ItemSparse.StatPercentEditor9,

                PriceRandomValue = Default.ItemSparse.PriceRandomValue,
                Stackable = Default.ItemSparse.Stackable,
                VendorStackCount = Default.ItemSparse.VendorStackCount,
                Display1 = Default.ItemSparse.Display1,
                Display2 = Default.ItemSparse.Display2,
                Display3 = Default.ItemSparse.Display3
            };

            itemDto.AddHotfix(itemDto.Id, TableHashes.ITEM_SPARSE, HotfixStatuses.VALID);

            return result;
        }

        Item BuildItem(ItemDto itemDto)
        {
            var result = new Item()
            {
                Id = itemDto.Id,
                VerifiedBuild = VerifiedBuild,
                SubClassId = itemDto.ItemSubClass,
                ItemGroupSoundsId = itemDto.ItemGroupSoundsId ?? Default.Item.ItemGroupSoundsId,
                ClassId = itemDto.ItemClass,
                InventoryType = itemDto.InventoryType,

                IconFileDataId = itemDto.IconId ?? Default.Item.IconFileDataId,
                Material = itemDto.Material ?? Default.Item.Material,

                SoundOverrideSubClassId = Default.Item.SoundOverrideSubClassId
            };
            itemDto.AddHotfix(itemDto.Id, TableHashes.ITEM, HotfixStatuses.VALID);

            return result;
        }

        ItemDisplayInfo BuildItemDisplayInfo(ItemDto itemDto)
        {
            var result = new ItemDisplayInfo()
            {
                Id = itemDto.Id,
                VerifiedBuild = VerifiedBuild,

                Flags = itemDto.ItemDisplayInfoFlags ?? Default.ItemDisplayInfo.Flags,
                ModelMaterialResourcesId0 = itemDto.ModelMaterialResourceId0 ?? Default.ItemDisplayInfo.ModelMaterialResourcesId0,
                ModelMaterialResourcesId1 = itemDto.ModelMaterialResourceId1 ?? Default.ItemDisplayInfo.ModelMaterialResourcesId1,
                ModelResourcesId0 = itemDto.ModelResourceId0 ?? Default.ItemDisplayInfo.ModelResourcesId0,
                ModelResourcesId1 = itemDto.ModelResourceId1 ?? Default.ItemDisplayInfo.ModelResourcesId1,
                AttachmentGeosetGroup0 = itemDto.GeosetGroupAttachment0 ?? Default.ItemDisplayInfo.AttachmentGeosetGroup0,
                AttachmentGeosetGroup1 = itemDto.GeosetGroupAttachment1 ?? Default.ItemDisplayInfo.AttachmentGeosetGroup1,
                AttachmentGeosetGroup2 = itemDto.GeosetGroupAttachment2 ?? Default.ItemDisplayInfo.AttachmentGeosetGroup2,
                AttachmentGeosetGroup3 = itemDto.GeosetGroupAttachment3 ?? Default.ItemDisplayInfo.AttachmentGeosetGroup3,
                AttachmentGeosetGroup4 = itemDto.GeosetGroupAttachment4 ?? Default.ItemDisplayInfo.AttachmentGeosetGroup4,
                AttachmentGeosetGroup5 = itemDto.GeosetGroupAttachment5 ?? Default.ItemDisplayInfo.AttachmentGeosetGroup5,
                GeosetGroup0 = itemDto.GeosetGroup0 ?? Default.ItemDisplayInfo.GeosetGroup0,
                GeosetGroup1 = itemDto.GeosetGroup1 ?? Default.ItemDisplayInfo.GeosetGroup1,
                GeosetGroup2 = itemDto.GeosetGroup2 ?? Default.ItemDisplayInfo.GeosetGroup2,
                GeosetGroup3 = itemDto.GeosetGroup3 ?? Default.ItemDisplayInfo.GeosetGroup3,
                GeosetGroup4 = itemDto.GeosetGroup4 ?? Default.ItemDisplayInfo.GeosetGroup4,
                GeosetGroup5 = itemDto.GeosetGroup5 ?? Default.ItemDisplayInfo.GeosetGroup5,
                HelmetGeosetVis0 = itemDto.HelmetGeosetVis0 ?? Default.ItemDisplayInfo.HelmetGeosetVis0,
                HelmetGeosetVis1 = itemDto.HelmetGeosetVis1 ?? Default.ItemDisplayInfo.HelmetGeosetVis1,
                ModelType0 = itemDto.ModelType0 ?? Default.ItemDisplayInfo.ModelType0, 
                ModelType1 = itemDto.ModelType1 ?? Default.ItemDisplayInfo.ModelType1,

                ItemRangedDisplayInfoID = Default.ItemDisplayInfo.ItemRangedDisplayInfoID,
                ItemVisual = Default.ItemDisplayInfo.ItemVisual,
                OverrideSwooshSoundKitID = Default.ItemDisplayInfo.OverrideSwooshSoundKitID,
                ParticleColorID = Default.ItemDisplayInfo.ParticleColorID,
                SheathedSpellVisualKitID = Default.ItemDisplayInfo.SheathedSpellVisualKitID,
                SheatheTransformMatrixID = Default.ItemDisplayInfo.SheatheTransformMatrixID,
                StateSpellVisualKitID = Default.ItemDisplayInfo.StateSpellVisualKitID,
                UnsheathedSpellVisualKitID = Default.ItemDisplayInfo.UnsheathedSpellVisualKitID
            };

            itemDto.AddHotfix(itemDto.Id, TableHashes.ITEM_DISPLAY_INFO, HotfixStatuses.VALID);
            
            return result;
        }

        List<ItemXItemEffect> BuildItemXItemEffects(ItemDto itemDto)
        {
            var result = new List<ItemXItemEffect>();
            int index = 0;
            foreach(var effect in itemDto.Effects)
            {
                result.Add(new ItemXItemEffect()
                {
                    Id = itemDto.Id + index,
                    ItemEffectId = itemDto.Id + index,
                    ItemId = itemDto.Id,
                    VerifiedBuild = VerifiedBuild
                });
                itemDto.AddHotfix(itemDto.Id + index, TableHashes.ITEM_X_ITEM_EFFECT, HotfixStatuses.VALID);
                index++;
            }
            return result;
        }

        List<ItemEffect> BuildItemEffects(ItemDto itemDto)
        {
            var result = new List<ItemEffect>();
            int index = 0;
            foreach (var effect in itemDto.Effects)
            {
                result.Add(new ItemEffect()
                {
                    Id = itemDto.Id + index,
                    VerifiedBuild = VerifiedBuild,

                    CategoryCoolDownMSec = effect.CategoryCoolDownMSec ?? Default.ItemEffect.CategoryCoolDownMSec,
                    Charges = effect.Charges ?? Default.ItemEffect.Charges,
                    CoolDownMSec = effect.CoolDownMSec ?? Default.ItemEffect.CoolDownMSec,
                    SpellCategoryId = effect.SpellCategoryId ?? Default.ItemEffect.SpellCategoryId,
                    SpellId = effect.SpellId ?? Default.ItemEffect.SpellId,
                    TriggerType = effect.TriggerType ?? Default.ItemEffect.TriggerType 
                });
                itemDto.AddHotfix(itemDto.Id + index, TableHashes.ITEM_EFFECT, HotfixStatuses.VALID);
                index++;
            }
            return result;
        }
    }
}
