using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class GameobjectDisplayInfo
    {
        public uint ID { get; set; } = 0;
        public decimal GeoBox1 { get; set; } = 0;
        public decimal GeoBox2 { get; set; } = 0;
        public decimal GeoBox3 { get; set; } = 0;
        public decimal GeoBox4 { get; set; } = 0;
        public decimal GeoBox5 { get; set; } = 0;
        public decimal GeoBox6 { get; set; } = 0;
        public int FileDataID { get; set; } = 0;
        public short ObjectEffectPackageID { get; set; } = 0;
        public decimal OverrideLootEffectScale { get; set; } = 0;
        public decimal OverrideNameScale { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
