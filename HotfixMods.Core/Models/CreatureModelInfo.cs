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
    public class CreatureModelInfo : IWorldSchema
    {
        [Key]
        public int DisplayId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
