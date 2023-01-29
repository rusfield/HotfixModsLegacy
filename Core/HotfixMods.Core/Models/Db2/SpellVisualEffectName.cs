using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualEffectName
    {
        [IndexField]
        public uint Id { get; set; } = 0;
        public int ModelFileDataId { get; set; } = 0;
        public decimal BaseMissileSpeed { get; set; } = 0;
        public decimal Scale { get; set; } = 1;
        public decimal MinAllowedScale { get; set; } = 0.01M;
        public decimal MaxAllowedScale { get; set; } = 100;
        public decimal Alpha { get; set; } = 1;
        public uint Flags { get; set; } = 0;
        public int TextureFileDataId { get; set; } = 0;
        public decimal EffectRadius { get; set; } = 0;
        public uint Type { get; set; } = 0;
        public int GenericId { get; set; } = 0;
        public uint RibbonQualityId { get; set; } = 0;
        public int DissolveEffectId { get; set; } = 0;
        public int ModelPosition { get; set; } = -1;
        public sbyte Field_9_1_0_38549_014 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
