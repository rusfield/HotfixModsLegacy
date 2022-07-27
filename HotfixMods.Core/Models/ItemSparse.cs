using HotfixMods.Core.Models.Interfaces;
using HotfixMods.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ItemSparse : IHotfixesSchema, IDb2
    {
        [Key]
        public int Id { get; set; }
        [Column("Display_lang")]
        public string Display { get; set; } // Name
        public int Stackable { get; set; } // 1
        public int VendorStackCount { get; set; } // 1
        public int PriceRandomValue { get; set; } // 1
        public int Flags0 { get; set; }
        public int Flags1 { get; set; }
        public int Flags2 { get; set; }
        public int Flags3 { get; set; }
        public int ItemLevel { get; set; }
        public ItemMaterial Material { get; set; }
        public ItemBondings Bonding { get; set; }
        public int RequiredLevel { get; set; }
        public InventoryTypes InventoryType { get; set; }
        public OverallQualities OverallQualityId { get; set; }
    }
}
