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
    public class ItemAppearance : IDb2, IHotfixesSchema
    {
        [Key]
        public int Id { get; set; }
        public int ItemDisplayInfoId { get; set; }
        public DisplayTypes DisplayType { get; set; }
        public int DefaultFileDataId { get; set; }
        public int UiOrder { get; set; }
    }
}
