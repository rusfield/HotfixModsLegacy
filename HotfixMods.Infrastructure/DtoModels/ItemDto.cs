using HotfixMods.Core.Enums;
using HotfixMods.Infrastructure.DtoEnums;
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
        public int? IconId { get; set; }
        public int? RequiredLevel { get; set; }
        public int? ItemLevel { get; set; }
        public ItemBondings? Bonding { get; set; }
        public OverallQualities? OverallQuality { get; set; }
        public ItemMaterial? Material { get; set; }
        public ItemTypeDtoEnum? ItemType { get; set; }
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
        public ItemFlags0? Flags0 { get; set; }
        public ItemFlags1?  Flags1 { get; set; }
        public ItemFlags2? Flags2 { get; set; }
        public ItemFlags3? Flags3 { get; set; }
        public ItemDisplayInfoFlags? ItemDisplayInfoFlags { get; set; }

    }
}
