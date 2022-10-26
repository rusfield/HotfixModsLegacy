using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualEffectName
    {
        public int Id { get; set; }
        public int ModelFileDataId { get; set; }
        public decimal BaseMissileSpeed { get; set; }
        public decimal Scale { get; set; }
        public decimal MinAllowedScale { get; set; }
        public decimal MaxAllowedScale { get; set; }
        public decimal Alpha { get; set; }
        public uint Flags { get; set; }
        public int TextureFileDataId { get; set; }
        public decimal EffectRadius { get; set; }
        public uint Type { get; set; }
        public int GenericId { get; set; }
        public uint RibbonQualityId { get; set; }
        public int DissolveEffectId { get; set; }
        public int ModelPosition { get; set; }
        public sbyte Field_9_1_0_38549_014 { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
