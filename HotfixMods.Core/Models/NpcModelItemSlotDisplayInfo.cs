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
    public class NpcModelItemSlotDisplayInfo : IHotfixesSchema, IDb2
    {
        [Key]
        public int Id { get; set; }
        public int ItemDisplayInfoId { get; set; }
        public ArmorSlots ItemSlot { get; set; }
        public int NpcModelId { get; set; } // References Id of CreatureDisplayInfoExtra (textures, etc).
    }
}
