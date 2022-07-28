using HotfixMods.Core.Enums;
using HotfixMods.Infrastructure.DtoEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class ItemDto
    {
        public int? Id { get; set; }
        public int? ItemDisplayInfoId { get; set; }
        public int? IconId { get; set; }
        public int? RequiredLevel { get; set; }
        public int? ItemLevel { get; set; }
        public ItemBondings? Bonding { get; set; }
        public OverallQualities? Quality { get; set; }
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


        public bool IsUpdate { get; set; }
    }
}
