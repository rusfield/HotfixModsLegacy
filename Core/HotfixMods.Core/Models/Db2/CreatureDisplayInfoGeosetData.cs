using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfoGeosetData
    {
        public int Id { get; set; } = 1;
        public byte GeosetIndex { get; set; }
        public byte GeosetValue { get; set; }
        public int CreatureDisplayInfoId { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}