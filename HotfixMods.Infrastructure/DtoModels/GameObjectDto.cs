using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class GameObjectDto : Dto
    {
        public string? Name { get; set; }
        public GameObjectTypes? Type { get; set; }
        public string? CastBarCaption { get; set; }
        public int? Faction { get; set; }
        public GameObjectAddonFlags? Flags { get; set; }
        public decimal? Size { get; set; }
        public decimal? GeoBox0 { get; set; }
        public decimal? GeoBox1 { get; set; }
        public decimal? GeoBox2 { get; set; }
        public decimal? GeoBox3 { get; set; }
        public decimal? GeoBox4 { get; set; }
        public decimal? GeoBox5 { get; set; }
        public int? FileDataId { get; set; }
    }
}
