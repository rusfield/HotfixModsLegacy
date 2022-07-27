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
    public class ItemInstance : ICharactersSchema
    {
        [Key]
        public int Guid { get; set; }
        [Column("owner_guid")]
        public int OwnerGuid { get; set; }
        public int ItemEntry { get; set; }
        public string Enchantments { get; set; }
        public string BonusListIds { get; set; }
    }
}
