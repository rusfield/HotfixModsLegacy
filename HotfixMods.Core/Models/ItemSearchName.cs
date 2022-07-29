using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ItemSearchName : IHotfixesSchema, IDb2
    {
        [Key]
        public int Id { get; set; }
        public string Display { get; set; } //name
        public OverallQualities OverallQualityId { get; set; }
        public int RequiredLevel { get; set; }
        public int ItemLevel { get; set; }
        [Column("Flags1")]
        public ItemFlags0 Flags0 { get; set; }
        [Column("Flags2")]
        public ItemFlags1 Flags1 { get; set; }
        [Column("Flags3")]
        public ItemFlags2 Flags2 { get; set; }
        [Column("Flags4")]
        public ItemFlags3 Flags3 { get; set; }
        public ItemRaceFlags AllowableRace { get; set; }
        public ItemClassFlags AllowableClass { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
