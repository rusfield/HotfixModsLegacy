using HotfixMods.Core.Enums;
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
        [Column("Display_lang")]
        public string Display { get; set; } //name
        public OverallQualities OverallQuality { get; set; }
        public int RequiredLevel { get; set; }
        public int ItemLevel { get; set; }
        public ItemFlags0 Flags0 { get; set; }
        public ItemFlags1 Flags1 { get; set; }
        public ItemFlags2 Flags2 { get; set; }
        public ItemFlags3 Flags3 { get; set; }
    }
}
