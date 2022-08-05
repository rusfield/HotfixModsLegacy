using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class Item : IHotfixesSchema, IDb2
    {
        [Key]
        public int Id { get; set; }
        public ItemClass ClassId { get; set; }
        public int SubClassId { get; set; } // A bunch of enums, based on ClassId
        public ItemMaterial Material { get; set; }
        public InventoryTypes InventoryType { get; set; }
        //public int SheatheType { get; set; } // TODO: Look into
        public int IconFileDataId { get; set; }
        public int VerifiedBuild { get; set; }
        public int SoundOverrideSubClassId { get; set; }
        public int ItemGroupSoundsId { get; set; }
    }
}
