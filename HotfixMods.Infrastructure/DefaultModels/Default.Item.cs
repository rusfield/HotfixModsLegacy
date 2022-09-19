using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using HotfixMods.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DefaultModels
{
    public static partial class Default
    {

        public static readonly ItemAppearance ItemAppearance = new()
        {
            DefaultIconFileDataId = 0,

            ItemDisplayInfoId = -1,
            DisplayType = (DisplayTypes)(-1),
            Id = -1,
            VerifiedBuild = -1,
            UiOrder = -1
        };

        public static readonly ItemSearchName ItemSearchName = new()
        {
            AllowableClass = ItemClassFlags.ALL,
            AllowableRace = ItemRaceFlags.ALL,
            Display = "New Item",
            Flags0 = ItemFlags0.DEFAULT,
            Flags1 = ItemFlags1.DEFAULT,
            Flags2 = ItemFlags2.DEFAULT,
            Flags3 = ItemFlags3.DEFAULT,
            ItemLevel = 1,
            RequiredLevel = 1,
            OverallQualityId = OverallQualities.COMMON,
            
            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly ItemSparse ItemSparse = new()
        {
            Flags0 = ItemFlags0.DEFAULT,
            Flags1 = ItemFlags1.DEFAULT,
            Flags2 = ItemFlags2.DEFAULT,
            Flags3 = ItemFlags3.DEFAULT,
            OverallQualityId = OverallQualities.COMMON,
            AllowableClass = ItemClassFlags.ALL,
            AllowableRace = ItemRaceFlags.ALL,
            Bonding = ItemBondings.NOT_BOUND,
            Description = "",
            Display = "New Item",
            PriceRandomValue = 1,
            Stackable = 1,
            VendorStackCount = 1,
            Display1 = "",
            Display2 = "",
            Display3 = "",
            ItemLevel = 1,
            Material = ItemMaterial.NONE,
            RequiredLevel = 1,
            StatModifierBonusStat0 = (ItemStatType)(-1),
            StatModifierBonusStat1 = (ItemStatType)(-1),
            StatModifierBonusStat2 = (ItemStatType)(-1),
            StatModifierBonusStat3 = (ItemStatType)(-1),
            StatModifierBonusStat4 = (ItemStatType)(-1),
            StatModifierBonusStat5 = (ItemStatType)(-1),
            StatModifierBonusStat6 = (ItemStatType)(-1),
            StatModifierBonusStat7 = (ItemStatType)(-1),
            StatModifierBonusStat8 = (ItemStatType)(-1),
            StatModifierBonusStat9 = (ItemStatType)(-1),
            StatPercentEditor0 = -1,
            StatPercentEditor1 = -1,
            StatPercentEditor2 = -1,
            StatPercentEditor3 = -1,
            StatPercentEditor4 = -1,
            StatPercentEditor5 = -1,
            StatPercentEditor6 = -1,
            StatPercentEditor7 = -1,
            StatPercentEditor8 = -1,
            StatPercentEditor9 = -1,
            SheatheType = ItemSheatheTypes.NONE,
            SocketMatchEnchantmentId = 0,
            SocketType0 = SocketTypes.NONE,
            SocketType1 = SocketTypes.NONE,
            SocketType2 = SocketTypes.NONE,

            InventoryType = (InventoryTypes)(-1),
            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly Item Item = new()
        {
            IconFileDataId = 0,
            ItemGroupSoundsId = 0,
            Material = ItemMaterial.NONE,
            SoundOverrideSubClassId = -1,

            SubClassId = -1,
            InventoryType = (InventoryTypes)(-1),
            ClassId = (ItemClasses)(-1),
            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly ItemDisplayInfo ItemDisplayInfo = new()
        {
            AttachmentGeosetGroup0 = 0,
            AttachmentGeosetGroup1 = 0,
            AttachmentGeosetGroup2 = 0,
            AttachmentGeosetGroup3 = 0,
            AttachmentGeosetGroup4 = 0,
            AttachmentGeosetGroup5 = 0,
            Flags = ItemDisplayInfoFlags.DEFAULT,
            GeosetGroup0 = 0,
            GeosetGroup1 = 0,
            GeosetGroup2 = 0,
            GeosetGroup3 = 0,
            GeosetGroup4 = 0,
            GeosetGroup5 = 0,
            HelmetGeosetVis0 = 0,
            HelmetGeosetVis1 = 0,
            ItemRangedDisplayInfoID = 0,
            ItemVisual = 0,
            OverrideSwooshSoundKitID = 0,
            ParticleColorID = 0,
            SheathedSpellVisualKitID = 0,
            SheatheTransformMatrixID = 0,
            StateSpellVisualKitID = 0,
            UnsheathedSpellVisualKitID = 0,
            ModelMaterialResourcesId0 = 0,
            ModelMaterialResourcesId1 = 0,
            ModelResourcesId0 = 0,
            ModelResourcesId1 = 0,
            ModelType0 = 0,
            ModelType1 = 0,

            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly ItemModifiedAppearance ItemModifiedAppearance = new()
        {
            OrderIndex = 0,
            ItemAppearanceModifierId = 0,

            Id = -1,
            ItemAppearanceId = -1,
            ItemId = -1,
            VerifiedBuild = -1
        };

        public static readonly ItemEffect ItemEffect = new()
        {
            SpellId = 0,
            CategoryCoolDownMSec = 0,
            Charges = 0,
            CoolDownMSec = 0,
            SpellCategoryId = 0,
            TriggerType = ItemTriggerType.ON_EQUIP,

            Id = -1,
            VerifiedBuild = -1
        };

        public static readonly ItemDisplayInfoMaterialRes ItemDisplayInfoMaterialRes = new()
        {
            // Unused
        };
    }
}
