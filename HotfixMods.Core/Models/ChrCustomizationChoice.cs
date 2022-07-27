using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ChrCustomizationChoice : IDb2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChrCustomizationOptionId { get; set; }
    }
}
