using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfoGeosetData
    {
        
        public int ID { get; set; } = 0;
        public byte GeosetIndex { get; set; }
        public byte GeosetValue { get; set; }
        
        public int CreatureDisplayInfoID { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}