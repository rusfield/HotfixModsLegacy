using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class GameObjectDisplayInfo : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public decimal GeoBox0 { get; set; }
        public decimal GeoBox1 { get; set; }
        public decimal Geobox2 { get; set; }
        public decimal Geobox3 { get; set; }
        public decimal Geobox4 { get; set; }
        public decimal Geobox5 { get; set; }
        public int FileDataId { get; set; }
    }
}
