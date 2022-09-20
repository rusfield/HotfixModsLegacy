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
        public decimal PriceRandomValue { get; set; } // 1
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
        [Column("StatPercentEditor1")]
        public int StatPercentEditor0 { get; set; }
        [Column("StatPercentEditor2")]
        public int StatPercentEditor1 { get; set; }
        [Column("StatPercentEditor3")]
        public int StatPercentEditor2 { get; set; }
        [Column("StatPercentEditor4")]
        public int StatPercentEditor3 { get; set; }
        [Column("StatPercentEditor5")]
        public int StatPercentEditor4 { get; set; }
        [Column("StatPercentEditor6")]
        public int StatPercentEditor5 { get; set; }
        [Column("StatPercentEditor7")]
        public int StatPercentEditor6 { get; set; }
        [Column("StatPercentEditor8")]
        public int StatPercentEditor7 { get; set; }
        [Column("StatPercentEditor9")]
        public int StatPercentEditor8 { get; set; }
        [Column("StatPercentEditor10")]
        public int StatPercentEditor9 { get; set; }
        [Column("StatModifierBonusStat1")]
        public ItemStatType StatModifierBonusStat0 { get; set; }
        [Column("StatModifierBonusStat2")]
        public ItemStatType StatModifierBonusStat1 { get; set; }
        [Column("StatModifierBonusStat3")]
        public ItemStatType StatModifierBonusStat2 { get; set; }
        [Column("StatModifierBonusStat4")]
        public ItemStatType StatModifierBonusStat3 { get; set; }
        [Column("StatModifierBonusStat5")]
        public ItemStatType StatModifierBonusStat4 { get; set; }
        [Column("StatModifierBonusStat6")]
        public ItemStatType StatModifierBonusStat5 { get; set; }
        [Column("StatModifierBonusStat7")]
        public ItemStatType StatModifierBonusStat6 { get; set; }
        [Column("StatModifierBonusStat8")]
        public ItemStatType StatModifierBonusStat7 { get; set; }
        [Column("StatModifierBonusStat9")]
        public ItemStatType StatModifierBonusStat8 { get; set; }
        [Column("StatModifierBonusStat10")]
        public ItemStatType StatModifierBonusStat9 { get; set; }
        [Column("SocketType1")]
        public SocketTypes SocketType0 { get; set; }
        [Column("SocketType2")]
        public SocketTypes SocketType1 { get; set; }
        [Column("SocketType3")]
        public SocketTypes SocketType2 { get; set; }
        public int SocketMatchEnchantmentId { get; set; }
        public ItemSheatheTypes SheatheType { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
