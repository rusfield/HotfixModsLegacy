using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ItemSet : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemId0 { get; set; }
        public int ItemId1 { get; set; }
        public int ItemId2 { get; set; }
        public int ItemId3 { get; set; }
        public int ItemId4 { get; set; }
        public int ItemId5 { get; set; }
        public int ItemId6 { get; set; }
        public int ItemId7 { get; set; }
        public int ItemId8 { get; set; }
        public int ItemId9 { get; set; }
        public int ItemId10 { get; set; }
        public int ItemId11 { get; set; }
        public int ItemId12 { get; set; }
        public int ItemId13 { get; set; }
        public int ItemId14 { get; set; }
        public int ItemId15 { get; set; }
        public int ItemId16 { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
