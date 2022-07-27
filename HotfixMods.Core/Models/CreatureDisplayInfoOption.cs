using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class CreatureDisplayInfoOption : IHotfixesSchema, IDb2
    {
        [Key]
        public int Id { get; set; }
        public int ChrCustomizationOptionId { get; set; }
        public int ChrCustomizationChoiceId { get; set; }
        public int CreatureDisplayInfoExtraId { get; set; }
    }
}
