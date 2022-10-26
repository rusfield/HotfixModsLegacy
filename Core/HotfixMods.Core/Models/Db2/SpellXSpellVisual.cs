using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellXSpellVisual
    {
        public int Id { get; set; }
        public byte DifficultyId { get; set; }
        public uint SpellVisualId { get; set; }
        public decimal Probability { get; set; }
        public int Field_10_0_0_44795_004 { get; set; }
        public int Priority { get; set; }
        public int SpellIconFileId { get; set; }
        public int ActiveIconFileId { get; set; }
        public ushort ViewerUnitConditionId { get; set; }
        public uint ViewerPlayerConditionId { get; set; }
        public ushort CasterUnitConditionId { get; set; }
        public uint CasterPlayerConditionId { get; set; }
        public int SpellId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
