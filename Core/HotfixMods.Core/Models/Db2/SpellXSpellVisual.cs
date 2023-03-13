using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellXSpellVisual
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public byte DifficultyID { get; set; } = 0;
        public uint SpellVisualID { get; set; } = 0;
        public decimal Probability { get; set; } = 0;
        public int Flags2 { get; set; } = 0;
        public int Priority { get; set; } = 0;
        public int SpellIconFileID { get; set; } = 0;
        public int ActiveIconFileID { get; set; } = 0;
        public ushort ViewerUnitConditionID { get; set; } = 0;
        public uint ViewerPlayerConditionID { get; set; } = 0;
        public ushort CasterUnitConditionID { get; set; } = 0;
        public uint CasterPlayerConditionID { get; set; } = 0;
        [ParentIndexField]
        public int SpellID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
