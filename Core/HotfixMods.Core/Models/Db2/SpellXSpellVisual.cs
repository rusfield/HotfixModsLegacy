using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellXSpellVisual
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public byte DifficultyId { get; set; } = 0;
        public uint SpellVisualId { get; set; } = 0;
        public decimal Probability { get; set; } = 0;
        public int Flags2 { get; set; } = 0;
        public int Priority { get; set; } = 0;
        public int SpellIconFileId { get; set; } = 0;
        public int ActiveIconFileId { get; set; } = 0;
        public ushort ViewerUnitConditionId { get; set; } = 0;
        public uint ViewerPlayerConditionId { get; set; } = 0;
        public ushort CasterUnitConditionId { get; set; } = 0;
        public uint CasterPlayerConditionId { get; set; } = 0;
        [ParentIndexField]
        public int SpellId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
