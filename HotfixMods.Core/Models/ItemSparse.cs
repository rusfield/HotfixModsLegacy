using HotfixMods.Core.Models.Interfaces;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotfixMods.Core.Models
{
    public class ItemSparse : IHotfixesSchema, IDb2
    {
        [Key]
        public int Id { get; set; }
        public string Display { get; set; } // Name
        public int Stackable { get; set; } // 1
        public int VendorStackCount { get; set; } // 1
        public int PriceRandomValue { get; set; } // 1
        [Column("Flags1")]
        public ItemFlags0 Flags0 { get; set; }
        [Column("Flags2")]
        public ItemFlags1 Flags1 { get; set; }
        [Column("Flags3")]
        public ItemFlags2 Flags2 { get; set; }
        [Column("Flags4")]
        public ItemFlags3 Flags3 { get; set; }
        public int ItemLevel { get; set; }
        public ItemMaterial Material { get; set; }
        public ItemBondings Bonding { get; set; }
        public int RequiredLevel { get; set; }
        public InventoryTypes InventoryType { get; set; }
        public OverallQualities OverallQualityId { get; set; }
        public ItemRaceFlags AllowableRace { get; set; }
        public ItemClassFlags AllowableClass { get; set; }
        public string Display1 { get; set; }
        public string Display2 { get; set; }
        public string Display3 { get; set; }
        public string Description { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
