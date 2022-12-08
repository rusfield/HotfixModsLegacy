using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellEffectEmission
    {
        public int Id { get; set; } = 1;
        public decimal EmissionRate { get; set; }
        public decimal ModelScale { get; set; }
        public short AreaModelId { get; set; }
        public sbyte Flags { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
