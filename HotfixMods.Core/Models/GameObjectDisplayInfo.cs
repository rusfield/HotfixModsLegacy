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
        public decimal GeoBox2 { get; set; }
        public decimal GeoBox3 { get; set; }
        public decimal GeoBox4 { get; set; }
        public decimal GeoBox5 { get; set; }
        public int FileDataId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
