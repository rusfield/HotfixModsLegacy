using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ItemDisplayInfo : IHotfixesSchema, IDb2
    {
        public int Id { get; set; }
        public ItemDisplayInfoFlags Flags { get; set; }
        public int ModelResourcesId0 { get; set; }
        public int ModelResourcesId1 { get; set; }
        public int ModelMaterialResourcesId0 { get; set; }
        public int ModelMaterialResourcesId1 { get; set; }
    }
}
