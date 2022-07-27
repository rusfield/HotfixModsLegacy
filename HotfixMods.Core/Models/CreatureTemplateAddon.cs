using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class CreatureTemplateAddon : IWorldSchema
    {
        [Key]
        public int Entry { get; set; }
        public string Auras { get; set; }
    }
}
