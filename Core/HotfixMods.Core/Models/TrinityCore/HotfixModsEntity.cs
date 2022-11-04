using HotfixMods.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models.TrinityCore
{
    public class HotfixModsEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TableHashes TableHash { get; set; }
        public int RecordId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
