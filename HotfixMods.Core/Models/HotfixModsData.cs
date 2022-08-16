using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class HotfixModsData : IHotfixesSchema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HotfixModsRecordTypes RecordType { get; set; }
        public int RecordId { get; set; }
        public string Comment { get; set; }
    }
}
