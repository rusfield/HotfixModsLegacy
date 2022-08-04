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
    public class ItemDisplayInfoMaterialRes : IHotfixesSchema, IDb2
    {
        [Key]
        public int Id { get; set; }
        public ComponentSections ComponentSection { get; set; }
        [Column("MaterialResourceId")]
        public int MaterialResourcesId { get; set; }
        public int ItemDisplayInfoId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
