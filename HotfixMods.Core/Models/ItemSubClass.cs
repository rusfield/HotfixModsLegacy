using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ItemSubClass : IDb2
    {
        public string DisplayName { get; set; }
        public string VerboseName { get; set; }
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int SubClassId { get; set; }
    }
}
