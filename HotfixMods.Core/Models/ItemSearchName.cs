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

        // TODO: Look into 
        public int Flags0 { get; set; }
        public int Flags1 { get; set; }
        public int Flags2 { get; set; }
        public int Flags3 { get; set; }
    }
}
