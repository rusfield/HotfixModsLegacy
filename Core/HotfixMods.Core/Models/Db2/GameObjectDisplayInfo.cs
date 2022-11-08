using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class GameobjectDisplayInfo
    {
        public int Id { get; set; } = 0;
        public decimal GeoBox1 { get; set; } = 0;
        public decimal GeoBox2 { get; set; } = 0;
        public decimal GeoBox3 { get; set; } = 0;
        public decimal GeoBox4 { get; set; } = 0;
        public decimal GeoBox5 { get; set; } = 0;
        public decimal GeoBox6 { get; set; } = 0;
        public int FileDataId { get; set; } = 0;
        public short ObjectEffectPackageId { get; set; } = 0;
        public decimal OverrideLootEffectScale { get; set; } = 0;
        public decimal OverrideNameScale { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
