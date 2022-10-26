using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class GameObjectDisplayInfo
    {
        public int Id { get; set; }
        public decimal GeoBox1 { get; set; }
        public decimal GeoBox2 { get; set; }
        public decimal GeoBox3 { get; set; }
        public decimal GeoBox4 { get; set; }
        public decimal GeoBox5 { get; set; }
        public decimal GeoBox6 { get; set; }
        public int FileDataId { get; set; }
        public short ObjectEffectPackageId { get; set; }
        public decimal OverrideLootEffectScale { get; set; }
        public decimal OverrideNameScale { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
