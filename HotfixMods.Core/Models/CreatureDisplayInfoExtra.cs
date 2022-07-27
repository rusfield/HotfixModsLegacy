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
    public class CreatureDisplayInfoExtra : IHotfixesSchema, IDb2
    {
        [Key]
        public int Id { get; set; }
        public Races DisplayRaceId { get; set; }
        public Genders DisplaySexId { get; set; }

    }
}
