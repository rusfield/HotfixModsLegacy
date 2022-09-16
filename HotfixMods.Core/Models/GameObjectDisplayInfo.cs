using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class GameObjectDisplayInfo : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        [Column("GeoBoxMinX")]
        public decimal GeoBox0 { get; set; }
        [Column("GeoBoxMinY")]
        public decimal GeoBox1 { get; set; }
        [Column("GeoBoxMinZ")]
        public decimal GeoBox2 { get; set; }
        [Column("GeoBoxMaxX")]
        public decimal GeoBox3 { get; set; }
        [Column("GeoBoxMaxY")]
        public decimal GeoBox4 { get; set; }
        [Column("GeoBoxMaxZ")]
        public decimal GeoBox5 { get; set; }
        public int FileDataId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
