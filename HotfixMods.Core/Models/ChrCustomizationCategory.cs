using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ChrCustomizationCategory : IDb2
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int OrderIndex { get; set; }
    }
}
