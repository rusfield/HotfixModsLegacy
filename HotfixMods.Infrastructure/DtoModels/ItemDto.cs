﻿using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using HotfixMods.Infrastructure.DtoModels.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class ItemDto : Dto
    {
        public int? ItemDisplayInfoId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? IconId { get; set; }
        public int? RequiredLevel { get; set; }
        public int? ItemLevel { get; set; }
        public ItemBondings? Bonding { get; set; }
        public OverallQualities? OverallQuality { get; set; }
        public ItemMaterial? Material { get; set; }
        public int? ModelResourceId0 { get; set; }
        public int? ModelResourceId1 { get; set; }
        public int? ModelMaterialResourceId0 { get; set; }
        public int? ModelMaterialResourceId1 { get; set; }
        public int? ComponentArmUpper { get; set; }
        public int? ComponentArmLower { get; set; }
        public int? ComponentHand { get; set; }
        public int? ComponentTorsoUpper { get; set; }
        public int? ComponentTorsoLower { get; set; }
        public int? ComponentLegUpper { get; set; }
        public int? ComponentLegLower { get; set; }
        public int? ComponentFoot { get; set; }
        public int? ComponentAccessory { get; set; }
        public int? GeosetGroup0 { get; set; }
        public int? GeosetGroup1 { get; set; }
        public int? GeosetGroup2 { get; set; }
        public int? GeosetGroup3 { get; set; }
        public int? GeosetGroup4 { get; set; }
        public int? GeosetGroup5 { get; set; }
        public int? GeosetGroupAttachment0 { get; set; }
        public int? GeosetGroupAttachment1 { get; set; }
        public int? GeosetGroupAttachment2 { get; set; }
        public int? GeosetGroupAttachment3 { get; set; }
        public int? GeosetGroupAttachment4 { get; set; }
        public int? GeosetGroupAttachment5 { get; set; }
        public int? ModelType0 { get; set; }
        public int? ModelType1 { get; set; }
        public int? HelmetGeosetVis0 { get; set; }
        public int? HelmetGeosetVis1 { get; set; }
        public int? ItemGroupSoundsId { get; set; }
        public int? StatPercentEditor0 { get; set; }
        public int? StatPercentEditor1 { get; set; }
        public int? StatPercentEditor2 { get; set; }
        public int? StatPercentEditor3 { get; set; }
        public int? StatPercentEditor4 { get; set; }
        public int? StatPercentEditor5 { get; set; }
        public int? StatPercentEditor6 { get; set; }
        public int? StatPercentEditor7 { get; set; }
        public int? StatPercentEditor8 { get; set; }
        public int? StatPercentEditor9 { get; set; }
        public ItemStatType? StatModifierBonusStat0 { get; set; }
        public ItemStatType? StatModifierBonusStat1 { get; set; }
        public ItemStatType? StatModifierBonusStat2 { get; set; }
        public ItemStatType? StatModifierBonusStat3 { get; set; }
        public ItemStatType? StatModifierBonusStat4 { get; set; }
        public ItemStatType? StatModifierBonusStat5 { get; set; }
        public ItemStatType? StatModifierBonusStat6 { get; set; }
        public ItemStatType? StatModifierBonusStat7 { get; set; }
        public ItemStatType? StatModifierBonusStat8 { get; set; }
        public ItemStatType? StatModifierBonusStat9 { get; set; }
        public ItemFlags0? Flags0 { get; set; }
        public ItemFlags1?  Flags1 { get; set; }
        public ItemFlags2? Flags2 { get; set; }
        public ItemFlags3? Flags3 { get; set; }
        public ItemDisplayInfoFlags? ItemDisplayInfoFlags { get; set; }
        public ItemRaceFlags? AllowableRaces { get; set; }
        public ItemClassFlags? AllowableClasses { get; set; }
        public SocketTypes? SocketType0 { get; set; }
        public SocketTypes? SocketType1 { get; set; }
        public SocketTypes? SocketType2 { get; set; }
        public int? SocketMatchEnchantmentId { get; set; }
        public ItemSheatheTypes? SheatheType { get; set; }
        public List<ItemEffectDto> Effects { get; set; }
        public DisplayTypes DisplayType { get; set; }
        public InventoryTypes InventoryType { get; set; }
        public ItemClasses ItemClass { get; set; }
        public int ItemSubClass { get; set; }
    }
}
